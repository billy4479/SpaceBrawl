using CodeMonkey.Utils;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour, IHealth
{
    [SerializeField] float speed = 300f;
    [SerializeField] float maxSpeed;
    private Rigidbody2D PlayerRB;
    private Animator animator;
    private Weapon weapon;

    private AssetsHolder assetsHolder;
    private GameManager gameManager;
    private Joystick joystick;
    private SaveManager sm;

    private void Start()
    {
        assetsHolder = AssetsHolder.instance;
        gameManager = GameManager.instance;
        joystick = assetsHolder.joystick;
        sm = SaveManager.instance;
        PlayerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
    }

    public void OnDeath(bool defeated)
    {
        gameManager.PlayerLose();
    }
    public bool IsPlayer() => true;

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
                {
                    PlayerRB.angularVelocity = 0f;
                    animator.SetInteger("dir", 0);
                }
            }
            else
            {
                Vector2 direction = new Vector2(joystick.Horizontal, joystick.Vertical);
                if (direction == Vector2.zero)
                {
                    animator.SetInteger("dir", 0);
                    PlayerRB.angularVelocity = 0f;
                }
                else
                {
                    float angle = UtilsClass.GetAngleFromVectorFloat(direction) - 90f;
                    PlayerRB.rotation = Mathf.LerpAngle(PlayerRB.rotation, angle, direction.magnitude);
                    PlayerRB.AddForce(transform.up * speed * Mathf.Pow(direction.magnitude, 2f) * Time.deltaTime);

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