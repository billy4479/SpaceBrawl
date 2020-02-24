using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float timeAlive = 2f;
    public int damage = 10;

    private void Start()
    {
        Destroy(gameObject, timeAlive);
    }

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