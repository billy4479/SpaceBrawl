using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private GameObject healtBar;
    [SerializeField] private GameObject healthBarContainer;
    [HideInInspector]
    public EnemyStats enemyStats;
    public PlayerStats playerStats;
    private int maxHealth;
    private int currentHeath;

    private AssetsHolder assetsHolder;
    private GameObject player;
    private bool isPlayer;
    private bool hasDied = false;
    private int lastID = 0;
    private IHealth health;

    private float regenTime = 5f;
    private float regenRate = 2f;
    private float lastRegen = float.MinValue;
    private float lastDamage = float.MinValue;

    private void Start()
    {
        assetsHolder = AssetsHolder.instance;
        player = assetsHolder.Player;
        health = GetComponent<IHealth>();
        isPlayer = health.IsPlayer();
        if(playerStats != null && enemyStats != null)
        {
            Debug.LogError("This HeathSystem is used for player and enemy simultaneusly!");
            throw new System.Exception();
        }
        if(playerStats == null && enemyStats == null)
        {
            Debug.LogError("This HealthSystem does not have any stats");
            throw new System.Exception();
        }
        if(enemyStats != null && !isPlayer)
        {
            maxHealth = enemyStats.life;
            regenTime = enemyStats.timeToStartRegen;
            regenRate = enemyStats.regenSpeed;
        }
        if(playerStats != null && isPlayer)
        {
            maxHealth = playerStats.life;
            regenTime = playerStats.timeToStartRegen;
            regenRate = playerStats.regenSpeed;
        }
        currentHeath = maxHealth;
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

    public void EnemyHasHitPlayer(int id, int damage)
    {
        if (id != lastID)
        {
            lastID = id;
            TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Bullet" && collider.GetInstanceID() != lastID && !isPlayer)
        {
            lastID = collider.GetInstanceID();
            TakeDamage(collider.GetComponent<Bullet>().damage);
            Destroy(collider.gameObject);
        }
        if (collider.tag == "EnemyBullet" && collider.GetInstanceID() != lastID && isPlayer)
        {
            lastID = collider.GetInstanceID();
            TakeDamage(collider.GetComponent<Bullet>().damage);
            Destroy(collider.gameObject);
        }
        if (collider.tag == "Player" && collider.GetInstanceID() != lastID && !isPlayer)
        {
            lastID = collider.GetInstanceID();
            player.GetComponent<HealthSystem>().EnemyHasHitPlayer(GetInstanceID(), GetComponent<EnemyBase>().stats.damage);
            Die(false);
        }
    }
}