using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float timeAlive = 2f;
    private GameManager gm;

    private void Start()
    {
        Destroy(gameObject, timeAlive);
        gm = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        transform.position += transform.up * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D info)
    {
        if (info.tag != "EnemyExplosion")
            Destroy(gameObject);
        if (info.tag == "Player")
        {
            foreach (GameObject enemies in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                gm.EnemyRBs.Clear();
                Destroy(enemies);
            }
            foreach (GameObject pointer in GameObject.FindGameObjectsWithTag("Pointer"))
            {
                Destroy(pointer);
            }
        }
    }
}