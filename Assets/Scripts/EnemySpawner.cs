using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    private void Awake()
    {
        instance = this;
    }

    private AssetsHolder assetsHolder;
    private GameManager gm;

    public GameObject SpawnEnemy(Vector3 position, Quaternion rotation, EnemyStats stats)
    {
        if(assetsHolder == null) assetsHolder = AssetsHolder.instance;
        if (gm == null) gm = GameManager.instance;
        GameObject obj = Instantiate(assetsHolder.Enemy_Base, position, rotation);
        obj.GetComponent<EnemyBase>().stats = stats;
        obj.GetComponent<HealthSystem>().enemyStats = stats;
        foreach (CapsuleCollider2D collider in obj.GetComponents<CapsuleCollider2D>())
            collider.size *= stats.scale;
        gm.enemyNumber++;
        if (gm.EnemyRBs == null)
            gm.EnemyRBs = new System.Collections.Generic.List<Rigidbody2D>();
        gm.EnemyRBs.Add(obj.GetComponent<Rigidbody2D>());

        return obj;
    }
}