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

        if(GUILayout.Button("Normal"))
        {
            Instantiate(assetsHolder.Enemy_1, gm.RandomSpawnPosOnBorders(), Quaternion.identity);
        }
        if (GUILayout.Button("Big"))
        {
            Instantiate(assetsHolder.Enemy_2, gm.RandomSpawnPosOnBorders(), Quaternion.identity);
        }
        if (GUILayout.Button("Shooter"))
        {
            Instantiate(assetsHolder.Enemy_3, gm.RandomSpawnPosOnBorders(), Quaternion.identity);
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
