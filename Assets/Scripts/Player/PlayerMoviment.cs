using CodeMonkey.Utils;
using UnityEngine;

public class PlayerMoviment : MonoBehaviour, IHealth
{

    private PlayerStats stats;
    [SerializeField] PlayerStats[] playerStats;

    private Rigidbody2D PlayerRB;
    private Animator animator;
    private Weapon weapon;

    private GameManager gameManager;
    private SaveManager sm;
    
    [SerializeField] private Joystick positionJoystick;
    [SerializeField] private Joystick fireJoystick;

    private float lastAngle;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        PlayerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        sm = SaveManager.instance;
        if (sm == null) Debug.Log("SaveManager null at Awake");
        stats = playerStats[sm.settings.selectedCharacter];
    }

    private void Start()
    {
        transform.localScale *= stats.scale;
        weapon.SetBulletStats(stats.bulletStats, stats.fireRate);
    }

    public object GetStats()
    {
        if (stats == null) throw new System.NullReferenceException();
        return stats;
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
                    PlayerRB.rotation = UtilsClass.GetAngleFromVectorFloat((Vector2)UtilsClass.GetMouseWorldPosition() - PlayerRB.position) - 90f;
                    PlayerRB.AddForce(transform.up * stats.acceleration);
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
                PlayerRB.rotation = UtilsClass.GetAngleFromVectorFloat((Vector2)UtilsClass.GetMouseWorldPosition() - PlayerRB.position) - 90f;
                if (Input.GetMouseButton(0)) weapon.Shoot();

                Vector2 direction = new Vector2();
                if (Input.GetKey(KeyCode.W)) direction.y++;
                if (Input.GetKey(KeyCode.A)) direction.x--;
                if (Input.GetKey(KeyCode.S)) direction.y--;
                if (Input.GetKey(KeyCode.D)) direction.x++;
                PlayerRB.AddForce(direction * stats.acceleration);
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
                    PlayerRB.AddForce(positionJoystick.Direction * stats.acceleration * Mathf.Pow(positionJoystick.Direction.magnitude, 3f));
                    animator.SetInteger("dir", 1);
                }
            }

            #region setMaxSpeed

            if (PlayerRB.velocity.x > stats.maxSpeed)
                PlayerRB.velocity = new Vector2(stats.maxSpeed, PlayerRB.velocity.y);
            if (PlayerRB.velocity.y > stats.maxSpeed)
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, stats.maxSpeed);
            if (PlayerRB.velocity.x < -stats.maxSpeed)
                PlayerRB.velocity = new Vector2(-stats.maxSpeed, PlayerRB.velocity.y);
            if (PlayerRB.velocity.y < -stats.maxSpeed)
                PlayerRB.velocity = new Vector2(PlayerRB.velocity.x, -stats.maxSpeed);

            #endregion setMaxSpeed
        }
        else
            animator.SetInteger("dir", 0);
    }
    private void OnDrawGizmosSelected()
    {
        try
        {
            Gizmos.DrawLine(PlayerRB.position, UtilsClass.GetMouseWorldPosition());
        }
        catch { }
    }
}