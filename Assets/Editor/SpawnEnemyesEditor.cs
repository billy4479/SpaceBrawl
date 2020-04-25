using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemySpawner))]
public class SpawnEnemyesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemySpawner enemySpawner = (EnemySpawner)target;
        GameManager gm = FindObjectOfType<GameManager>();
        var enemyList = enemySpawner.enemyStats;
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        try
        {
            for (int i = 0; i < enemyList.Length; i++)
            {
                if (GUILayout.Button(enemyList[i].name))
                {
                    enemySpawner.SpawnSingleEnemy(Quaternion.identity, enemyList[i].ID, gm.debug);
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
