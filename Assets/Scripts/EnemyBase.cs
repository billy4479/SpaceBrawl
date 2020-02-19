using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    #region Variables
    private Rigidbody2D PlayerRB;
    private GameManager gm;
    private bool canMove = true;
    private float repelRange = 1f;
    private float repelStrenght = 1f;

    public Rigidbody2D rb;
    public Animator animator;
    public GameObject pointer;
    public float speed = 5f;
    public float turnSpeed = .1f;
    public int HP;
    public int prob;
    public int pointsAtDeath = 1;
    public bool isShooter = false;
    public bool isSummoner = false;
    public float fireDistance = 5f;

    private const bool debug = false;
    #endregion

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

                }
            }
            else
                rb.MovePosition(MoveNormally());

        }

        if (HP <= 0 && canMove)
            StartCoroutine(EnemyDeath());

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

    private void Start()
    {
        if (debug)
            speed = 0f;
        if (gm.EnemyRBs == null)
            gm.EnemyRBs = new List<Rigidbody2D>();
        gm.EnemyRBs.Add(rb);
    }

    private void Awake()
    {
        PlayerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private IEnumerator EnemyDeath()
    {
        canMove = false;
        gameObject.name = "EnemyExplosion";
        gameObject.tag = "EnemyExplosion";
        gm.EnemyRBs.Remove(rb);

        CapsuleCollider2D[] coll = gameObject.GetComponents<CapsuleCollider2D>();
        for (int i = 0; i < coll.Length; i++)
        {
            coll[i].isTrigger = true;
        }
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        animator.SetBool("Death", true);
        Destroy(pointer);
        gm.Score += pointsAtDeath;

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
        gm.EnemyNumber--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canMove == true)
        {
            if (collision.name == "Bullet(Clone)")
                HP -= 10;

            if (collision.tag == "Player")
            {
                foreach (GameObject enemies in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    gm.EnemyRBs = new List<Rigidbody2D>();
                    Destroy(enemies);
                }
                Destroy(pointer);
                gm.PlayerLose();
            }
        }
    }

}
