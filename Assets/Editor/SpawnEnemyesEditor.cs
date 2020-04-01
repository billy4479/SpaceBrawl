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

        try
        {
            for (int i = 0; i < assetsHolder.Enemy_Stats.Length; i++)
            {
                if (GUILayout.Button(i.ToString()))
                {
                    enemySpawner.SpawnEnemy(gm.RandomSpawnPosOnBorders(), Quaternion.identity, assetsHolder.Enemy_Stats[i]);
                }
            }

            if (GUILayout.Button("Clear Enemies"))
            {
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<EnemyBase>().RemoveEnemy();
                }
            }
        }
        catch { }
    }
}
