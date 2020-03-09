using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    private GameManager gm;
    private Weapon playerWeapon;
    private bool hold = false;

    private void Start()
    {
        gm = GameManager.instance;
        playerWeapon = AssetsHolder.instance.Player.GetComponent<Weapon>();
    }

    private void Update()
    {
        if (hold)
            Shoot();
    }
    
    private void Shoot()
    {
        if (playerWeapon != null && !gm.SuspendInput && !PauseMenu.isPaused)
            playerWeapon.Shoot();
    }
    public void StartShooting()
    {
        hold = true;
    }
    public void StopShooting()
    {
        hold = false;
    }
}