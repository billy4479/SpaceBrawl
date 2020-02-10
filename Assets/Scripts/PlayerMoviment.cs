using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    float speed = 300f;
    public Rigidbody2D PlayerRB;
    public GameManager gameManager;
    public Animator animator;
    public float maxSpeed;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!gameManager.SuspendInput)
        {
            #region moviment
            PlayerRB.rotation = RotateToMouse(Input.mousePosition, transform.position);
/*
            if (Input.GetKey(KeyCode.W))
                PlayerRB.AddForce(transform.up * speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.S))
                PlayerRB.AddForce(transform.up * -1 * speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.A))
                PlayerRB.AddForce(transform.right * -1 * speed * Time.deltaTime);

            if (Input.GetKey(KeyCode.D))
                PlayerRB.AddForce(transform.right * speed * Time.deltaTime);
*/
            if (Input.GetMouseButton(0))
            {
                PlayerRB.AddForce(transform.up * speed * Time.deltaTime);
            }

            #endregion

            #region animation
            if (Input.GetKey(KeyCode.W))
                animator.SetInteger("dir", 1);
            else if (Input.GetKey(KeyCode.A))
                animator.SetInteger("dir", 2);
            else if (Input.GetKey(KeyCode.D))
                animator.SetInteger("dir", 3);
            else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
                animator.SetInteger("dir", 4);
            else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
                animator.SetInteger("dir", 5);
            else
                animator.SetInteger("dir", 0);
            #endregion
        }
        else
            animator.SetInteger("dir", 0);

        #region setMaxSpeed
        if (PlayerRB.velocity.x > maxSpeed)
            PlayerRB.velocity = new Vector2(maxSpeed, PlayerRB.velocity.y);
        if (PlayerRB.velocity.y > maxSpeed)
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, maxSpeed);
        if (PlayerRB.velocity.x < -maxSpeed)
            PlayerRB.velocity = new Vector2(-maxSpeed, PlayerRB.velocity.y);
        if (PlayerRB.velocity.y < -maxSpeed)
            PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, -maxSpeed);
        #endregion

    }
    private Vector3 pixelPos(Vector3 posR)
    {
        return Camera.main.WorldToScreenPoint(posR);
    }

    private float RotateToMouse(Vector3 MousePos, Vector3 PlayerPos)
    {
        float rotation = Mathf.Atan2(MousePos.x - pixelPos(PlayerPos).x, MousePos.y - pixelPos(PlayerPos).y) * Mathf.Rad2Deg * -1;

        return rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
            gameManager.PlayerLose();
    }
}