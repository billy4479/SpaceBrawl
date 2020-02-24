using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    public List<GameObject> Enemies = new List<GameObject>();

    #region GUI

    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI LevelLabel;

    #endregion GUI

    public GameObject player;
    public List<Rigidbody2D> EnemyRBs;
    public List<Transform> anchors;
    public ShootingHandler shootingHandler;
    public Animator playerAnimator;

    public bool SuspendInput = false;
    public int Score = 0;
    public int Level = 1;
    public int EnemyNumber = 0;

    private AudioManager audioManager;
    private bool wasAlive = false;

    private enum Side { UP, DOWN, RIGHT, LEFT };

    [HideInInspector]
    public static Vector2 screenSize; //Spostare in background.cs?

    #endregion Variables

    private void Awake()
    {
        this.Score = 0;
        this.Level = 1;
        this.EnemyNumber = 0;
        this.SuspendInput = false;
    }

    private void Start()
    {
        audioManager = AudioManager.instance;
        player.GetComponent<HeathSystem>().DieEvent += PlayerLose;

        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;

        SpawnNewEnemies(Level);
    }

    private IEnumerator PlayerWon()
    {
        EnemyNumber = -1;
        Level++;

        yield return new WaitForSeconds(2);

        SpawnNewEnemies(Level);
    }

    private void PlayerLose(object sender, EventArgs e)
    {
        SuspendInput = true;
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

        if (Score > SaveManager.instance.scores[9].score)
        {
            SaveManager.SavedScores[] scr = SaveManager.instance.scores;

            int position = GetScoreboardPosition(Score, scr);

            for (int i = 9; i >= position; i--)
            {
                if (i == 9)
                    continue;
                scr[i + 1].score = scr[i].score;
                scr[i + 1].date = scr[i].date;
                scr[i + 1].name = scr[i].name;
            }
            scr[position].score = Score;
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
        ScoreLabel.text = "Score: " + Score;
        LevelLabel.text = "Level: " + Level;
        #endregion
        #region Screensize
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
        #endregion
        
        if (EnemyNumber == 0)
            StartCoroutine(PlayerWon());
    }

    private void SpawnNewEnemies(int EnemiesToSpawn)
    {
        for (int i = 0; i < EnemiesToSpawn; i++)
        {
            int arrLen = Enemies.Count;
            int totProb = 0;
            for (int j = 0; j < arrLen; j++)
            {
                totProb += Enemies[j].GetComponent<EnemyBase>().prob;
            }
            int randNum = UnityEngine.Random.Range(0, totProb);

            int finalIndex = -1;
            int accumulo = 0;

            for (int j = 0; j < arrLen; j++)
            {
                if (randNum < Enemies[j].GetComponent<EnemyBase>().prob + accumulo)
                {
                    finalIndex = j;
                    break;
                }
                else
                    accumulo += Enemies[j].GetComponent<EnemyBase>().prob;
            }
            if (finalIndex == -1)
            {
                Debug.LogError("No Enemy Selection");
                break;
            }
            Vector3 pos = RandomSpawnPosOnBorders();
            Instantiate(Enemies[finalIndex], pos, Quaternion.AngleAxis(Mathf.Atan2((player.transform.position - pos).normalized.y, (player.transform.position - pos).normalized.x) * Mathf.Rad2Deg - 90f, new Vector3(0, 0, 1)));
        }
        EnemyNumber = Level;
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
    #endregion
}