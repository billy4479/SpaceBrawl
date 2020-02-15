using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PlayerMoviment : MonoBehaviour
{
    public float speed = 300f;
    public Rigidbody2D PlayerRB;
    public GameManager gameManager;
    public Animator animator;
    public float maxSpeed;
    public float turnSpeed;

    private Joystick joystick;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
    }

    void Update()
    {
        if (!gameManager.SuspendInput)
        {
            Vector2 direction = new Vector2(joystick.Horizontal, joystick.Vertical);
            float angle = UtilsClass.GetAngleFromVectorFloat(direction) - 90f;
            if (direction == Vector2.zero)
                angle = PlayerRB.rotation;
            angle = Mathf.LerpAngle(PlayerRB.rotation, angle, turnSpeed);
            PlayerRB.rotation = angle;
            if (direction != Vector2.zero)
                PlayerRB.AddForce(transform.up * speed * Time.deltaTime);

            if (direction != Vector2.zero)
                animator.SetInteger("dir", 1);
            else
                animator.SetInteger("dir", 0);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
            gameManager.PlayerLose();
    }
}