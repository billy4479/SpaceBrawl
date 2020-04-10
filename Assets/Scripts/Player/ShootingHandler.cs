using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    private GameManager gm;
    private AssetsHolder assetsHolder;
    private Weapon playerWeapon;
    private Joystick fire;

    private void Start()
    {
        gm = GameManager.instance;
        assetsHolder = AssetsHolder.instance;
        fire = assetsHolder.Joystick_Fire;
        playerWeapon = assetsHolder.Player_Current.GetComponent<Weapon>();
    }

    private void Update()
    {
        if (fire.Direction != Vector2.zero && playerWeapon != null && !gm.suspendInput && !PauseMenu.isPaused)
            playerWeapon.Shoot();
    }
}