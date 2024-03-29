﻿using UnityEngine;

public class Weapon : MonoBehaviour
{
    private BulletStats stats;
    public BulletStats SetBulletStats(BulletStats stats, float fireRate)
    {
        this.stats = stats;
        try
        { shotSoundName = stats.soundName; }
        catch (System.NullReferenceException) { }
        this.fireRate = fireRate;
        return stats;
    }
    [SerializeField] private GameObject bulletBase;
    public Transform FirePoint;
    private AudioManager audioManager;
    private float lastShoot;
    private float fireRate;
    private string shotSoundName;
    private bool reassign = false;

    private void Awake()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null) reassign = true;
    }

    private void Start()
    {
        if (reassign) audioManager = AudioManager.instance;
    }

    public void Shoot()
    {
        if (Time.time - lastShoot > 1f / fireRate)
        {
            lastShoot = Time.time;
            var obj = Instantiate(bulletBase, FirePoint.position, FirePoint.rotation);
            var controller = obj.GetComponent<Bullet>();
            controller.damage = stats.damage;
            controller.speed = stats.speed;
            controller.targetPlayer = !stats.player;
            obj.GetComponent<Animator>().runtimeAnimatorController = stats.aoc;
            obj.transform.localScale *= stats.size;

            audioManager.PlaySound(shotSoundName);
            Destroy(obj, stats.timaAlive);
        }
    }
}