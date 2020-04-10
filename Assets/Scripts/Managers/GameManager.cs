using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    private AssetsHolder assetsHolder = AssetsHolder.instance;

    private GameObject player;
    private Animator playerAnimator;
    private Transform[] anchors;
    private TextMeshProUGUI ScoreLabel;
    private TextMeshProUGUI LevelLabel;
    private EnemyStats[] EnemyTypes;
    private EnemySpawner enemySpawner;

    public List<Rigidbody2D> EnemyRBs;
    public ShootingHandler shootingHandler;

    public bool suspendInput = false;
    public int score = 0;
    public int level = 1;
    public int enemyNumber = 0;

    private AudioManager audioManager;

    private enum Side { UP, DOWN, RIGHT, LEFT }

    [HideInInspector]
    public Vector2 screenSize { get; private set; }

    #endregion Variables

    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion Singleton

    private void Start()
    {
        assetsHolder = AssetsHolder.instance;
        player = assetsHolder.Player_Current;
        playerAnimator = player.GetComponent<Animator>();
        anchors = assetsHolder.Anchors;
        ScoreLabel = assetsHolder.Label_Score;
        LevelLabel = assetsHolder.Label_Level;
        EnemyTypes = assetsHolder.Enemy_Stats;
        enemySpawner = EnemySpawner.instance;
        player.GetComponent<PlayerMoviment>().stats = assetsHolder.Player_Stats[assetsHolder.Player_Stats_Index];
        player.GetComponent<HealthSystem>().playerStats = assetsHolder.Player_Stats[assetsHolder.Player_Stats_Index];

        score = 0;
        level = 1;
        enemyNumber = 0;
        suspendInput = false;

        audioManager = AudioManager.instance;

        screenSize = new Vector2(
            Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f,
            Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f
        );
        SpawnNewEnemies(level);
    }

    private IEnumerator PlayerWon()
    {
        enemyNumber = -1;
        level++;

        yield return new WaitForSeconds(2);

        SpawnNewEnemies(level);
    }

    public void PlayerLose()
    {
        suspendInput = true;
        foreach (GameObject pointer in GameObject.FindGameObjectsWithTag("Pointer"))
            Destroy(pointer);
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
            Destroy(bullet);
        foreach (EnemyBase enemy in GameObject.FindObjectsOfType<EnemyBase>())
            enemy.RemoveEnemy();

        Rigidbody2D PlayerRB = player.GetComponent<Rigidbody2D>();
        PlayerRB.velocity = Vector2.zero;
        PlayerRB.angularVelocity = 0;

        audioManager.PlaySound("PlayerExplosion");
        playerAnimator.SetBool("Death", true);

        if (score > SaveManager.instance.scores[9].score)
        {
            SaveManager.SavedScores[] scr = SaveManager.instance.scores;

            int position = GetScoreboardPosition(score, scr);

            for (int i = 9; i >= position; i--)
            {
                if (i == 9)
                    continue;
                scr[i + 1].score = scr[i].score;
                scr[i + 1].date = scr[i].date;
                scr[i + 1].name = scr[i].name;
            }
            scr[position].score = score;
            scr[position].name = SaveManager.instance.settings.name;
            scr[position].date = System.DateTime.Now.ToString("d", System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR"));

            SaveManager.instance.WriteChanges();
        }

        StartCoroutine(WaitForPlayerDeathAnimation());
    }

    private IEnumerator WaitForPlayerDeathAnimation()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Menu");
    }

    private void FixedUpdate()
    {
        #region GUI

        ScoreLabel.text = "Score: " + score;
        LevelLabel.text = "Level: " + level;

        #endregion GUI

        if (enemyNumber == 0)
            StartCoroutine(PlayerWon());
        //Debug.Log(screenSize);
    }

    private void SpawnNewEnemies(int EnemiesToSpawn)
    {
        enemyNumber = 0;
        List<EnemyStats> enemyList = new List<EnemyStats>(EnemyTypes);
        List<EnemyStats> enemyToRemove = new List<EnemyStats>();
        foreach (var enemy in enemyList)
        {
            if (enemy.minLevel > level)
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
                Debug.LogError("No Enemy Selection");
                break;
            }
            Vector3 pos = RandomSpawnPosOnBorders();
            enemySpawner.SpawnEnemy(pos, Quaternion.identity, enemyList[finalIndex]);
        }
    }

    #region UtilityFunctions

    public Vector3 RandomSpawnPosOnBorders()
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

    private int GetScoreboardPosition(int score, SaveManager.SavedScores[] savedScores)
    {
        int[] scores = new int[10];
        for (int i = 0; i < savedScores.Length; i++)
        {
            scores[i] = savedScores[i].score;
        }
        for (int i = 0; i < savedScores.Length; i++)
        {
            if (score > scores[i])
            {
                return i;
            }
        }
        Debug.LogError("Error while getting position");
        return -1;
    }

    #endregion UtilityFunctions
}