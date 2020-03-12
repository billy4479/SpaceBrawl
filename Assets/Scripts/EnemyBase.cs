using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IHealth
{
    #region Variables

    public EnemyStats stats;

    private Rigidbody2D PlayerRB;
    private GameManager gm;
    private bool canMove = true;
    private float repelRange = 1.5f;
    private float repelStrenght = 1f;
    private float lastShoot = float.MinValue;
    private AudioManager am;

    private AssetsHolder assetsHolder;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject pointer;
    private Transform firePos;

    private GameObject bullet;
    private float speed;
    private float turnSpeed;
    private int pointsAtDeath;

    private int prob;
    private int damage;
    private bool isShooter;
    private bool isSummoner;
    private float fireDistance;
    private float starfeSpeed;
    private float fireRate;

    #endregion Variables

    private const bool debug = false;

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 direction = (PlayerRB.position - rb.position).normalized;
            float distance = Vector2.Distance(rb.position, PlayerRB.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, turnSpeed);

            if (isShooter)
            {
                if (distance >= fireDistance)
                    rb.MovePosition(MoveNormally());
                else
                {
                    rb.MovePosition(MoveStarfing());
                    Shoot();
                }
            }
            else
                rb.MovePosition(MoveNormally());
        }
    }

    private void Shoot()
    {
        if (Time.time - lastShoot > 1f / fireRate)
        {
            Instantiate(bullet, firePos.position, firePos.rotation);
            am.PlaySound("EnemyShot");
            lastShoot = Time.time;
        }
    }

    #region Moviment

    private Vector2 MoveStarfing()
    {
        return transform.position + transform.right * Time.fixedDeltaTime * starfeSpeed;
    }

    private Vector2 MoveNormally()
    {
        Vector2 repelForce = Vector2.zero;
        foreach (Rigidbody2D enemy in gm.EnemyRBs)
        {
            if (enemy == rb)
                continue;
            if (Vector2.Distance(enemy.position, rb.position) <= repelRange)
                repelForce += (rb.position - enemy.position).normalized; //<=== direction
        }

        Vector2 newPos = transform.position + transform.up * Time.fixedDeltaTime * speed;
        newPos += repelForce * Time.fixedDeltaTime * repelStrenght;
        return newPos;
    }

    #endregion Moviment

    private void Start()
    {
        am = AudioManager.instance;
        assetsHolder = AssetsHolder.instance;
        gm = GameManager.instance;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = stats.aoc;
        pointer = transform.Find("Pointer").gameObject;
        firePos = transform.Find("FirePosition");
        PlayerRB = assetsHolder.Player.GetComponent<Rigidbody2D>();
        bullet = assetsHolder.Bullet_Enemy;
        speed = stats.speed;
        turnSpeed = stats.rotationSpeed;
        pointsAtDeath = stats.pointAtDeath;
        prob = stats.probability;
        damage = stats.damage;
        isShooter = stats.enemyType == EnemyType.Shooter ? true : false;
        isSummoner = stats.enemyType == EnemyType.Summoner ? true : false;
        fireDistance = stats.fireDistance;
        starfeSpeed = stats.starfeSpeed;
        fireRate = stats.fireRate;
        pointer.GetComponent<SpriteRenderer>().color = stats.arrowColor;
        transform.localScale *= stats.scale;

        if (debug)
            speed = 0f;
        if (gm.EnemyRBs == null)
            gm.EnemyRBs = new List<Rigidbody2D>();
        gm.EnemyRBs.Add(rb);
    }

    public void RemoveEnemy()
    {
        Destroy(pointer);
        gm.EnemyRBs.Remove(rb);
        gm.EnemyNumber--;
        Destroy(gameObject);
    }

    private IEnumerator AnimateDeath()
    {
        animator.SetBool("Death", true);

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void OnDeath(bool defeated)
    {
        tag = "EnemyExplosion";

        gm.EnemyRBs.Remove(rb);
        canMove = false;
        Destroy(rb);
        Destroy(pointer);
        CapsuleCollider2D[] capsuleColl = GetComponents<CapsuleCollider2D>();
        for (int i = 0; i < capsuleColl.Length; i++)
            Destroy(capsuleColl[i]);
        PolygonCollider2D[] polColl = GetComponents<PolygonCollider2D>();
        for (int i = 0; i < polColl.Length; i++)
            Destroy(polColl[i]);

        if (defeated)
            gm.Score += pointsAtDeath;
        gm.EnemyNumber--;
        am.PlaySound("EnemyExplosion");
        StartCoroutine(AnimateDeath());
    }
    public bool IsPlayer() => false;
}