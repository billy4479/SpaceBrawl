using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingHandler : MonoBehaviour
{
    public GameManager gm;

    private Weapon weapon;
    private bool hold = false;

    public void SetNewPlayer(GameObject player)
    {
        weapon = player.GetComponent<Weapon>();
    }
    public void RemovePlayer()
    {
        weapon = null;
    }

    private void Shoot()
    {
        if (weapon != null && !gm.SuspendInput && !Pause.isPaused)
            weapon.Shoot();
    }

    private void Update()
    {
        if (hold)
            Shoot();
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
