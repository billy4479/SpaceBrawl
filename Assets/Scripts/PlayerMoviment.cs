using CodeMonkey.Utils;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    public float speed = 300f;
    public Rigidbody2D PlayerRB;
    public Animator animator;
    public float maxSpeed;
    public Weapon weapon;

    private GameManager gameManager;
    private Joystick joystick;
    private SaveManager sm;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (GameObject.FindGameObjectWithTag("Joystick") != null)
            joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
        sm = SaveManager.instance;
    }

    private void Update()
    {
        if (!gameManager.SuspendInput)
        {
            if (sm.settings.controllMethod == SaveManager.ControllMethod.Finger)
            {
                if (Input.GetMouseButton(0))
                {
                    PlayerRB.rotation = UtilsClass.GetAngleFromVector((Vector2)UtilsClass.GetMouseWorldPosition() - PlayerRB.position) - 90f;
                    PlayerRB.AddForce(transform.up * speed * Time.deltaTime);
                    weapon.Shoot();

                    animator.SetInteger("dir", 1);
                }
                else
                    animator.SetInteger("dir", 0);
            }
            else
            {
                Vector2 direction = new Vector2(joystick.Horizontal, joystick.Vertical);
                if (direction == Vector2.zero)
                {
                    animator.SetInteger("dir", 0);
                }
                else
                {
                    float angle = UtilsClass.GetAngleFromVectorFloat(direction) - 90f;
                    PlayerRB.rotation = Mathf.LerpAngle(PlayerRB.rotation, angle, direction.magnitude);
                    PlayerRB.AddForce(transform.up * speed * direction.magnitude * Time.deltaTime);

                    animator.SetInteger("dir", 1);
                }
            }

            #region setMaxSpeed

            if (PlayerRB.velocity.x > maxSpeed)
                PlayerRB.velocity = new Vector2(maxSpeed, PlayerRB.velocity.y);
            if (PlayerRB.velocity.y > maxSpeed)
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, maxSpeed);
            if (PlayerRB.velocity.x < -maxSpeed)
                PlayerRB.velocity = new Vector2(-maxSpeed, PlayerRB.velocity.y);
            if (PlayerRB.velocity.y < -maxSpeed)
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, -maxSpeed);

            #endregion setMaxSpeed
        }
        else
            animator.SetInteger("dir", 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
            gameManager.PlayerLose();
    }
}