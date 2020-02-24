using System;
using UnityEngine;

public class HeathSystem : MonoBehaviour
{
    private enum OnWho { Player, Enemy };

    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject healtBar;
    [SerializeField] private GameObject healthBarContainer;
    [SerializeField] private OnWho onWho;
    [SerializeField] private int currentHeath;
    private GameObject player;

    #region DieEvent

    public event EventHandler<DieEventArgs> DieEvent;

    public class DieEventArgs : EventArgs
    {
        public bool defeated;
    }

    #endregion DieEvent

    private bool hasDied = false;
    private int lastID = 0;

    private float regenTime = 5f;
    private float regenRate = 2f;
    private float lastRegen = float.MinValue;
    private float lastDamage = float.MinValue;

    private void Start()
    {
        currentHeath = maxHealth;
        if (onWho == OnWho.Player)
            player = gameObject;
        else player = GameObject.FindGameObjectWithTag("Player");
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
        DieEvent?.Invoke(this, new DieEventArgs { defeated = hasBeenDefeated });
    }

    private void EnemyHitPlayer()
    {
        player.GetComponent<HeathSystem>().TakeDamage(GetComponent<EnemyBase>().damage);
        Die(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (onWho == OnWho.Enemy && collider.tag == "Bullet" && collider.GetInstanceID() != lastID)
        {
            lastID = collider.GetInstanceID();
            TakeDamage(collider.GetComponent<Bullet>().damage);
            Destroy(collider.gameObject);
        }
        if (onWho == OnWho.Player && collider.tag == "EnemyBullet" && collider.GetInstanceID() != lastID)
        {
            lastID = collider.GetInstanceID();
            TakeDamage(collider.GetComponent<Bullet>().damage);
        }
        if (onWho == OnWho.Enemy && collider.tag == "Player" && collider.GetInstanceID() != lastID)
        {
            lastID = collider.GetInstanceID();
            EnemyHitPlayer();
        }
    }
}