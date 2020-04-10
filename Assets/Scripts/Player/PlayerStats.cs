using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Statistics")]
public class PlayerStats : ScriptableObject
{
    public float acceleration;
    public float maxSpeed;
    public int life;
    public float timeToStartRegen;
    public float regenSpeed;
    public BulletStats bulletStats;
    public float fireRate;
}
