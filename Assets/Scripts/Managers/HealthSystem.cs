using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject healtBar;
    [SerializeField] private GameObject healthBarContainer;
    [HideInInspector] public EnemyStats enemyStats;
    [HideInInspector] public PlayerStats playerStats;
    private int maxHealth;
    private int currentHeath;

    private bool isPlayer;
    private bool hasDied = false;
    private int lastID = 0;
    private IHealth health;

    private float regenTime = 5f;
    private float regenRate = 2f;
    private float lastRegen = float.MinValue;
    private float lastDamage = float.MinValue;

    private void Awake()
    {
        health = GetComponent<IHealth>();
        isPlayer = health.IsPlayer();
    }

    private void Start()
    {
        if (isPlayer) playerStats = health.GetStats() as PlayerStats;
        if (!isPlayer) enemyStats = health.GetStats() as EnemyStats;
        if (playerStats != null && enemyStats != null)
        {
            Debug.LogError("This HeathSystem is used for player and enemy simultaneusly!");
            throw new System.Exception();
        }
        if (playerStats == null && enemyStats == null)
        {
            Debug.LogError("This HealthSystem does not have any stats");
            throw new System.Exception();
        }
        if (enemyStats != null && !isPlayer)
        {
            maxHealth = enemyStats.life;
            regenTime = enemyStats.timeToStartRegen;
            regenRate = enemyStats.regenSpeed;
        }
        if (playerStats != null && isPlayer)
        {
            maxHealth = playerStats.life;
            regenTime = playerStats.timeToStartRegen;
            regenRate = playerStats.regenSpeed;
        }
        currentHeath = maxHealth;
        if (!isPlayer)
        {
            healthBarContainer.transform.localPosition *= enemyStats.scale;
            healthBarContainer.transform.localScale *= enemyStats.scale;
        }
        else
        {
            healthBarContainer.transform.localPosition *= playerStats.scale;
            healthBarContainer.transform.localScale *= playerStats.scale;
        }

    }

    private void Update()
    {
        healthBarContainer.transform.rotation = Quaternion.Euler(
            healthBarContainer.transform.rotation.x,
            healthBarContainer.transform.rotation.y,
            transform.rotation.z
        );
        if (Time.time - lastDamage > regenTime && currentHeath < maxHealth && Time.time - lastRegen > 1f / regenRate)
        {
            Regen();
        }
    }

    private void Regen()
    {
        lastRegen = Time.time;
        currentHeath++;
        UpdateScale();
    }

    private void TakeDamage(int damage)
    {
        currentHeath -= damage;
        lastDamage = Time.time;
        UpdateScale();
        if (currentHeath <= 0 && !hasDied)
            Die(true);
    }

    private void UpdateScale()
    {
        float scale = (float)currentHeath / (float)maxHealth;
        healtBar.transform.localScale = new Vector3(scale, healtBar.transform.localScale.y, healtBar.transform.localScale.z);
    }

    private void Die(bool hasBeenDefeated)
    {
        hasDied = true;
        healthBarContainer.SetActive(false);
        health.OnDeath(hasBeenDefeated);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bullet") && collider.GetInstanceID() != lastID)
        {
            lastID = collider.GetInstanceID();
            var controller = collider.GetComponent<Bullet>();
            if (controller.targetPlayer && isPlayer)
                TakeDamage(controller.damage);
            if (!controller.targetPlayer && !isPlayer)
                TakeDamage(controller.damage);
        }
        if (collider.CompareTag("Player") && collider.GetInstanceID() != lastID && !isPlayer && !hasDied)
        {
            //Se sei il nemico e ti colpisce il player
            lastID = collider.GetInstanceID();
            collider.GetComponent<HealthSystem>().TakeDamage(enemyStats.bodyDamage);
            Die(false);
        }
        if (collider.CompareTag("Enemy") && collider.GetInstanceID() != lastID && isPlayer)
        {
            //Se sei il giocatore e ti colpisce il nemico
            //NON VIENE ESEGUITO
            lastID = collider.GetInstanceID();
            TakeDamage(collider.GetComponent<EnemyBase>().stats.bodyDamage);
        }
    }
}