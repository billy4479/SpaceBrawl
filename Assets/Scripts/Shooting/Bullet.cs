using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float speed = 10f;
    [HideInInspector] public int damage = 10;
    [HideInInspector] public bool targetPlayer = false;

    private void FixedUpdate()
    {
        transform.position += transform.up * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "EnemyExplosion")
            Destroy(gameObject);
    }
}