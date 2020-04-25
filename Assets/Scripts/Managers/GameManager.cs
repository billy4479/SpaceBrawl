using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private PlayerStats[] playerStats;
    private GameObject player;
    private Animator playerAnimator;
    private Transform[] anchors;

    public List<Rigidbody2D> EnemyRBs;
    public ShootingHandler shootingHandler;

    private AudioManager audioManager;
    private SaveManager saveManager;
    private EnemySpawner enemySpawner;
    private GameInfo gameInfo;

    //Game variables
    public bool suspendInput = false;
    public int score = 0;
    public int level = 1;
    public int enemyNumber = 0;

    [HideInInspector] public Vector2 screenSize { get; private set; }

    public event EventHandler<GameOverEventArgs> OnGameOver;

    public bool debug = false;

    #endregion Variables

    private void Awake()
    {
        audioManager = AudioManager.instance;
        saveManager = SaveManager.instance;
        enemySpawner = FindObjectOfType<EnemySpawner>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();

        gameInfo = saveManager.GetGameInfo();
        score = 0;
        level = 1;
        enemyNumber = 0;
        suspendInput = false;

        var anchorsGO = GameObject.FindGameObjectsWithTag("Anchor");
        anchors = new Transform[2];
        try
        {
            for (int i = 0; i < anchorsGO.Length; i++)
            {
                anchors[i] = anchorsGO[i].transform;
            }
        }
        catch
        {
            throw new IndexOutOfRangeException("Too many anchors");
        }

        screenSize = new Vector2(
            Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f,
            Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f
        );
    }

    private void Start()
    {
        enemySpawner.SpawnNewLevelEnemy(level, debug);
    }

    private IEnumerator LevelPassed()
    {
        enemyNumber = -1;
        level++;

        yield return new WaitForSeconds(2);

        enemySpawner.SpawnNewLevelEnemy(level, debug);
    }

    public void PlayerLose()
    {
        suspendInput = true;
        foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("Bullet"))
            Destroy(bullet);
        foreach (EnemyBase enemy in FindObjectsOfType<EnemyBase>())
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
        OnGameOver?.Invoke(this, new GameOverEventArgs() { score = score, coins = 0, isStoryMode = gameInfo.isStoryMode, level = level });
    }

    private void Update()
    {
        if (enemyNumber == 0 && !suspendInput)
            StartCoroutine(LevelPassed());
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
}

public class GameOverEventArgs
{
    public int score;
    public int level;
    public int coins;
    public bool isStoryMode;
}