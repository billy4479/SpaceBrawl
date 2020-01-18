using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private Transform PlayerTransform;
    private GameManager gm;
    private Rigidbody2D rb;
    private Animator animator;
    private bool canMove = true;

    public float speed;
    public int HP;
    public int prob;
    public int pointsAtDeath;

    private void Start()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }


    private void Update()
    {
        if (canMove)
        {
            rb.rotation = (Mathf.Atan2(transform.position.y - PlayerTransform.position.y, transform.position.x - PlayerTransform.position.x) * Mathf.Rad2Deg + 90);
            rb.MovePosition(rb.position + new Vector2(transform.up[0], transform.up[1]) * Time.deltaTime * speed);
        }

        if (HP <= 0 && canMove)
            StartCoroutine(EnemyDeath());
    }
    private IEnumerator EnemyDeath()
    {
        canMove = false;
        gameObject.name = "EnemyExplosion";

        CapsuleCollider2D[] coll = gameObject.GetComponents<CapsuleCollider2D>();
        for (int i = 0; i < coll.Length; i++)
        {
            coll[i].isTrigger = true;
        }
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

        animator.SetBool("Death", true);
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
                    Destroy(enemies);
                }
                gm.PlayerLose();
            }
        }
    }
}
