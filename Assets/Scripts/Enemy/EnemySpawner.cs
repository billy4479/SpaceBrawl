using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameManager gm;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] anchors;
    public EnemyStats[] enemyStats;
    private enum Side { UP, DOWN, RIGHT, LEFT }


    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public GameObject SpawnSingleEnemy(Vector3 position, Quaternion rotation, int statsIndex, bool debug = false)
    {
        var stats = enemyStats[statsIndex];
        GameObject obj = Instantiate(enemyPrefab, position, rotation);
        obj.GetComponent<EnemyBase>().stats = stats;
        obj.GetComponent<EnemyBase>().debug = debug;
        obj.GetComponent<HealthSystem>().enemyStats = stats;
        foreach (CapsuleCollider2D collider in obj.GetComponents<CapsuleCollider2D>())
            collider.size *= stats.scale;
        gm.enemyNumber++;
        if (gm.EnemyRBs == null)
            gm.EnemyRBs = new System.Collections.Generic.List<Rigidbody2D>();
        gm.EnemyRBs.Add(obj.GetComponent<Rigidbody2D>());

        return obj;
    }
    public GameObject SpawnSingleEnemy(Quaternion rotation, int statsIndex, bool debug = false) {
        return SpawnSingleEnemy(RandomSpawnPosOnBorders(), rotation, statsIndex, debug);
    }

    public void SpawnNewLevelEnemy(int EnemiesToSpawn, bool debug = false)
    {
        gm.enemyNumber = 0;
        List<EnemyStats> enemyList = new List<EnemyStats>(enemyStats);
        List<EnemyStats> enemyToRemove = new List<EnemyStats>();
        foreach (var enemy in enemyList)
        {
            if (enemy.minLevel > gm.level)
                enemyToRemove.Add(enemy);
        }
        foreach (var enemy in enemyToRemove)
        {
            enemyList.Remove(enemy);
        }

        int totProb = 0;
        int arrLen = enemyList.Count;
        for (int j = 0; j < arrLen; j++)
            totProb += enemyList[j].probability;

        for (int i = 0; i < EnemiesToSpawn; i++)
        {
            int randNum = Random.Range(0, totProb);

            int finalIndex = -1;
            int accumulo = 0;

            for (int j = 0; j < arrLen; j++)
            {
                if (randNum < enemyList[j].probability + accumulo)
                {
                    finalIndex = j;
                    break;
                }
                else
                    accumulo += enemyList[j].probability;
            }
            if (finalIndex == -1)
            {
                throw new System.IndexOutOfRangeException("No Enemy Selection");
                //break;
            }
            Vector3 pos = RandomSpawnPosOnBorders();
            SpawnSingleEnemy(pos, Quaternion.identity, enemyList[finalIndex].ID, debug);
        }



    }
    private Vector3 RandomSpawnPosOnBorders()
    {
        Side side = (Side)UnityEngine.Random.Range(0, 4);

        switch (side)
        {
            case Side.UP:
                return new Vector3(UnityEngine.Random.Range(anchors[0].position.x, -anchors[0].position.x), anchors[0].position.y, 0);

            case Side.DOWN:
                return new Vector3(UnityEngine.Random.Range(anchors[1].position.x, -anchors[1].position.x), anchors[1].position.y, 0);

            case Side.RIGHT:
                return new Vector3(anchors[0].position.x, UnityEngine.Random.Range(anchors[0].position.y, -anchors[0].position.y), 0);

            case Side.LEFT:
                return new Vector3(anchors[1].position.x, UnityEngine.Random.Range(anchors[1].position.y, -anchors[1].position.y), 0);

            default:
                Debug.LogError("No random number in enemy spawning function");
                return Vector3.zero;
        }
    }
}