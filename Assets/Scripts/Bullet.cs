using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float timeAlive = 2f;
    public Rigidbody2D rb;
    void Start()
    {
        rb.velocity = transform.up * speed;
        Destroy(gameObject, timeAlive);
    }
    private void OnTriggerEnter2D(Collider2D info)
    {
        if (info.tag != "EnemyExplosion")
            Destroy(gameObject);
    }
}
