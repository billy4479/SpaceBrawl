using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    public GameManager gm;

    public Weapon playerWeapon;
    private bool hold = false;

    private void Update()
    {
        if (hold)
            Shoot();
    }
    
    private void Shoot()
    {
        if (playerWeapon != null && !gm.SuspendInput && !Pause.isPaused)
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