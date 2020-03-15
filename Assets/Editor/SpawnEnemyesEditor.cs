using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class SpawnEnemyesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemySpawner enemySpawner = (EnemySpawner)target;
        GameManager gm = GameManager.instance;
        AssetsHolder assetsHolder = AssetsHolder.instance;
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (GUILayout.Button("Normal"))
        {
            enemySpawner.SpawnEnemy(gm.RandomSpawnPosOnBorders(), Quaternion.identity, assetsHolder.Enemy_Stats[0]);
        }
        if (GUILayout.Button("Big"))
        {
            enemySpawner.SpawnEnemy(gm.RandomSpawnPosOnBorders(), Quaternion.identity, assetsHolder.Enemy_Stats[1]);
        }
        if (GUILayout.Button("Shooter"))
        {
            enemySpawner.SpawnEnemy(gm.RandomSpawnPosOnBorders(), Quaternion.identity, assetsHolder.Enemy_Stats[2]);
        }
        if (GUILayout.Button("Summoner"))
        {
            enemySpawner.SpawnEnemy(gm.RandomSpawnPosOnBorders(), Quaternion.identity, assetsHolder.Enemy_Stats[3]);
        }
        if (GUILayout.Button("Spawned"))
        {
            enemySpawner.SpawnEnemy(gm.RandomSpawnPosOnBorders(), Quaternion.identity, assetsHolder.Enemy_Stats[4]);
        }
        if (GUILayout.Button("Clear Enemies"))
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                enemy.GetComponent<EnemyBase>().RemoveEnemy();
            }
        }
    }
}
