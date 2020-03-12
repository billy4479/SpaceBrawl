using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class SpawnEnemyesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameManager gm = (GameManager)target;
        AssetsHolder assetsHolder = AssetsHolder.instance;
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (GUILayout.Button("Normal"))
        {
            GameObject obj = Instantiate(assetsHolder.Enemy_Base, gm.RandomSpawnPosOnBorders(), Quaternion.identity);
            obj.GetComponent<EnemyBase>().stats = assetsHolder.EnemyStats[0];
            obj.GetComponent<HealthSystem>().enemyStats = assetsHolder.EnemyStats[0];
        }
        if (GUILayout.Button("Big"))
        {
            GameObject obj = Instantiate(assetsHolder.Enemy_Base, gm.RandomSpawnPosOnBorders(), Quaternion.identity);
            obj.GetComponent<EnemyBase>().stats = assetsHolder.EnemyStats[1];
            obj.GetComponent<HealthSystem>().enemyStats = assetsHolder.EnemyStats[1];
        }
        if (GUILayout.Button("Shooter"))
        {
            GameObject obj = Instantiate(assetsHolder.Enemy_Base, gm.RandomSpawnPosOnBorders(), Quaternion.identity);
            obj.GetComponent<EnemyBase>().stats = assetsHolder.EnemyStats[2];
            obj.GetComponent<HealthSystem>().enemyStats = assetsHolder.EnemyStats[2];
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
