using UnityEngine;

[CreateAssetMenu(menuName = "Bullet Statistics")]
public class BulletStats : ScriptableObject
{
    public bool player;
    public int damage = 10;
    public float speed = 10f;
    public float timaAlive = 2f;
    public float size = 1f;
    public AnimatorOverrideController aoc;
    public string soundName;
}
