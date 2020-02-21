using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class SpawnEnemyesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameManager gm = (GameManager)target;
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if(GUILayout.Button("Normal"))
        {
            Instantiate(gm.Enemies[0], gm.RandomSpawnPosOnBorders(), Quaternion.identity);
        }
        if (GUILayout.Button("Big"))
        {
            Instantiate(gm.Enemies[1], gm.RandomSpawnPosOnBorders(), Quaternion.identity);
        }
        if (GUILayout.Button("Shooter"))
        {
            Instantiate(gm.Enemies[2], gm.RandomSpawnPosOnBorders(), Quaternion.identity);
        }
        if (GUILayout.Button("Clear Enemies"))
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy.GetComponent<EnemyBase>().pointer);
                Destroy(enemy);
                gm.EnemyRBs.Clear();
            }
        }
    }
}
