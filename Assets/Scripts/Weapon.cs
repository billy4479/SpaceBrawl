using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform FirePoint;
    public GameManager gameManager;
    private Rigidbody2D rb;
    private float strengh = 25f;
    private float counter = -1f;
    private float reloadTime = .25f;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && !gameManager.SuspendInput && counter + reloadTime <=  Time.time)
        {
            counter = Time.time;
            Instantiate(bullet, FirePoint.position, FirePoint.rotation);
            rb.AddForce(transform.up * -strengh);
        }
    }
}
