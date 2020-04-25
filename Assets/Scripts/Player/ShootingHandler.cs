using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    private GameManager gm;
    private Weapon playerWeapon;
    [SerializeField]private Joystick fire;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        playerWeapon = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapon>();
    }

    private void Update()
    {
        if (fire.Direction != Vector2.zero && playerWeapon != null && !gm.suspendInput && !PauseMenu.isPaused)
            playerWeapon.Shoot();
    }
}