using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyStats))]
public class EnemyStatsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var script = target as EnemyStats;

        script.ID = EditorGUILayout.IntField(nameof(script.ID), script.ID);
        script.enemyType = (EnemyType)EditorGUILayout.EnumPopup(nameof(script.enemyType), script.enemyType);
        script.scale = EditorGUILayout.FloatField(nameof(script.scale), script.scale);
        script.speed = EditorGUILayout.FloatField(nameof(script.speed), script.speed);
        script.bodyDamage = EditorGUILayout.IntField(nameof(script.bodyDamage), script.bodyDamage);
        script.pointAtDeath = EditorGUILayout.IntField(nameof(script.pointAtDeath), script.pointAtDeath);
        script.rotationSpeed = EditorGUILayout.FloatField(nameof(script.rotationSpeed), script.rotationSpeed);
        script.probability = EditorGUILayout.IntField(nameof(script.probability), script.probability);
        script.minLevel = EditorGUILayout.IntField(nameof(script.minLevel), script.minLevel);
        EditorGUILayout.Space();
        script.arrowColor = EditorGUILayout.ColorField(nameof(script.arrowColor), script.arrowColor);
        script.aoc = EditorGUILayout.ObjectField(nameof(script.aoc), script.aoc, typeof(AnimatorOverrideController), true) as AnimatorOverrideController;
        EditorGUILayout.Space();
        script.life = EditorGUILayout.IntField(nameof(script.life), script.life);
        script.regenSpeed = EditorGUILayout.FloatField(nameof(script.regenSpeed), script.regenSpeed);
        script.timeToStartRegen = EditorGUILayout.FloatField(nameof(script.timeToStartRegen), script.timeToStartRegen);


        switch (script.enemyType)
        {
            case EnemyType.Normal:
                break;
            case EnemyType.Shooter:
                EditorGUILayout.Space();
                script.fireDistance = EditorGUILayout.FloatField(nameof(script.fireDistance), script.fireDistance);
                script.starfeSpeed = EditorGUILayout.FloatField(nameof(script.starfeSpeed), script.starfeSpeed);
                script.fireRate = EditorGUILayout.FloatField(nameof(script.fireRate), script.fireRate);
                script.bulletStats = EditorGUILayout.ObjectField(nameof(script.bulletStats), script.bulletStats, typeof(BulletStats), true) as BulletStats;
                break;
            case EnemyType.Summoner:
                EditorGUILayout.Space();
                script.spawnRate = EditorGUILayout.FloatField(nameof(script.spawnRate), script.spawnRate);
                script.enemyToSpawnNumber = EditorGUILayout.IntField(nameof(script.enemyToSpawnNumber), script.enemyToSpawnNumber);
                script.enemyToSpawnID = EditorGUILayout.IntField(nameof(script.enemyToSpawnID), script.enemyToSpawnID);
                break;
            case EnemyType.Charger:
                EditorGUILayout.Space();
                script.starfeSpeed = EditorGUILayout.FloatField(nameof(script.starfeSpeed), script.starfeSpeed);
                script.timeToCharge = EditorGUILayout.FloatField(nameof(script.timeToCharge), script.timeToCharge);
                script.chargeDistance = EditorGUILayout.FloatField(nameof(script.chargeDistance), script.chargeDistance);
                script.distanceOffset = EditorGUILayout.FloatField(nameof(script.distanceOffset), script.distanceOffset);
                script.speedCoeff = EditorGUILayout.FloatField(nameof(script.speedCoeff), script.speedCoeff);
                break;
            default:
                break;
        }

    }
}
