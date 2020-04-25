using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Statistics")]
public class EnemyStats : ScriptableObject
{
    //Base
    public int ID;
    public EnemyType enemyType;
    public float scale;
    public float speed;
    public int bodyDamage;
    public int pointAtDeath;
    public float rotationSpeed;
    public int probability;
    public int minLevel;
    
    public Color arrowColor = Color.white;
    public AnimatorOverrideController aoc;
    //Health
    public int life;
    public float regenSpeed;
    public float timeToStartRegen;
    //Shooter
    public float fireDistance;
    public float starfeSpeed;
    public float fireRate;
    public BulletStats bulletStats;
    //Spawner
    public float spawnRate;
    public int enemyToSpawnNumber;
    public int enemyToSpawnID;
    //Charger
    public float timeToCharge;
    public float chargeDistance;
    public float distanceOffset;
    public float speedCoeff;

}

public enum EnemyType {Normal, Shooter, Summoner, Charger}
