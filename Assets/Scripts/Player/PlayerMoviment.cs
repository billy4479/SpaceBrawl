using CodeMonkey.Utils;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour, IHealth
{
    [HideInInspector]
    public PlayerStats stats;
    private float speed;
    private float maxSpeed;
    private Rigidbody2D PlayerRB;
    private Animator animator;
    private Weapon weapon;

    private AssetsHolder assetsHolder;
    private GameManager gameManager;
    private Joystick positionJoystick;
    private Joystick fireJoystick;
    private SaveManager sm;

    private float lastAngle;

    private void Start()
    {
        assetsHolder = AssetsHolder.instance;
        gameManager = GameManager.instance;
        positionJoystick = assetsHolder.Joystick_Position;
        fireJoystick = assetsHolder.Joystick_Fire;
        sm = SaveManager.instance;
        PlayerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        weapon.SetBulletStats(stats.bulletStats, stats.fireRate);
        speed = stats.acceleration;
        maxSpeed = stats.maxSpeed;
    }

    public void OnDeath(bool defeated)
    {
        gameManager.PlayerLose();
    }

    public bool IsPlayer() => true;

    private void Update()
    {
        if (!gameManager.suspendInput)
        {
            if (sm.settings.controllMethod == SaveManager.ControllMethod.Finger)
            {
                if (Input.GetMouseButton(0))
                {
                    PlayerRB.rotation = UtilsClass.GetAngleFromVector((Vector2)UtilsClass.GetMouseWorldPosition() - PlayerRB.position) - 90f;
                    PlayerRB.AddForce(transform.up * speed);
                    weapon.Shoot();

                    animator.SetInteger("dir", 1);
                }
                else
                {
                    PlayerRB.angularVelocity = 0f;
                    animator.SetInteger("dir", 0);
                }
            }
            else if (sm.settings.controllMethod == SaveManager.ControllMethod.KeyBoard)
            {
                PlayerRB.rotation = UtilsClass.GetAngleFromVector((Vector2)UtilsClass.GetMouseWorldPosition() - PlayerRB.position) - 90f;
                if (Input.GetMouseButton(0)) weapon.Shoot();

                Vector2 direction = new Vector2();
                if (Input.GetKey(KeyCode.W)) direction.y++;
                if (Input.GetKey(KeyCode.A)) direction.x--;
                if (Input.GetKey(KeyCode.S)) direction.y--;
                if (Input.GetKey(KeyCode.D)) direction.x++;
                PlayerRB.AddForce(direction * speed);
                if (direction != Vector2.zero) animator.SetInteger("dir", 1);
                else animator.SetInteger("dir", 0);
            }
            else
            {
                if (fireJoystick.Direction != Vector2.zero)
                {
                    float angle = UtilsClass.GetAngleFromVectorFloat(fireJoystick.Direction) - 90f;
                    PlayerRB.rotation = Mathf.LerpAngle(PlayerRB.rotation, angle, fireJoystick.Direction.magnitude);
                    lastAngle = PlayerRB.rotation;
                }
                else
                {
                    PlayerRB.rotation = lastAngle;
                }
                if (positionJoystick.Direction == Vector2.zero)
                {
                    animator.SetInteger("dir", 0);
                    PlayerRB.angularVelocity = 0f;
                }
                else
                {
                    PlayerRB.AddForce(positionJoystick.Direction * speed * Mathf.Pow(positionJoystick.Direction.magnitude, 3f));
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
}