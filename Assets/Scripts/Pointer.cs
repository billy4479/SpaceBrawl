using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private GameManager gm;
    private Rigidbody2D enemy;
    private Rigidbody2D player;

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemy = transform.GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.parent = null;
    }

    private void FixedUpdate()
    {
        try
        {
            player = gm.CurrentPlayer.GetComponent<Rigidbody2D>();
        }
        catch
        {
            Destroy(gameObject);
        }
        Vector2 direction = (enemy.position - player.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);

        //direction.Scale(gm.screenSize * 0.9f);
        Vector2 position = player.position + direction * gm.screenSize.y * .9f;
        transform.position = position;
    }

}
