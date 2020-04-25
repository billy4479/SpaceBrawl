using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IHealth
{
    #region Variables

    [HideInInspector]
    public EnemyStats stats;

    private bool canMove = true;
    private float repelRange = 1.5f;
    private float repelStrenght = 1f;

    //Components and Managers
    private Rigidbody2D playerRB;
    private GameManager gm;
    private AudioManager am;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject pointer;
    private Transform firePos;
    private Transform[] anchors;

    private float speed;
    private float starfeSpeed;

    //Shooter
    private Weapon weapon;

    //Spawner
    private float lastSpawn = float.MinValue;
    private EnemySpawner enemySpawner;

    //Charger
    private float chargerTimer = float.MinValue;
    private bool isCharging = false;
    private float directionAtDashStart;
    private bool isDashing = false;
    private float initialSpeed;

    #endregion Variables

    //[HideInInspector]
    public bool debug = false;

    private void Awake()
    {
        am = AudioManager.instance;
        gm = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        pointer = transform.Find("Pointer").gameObject;
        firePos = transform.Find("FirePosition");
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        weapon = GetComponent<Weapon>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        anchors = new Transform[2];
        var tmp = GameObject.FindGameObjectsWithTag("Anchor");
        for (int i = 0; i < tmp.Length; i++) anchors[i] = tmp[i].transform;
    }

    private void Start()
    {
        transform.localScale *= stats.scale;
        firePos.transform.localPosition *= stats.scale;
        weapon.SetBulletStats(stats.bulletStats, stats.fireRate);
        initialSpeed = stats.speed;
        pointer.GetComponent<SpriteRenderer>().color = stats.arrowColor;
        animator.runtimeAnimatorController = stats.aoc;
        speed = stats.speed;
        starfeSpeed = stats.starfeSpeed;
        if (debug)
        {
            starfeSpeed = 0f;
            speed = 0f;
            initialSpeed = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            #region Rotation
            Vector2 direction = (playerRB.position - rb.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, stats.rotationSpeed);
            #endregion
            float distance = Vector2.Distance(rb.position, playerRB.position);

            switch (stats.enemyType)
            {
                case EnemyType.Normal:
                    rb.MovePosition(MoveNormally());
                    break;
                case EnemyType.Shooter:
                    if (distance >= stats.fireDistance)
                        rb.MovePosition(MoveNormally());
                    else
                    {
                        rb.MovePosition(MoveStarfing());
                        weapon.Shoot();
                    }
                    break;
                case EnemyType.Summoner:
                    rb.MovePosition(MoveNormally());
                    if (Time.time - lastSpawn > 1f / stats.spawnRate && !debug)
                    {
                        lastSpawn = Time.time;
                        Spawn();
                    }
                    break;
                case EnemyType.Charger:
                    if (distance >= stats.chargeDistance) //Se è troppo lontano
                    {
                        if (distance >= stats.chargeDistance + stats.distanceOffset) //E NON è nella safeZone
                        { //Interrompi
                            StopDashing();
                            rb.MovePosition(MoveNormally());
                        }
                        else if (isCharging || isDashing) MoveChargerCharge(); //Carica lo stesso
                        else rb.MovePosition(MoveNormally());
                    }
                    else MoveChargerCharge(); //Se è abbastanza vicino carica e basta
                    break;
                default:
                    break;
            }
        }
    }

    #region Charger
    private void StartCharge()
    {
        if (!isCharging)
        {
            chargerTimer = Time.time;
            isCharging = true;
        }
        else if (Time.time - chargerTimer > stats.fireRate && !debug)
        {
            //Dash
            speed = initialSpeed * stats.speedCoeff;
            isCharging = false;
            directionAtDashStart = rb.rotation;
            isDashing = true;
        }
    }
    private void MoveChargerCharge()
    {
        if (!isDashing)
        {
            rb.MovePosition(MoveStarfing());
            StartCharge();
        }
        else
        {
            rb.rotation = directionAtDashStart;
            rb.MovePosition(MoveNormally());
        }
    }
    private void StopDashing()
    {
        chargerTimer = float.MinValue;
        speed = initialSpeed;
        isDashing = false;
        isCharging = false;
    }
    #endregion
    #region Spawner
    private void Spawn()
    {
        for (int i = 0; i < stats.enemyToSpawnNumber; i++)
        {
            Vector3 randomOffset = GetRandomOffset();
            Vector3 position = transform.position + randomOffset.normalized * 2f;
            enemySpawner.SpawnSingleEnemy(position, Quaternion.identity, stats.enemyToSpawnID);
        }
    }
    private Vector3 GetRandomOffset()
    {
        Vector3 randomOffset = new Vector3(Random.Range(1f, 5f), Random.Range(1f, 5f));
        if (Random.Range(0, 2) == 1)
            randomOffset.x = randomOffset.x * -1f;
        if (Random.Range(0, 2) == 1)
            randomOffset.y = randomOffset.y * -1f;

        float max = Mathf.Abs(anchors[0].position.x);
        if (randomOffset.x > max - 1f || randomOffset.x < -max + 1f || randomOffset.y > max - 1f || randomOffset.y < -max + 1f)
        {
            randomOffset = GetRandomOffset();
        }

        return randomOffset;
    }
    #endregion

    #region Moviment

    private Vector2 MoveStarfing()
    {
        return transform.position + transform.right * Time.fixedDeltaTime * starfeSpeed;
    }

    private Vector2 MoveNormally()
    {
        Vector2 repelForce = Vector2.zero;
        foreach (Rigidbody2D enemy in gm.EnemyRBs)
        {
            if (enemy == rb)
                continue;
            if (Vector2.Distance(enemy.position, rb.position) <= repelRange)
                repelForce += (rb.position - enemy.position).normalized; //<=== direction
        }

        Vector2 newPos = transform.position + transform.up * Time.fixedDeltaTime * speed;
        newPos += repelForce * Time.fixedDeltaTime * repelStrenght;
        return newPos;
    }

    #endregion Moviment
  
    #region Heath
    public void RemoveEnemy()
    {
        Destroy(pointer);
        gm.EnemyRBs.Remove(rb);
        gm.enemyNumber--;
        Destroy(gameObject); 
    }
    private IEnumerator AnimateDeath()
    {
        animator.SetBool("Death", true);

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    public void OnDeath(bool defeated)
    {
        tag = "EnemyExplosion";

        gm.EnemyRBs.Remove(rb);
        canMove = false;
        Destroy(rb);
        Destroy(pointer);
        CapsuleCollider2D[] capsuleColl = GetComponents<CapsuleCollider2D>();
        for (int i = 0; i < capsuleColl.Length; i++)
            Destroy(capsuleColl[i]);
        /*PolygonCollider2D[] polColl = GetComponents<PolygonCollider2D>();
        for (int i = 0; i < polColl.Length; i++)
            Destroy(polColl[i]);*/

        if (defeated)
            gm.score += stats.pointAtDeath;
        gm.enemyNumber--;
        am.PlaySound("EnemyExplosion");
        StartCoroutine(AnimateDeath());
    }
    public bool IsPlayer() => false;

    public object GetStats() => stats;
    #endregion

}