using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    #region Variables

    private Rigidbody2D PlayerRB;
    private GameManager gm;
    private bool canMove = true;
    private float repelRange = 1.5f;
    private float repelStrenght = 1f;
    private float lastShoot = float.MinValue;

    public Rigidbody2D rb;
    public Animator animator;
    public GameObject pointer;
    public GameObject bullet;
    public Transform firePos;

    [Space]

    public float speed = 5f;
    public float turnSpeed = .1f;
    public int prob;
    public int pointsAtDeath = 1;
    public int damage;

    public bool isShooter = false;
    public bool isSummoner = false;

    public float fireDistance = 5f;
    public float starfeSpeed = 2f;
    public float fireRate = 1f;

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
        PlayerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        GetComponent<HeathSystem>().DieEvent += Die;
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
        Destroy(gameObject);
    }

    private IEnumerator AnimateDeath()
    {
        animator.SetBool("Death", true);
        canMove = false;
        gameObject.name = "EnemyExplosion";
        gameObject.tag = "EnemyExplosion";
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    public void Die(object sender, HeathSystem.DieEventArgs e)
    {
        gm.EnemyRBs.Remove(rb);
        tag = "EnemyExplosion";
        Destroy(rb);
        Destroy(pointer);

        CapsuleCollider2D[] coll = gameObject.GetComponents<CapsuleCollider2D>();
        for (int i = 0; i < coll.Length; i++)
            Destroy(coll[i]);

        if (e.defeated)
            gm.Score += pointsAtDeath;
        gm.EnemyNumber--;

        StartCoroutine(AnimateDeath());
    }
}