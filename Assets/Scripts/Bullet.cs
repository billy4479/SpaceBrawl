using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 10f;
    public Rigidbody2D rb;
    void Start()
    {
        rb.velocity = transform.up * speed;
    }
    private void OnTriggerEnter2D(Collider2D info)
    {
        if (info.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        if (info.tag == "Border")
        {
            Destroy(gameObject);
        }

    }
}
