using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Statistics")]
public class EnemyStats : ScriptableObject
{
    public EnemyType enemyType;
    public int life = 10;
    public float scale = 1f;
    public float regenSpeed = 2f;
    public float timeToStartRegen = 5f;
    public float speed;
    public int damage = 10;
    public int pointAtDeath = 1;
    public float rotationSpeed = .1f;
    public int probability = 10;
    public int minLevel;
    [Space]
    public float fireDistance = 5f;
    public float starfeSpeed = 1f;
    public float fireRate = 1f;
    [Space]
    public float spawnRate;
    public int enemyToSpawnNumber;
    public int enemyToSpawnIndex;
    [Space]
    public Color arrowColor = Color.white;
    public AnimatorOverrideController aoc;

}

public enum EnemyType {Normal, Shooter, Summoner, Charger}
