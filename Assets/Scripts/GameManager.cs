using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region variables
    public GameObject[] Enemies;

    public GameObject PlayerPrefab;
    public TextMeshProUGUI LifesLabel;
    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI LevelLabel;
    public List<Rigidbody2D> EnemyRBs;
    public List<Transform> anchors;
    public ShootingHandler shootingHandler;

    public bool SuspendInput = false;
    public int Lifes = 3;
    public int Score = 0;
    public int Level = 1;
    public int EnemyNumber = 0;
    [Range(0f, 1f)]
    public float bounciness;
    [Range(0f, 1f)]
    public float friction;
    public GameObject CurrentPlayer;
    private AudioManager audioManager;

    private enum sides { UP, DOWN, RIGHT, LEFT };

    public Vector2 screenSize;
    private Vector3 cameraPos;
    #endregion

    private void Awake()
    {
        this.Score = 0;
        this.Lifes = 3;
        this.Level = 1;
        this.EnemyNumber = 0;
        this.SuspendInput = false;
        cameraPos = Camera.main.transform.position;
    }

    private void Start()
    {
        audioManager = AudioManager.instance;

        CurrentPlayer = Instantiate(this.PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        shootingHandler.SetNewPlayer(CurrentPlayer);

        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;

        this.SpawnNewEnemies(this.Level);
    }

    private IEnumerator PlayerWon()
    {
        this.EnemyNumber = -1;
        this.Level++;

        yield return new WaitForSeconds(3);

        this.SpawnNewEnemies(this.Level);
    }

    public void PlayerLose()
    {
        this.Lifes -= 1;

        if (this.Lifes == 0)
        {
            if (this.Score > SaveManager.instance.scores.score[0])
            {
                SaveManager.SavedScores scr = SaveManager.instance.scores;
                Destroy(CurrentPlayer);
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
                for (int i = 0; i < bullets.Length; i++)
                    Destroy(bullets[i]);
                GameObject[] pointers = GameObject.FindGameObjectsWithTag("Pointer");
                for (int i = 0; i < pointers.Length; i++)
                    Destroy(pointers[i]);
                this.SuspendInput = true;

                for (int i = scr.score.Length - 1; i > 0; i--)
                {
                    if (i == 0)
                        continue;

                    scr.score[i] = scr.score[i - 1];
                    scr.date[i] = scr.date[i - 1];
                    scr.name[i] = scr.name[i - 1];
                }
                scr.score[0] = this.Score;
                scr.name[0] = SaveManager.instance.settings.name;
                scr.date[0] = System.DateTime.Now.ToString("d", System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR"));

                SaveManager.instance.WriteChanges();
            }
            SceneManager.LoadScene("Menu");

            return;
        }

        StartCoroutine(this.PlayerRespawning());
    }

    private IEnumerator PlayerRespawning()
    {
        this.SuspendInput = true;
        foreach (GameObject pointer in GameObject.FindGameObjectsWithTag("Pointer"))
            Destroy(pointer);

        Rigidbody2D PlayerRB = CurrentPlayer.GetComponent<Rigidbody2D>();
        PlayerRB.velocity = Vector2.zero;
        PlayerRB.angularVelocity = 0;

        Animator playerAnimator = CurrentPlayer.GetComponent<Animator>();
        playerAnimator.SetBool("Death", true);
        audioManager.PlaySound("PlayerExplosion");

        yield return new WaitForSeconds(1);

        playerAnimator.SetBool("Death", false);
        Destroy(CurrentPlayer);
        shootingHandler.RemovePlayer();

        yield return new WaitForSeconds(3);

        CurrentPlayer = Instantiate(this.PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        shootingHandler.SetNewPlayer(CurrentPlayer);

        audioManager.StopSound("PlayerExplosion");

        yield return new WaitForSeconds(1);

        this.SuspendInput = false;

        this.SpawnNewEnemies(this.Level);

    }

    private void SpawnNewEnemies(int EnemiesToSpawn)
    {
        for (int i = 0; i < EnemiesToSpawn; i++)
        {
            int arrLen = Enemies.Length;
            int totProb = 0;
            for (int j = 0; j < arrLen; j++)
            {
                totProb += Enemies[j].GetComponent<EnemyBase>().prob;
            }
            int randNum = Random.Range(0, totProb);

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
            Instantiate(this.Enemies[finalIndex], pos, Quaternion.AngleAxis(Mathf.Atan2((CurrentPlayer.transform.position - pos).normalized.y, (CurrentPlayer.transform.position - pos).normalized.x) * Mathf.Rad2Deg - 90f, new Vector3(0, 0, 1)));
        }
        this.EnemyNumber = this.Level;
    }

    private Vector3 RandomSpawnPosOnBorders()
    {
        Vector3 result = new Vector3(0, 0, 0);
        sides side = (sides)Random.Range(0, 4);

        switch (side)
        {
            case sides.UP:
                result = new Vector3(Random.Range(anchors[0].position.x, -anchors[0].position.x), anchors[0].position.y, 0);
                break;
            case sides.DOWN:
                result = new Vector3(Random.Range(anchors[1].position.x, -anchors[1].position.x), anchors[1].position.y, 0);
                break;
            case sides.RIGHT:
                result = new Vector3(Random.Range(anchors[0].position.y, -anchors[0].position.y), anchors[0].position.x, 0);
                break;
            case sides.LEFT:
                result = new Vector3(Random.Range(anchors[1].position.y, -anchors[1].position.y), anchors[1].position.x, 0);
                break;
            default:
                Debug.LogError("No random number in enemy spawning function");
                result = new Vector3(0, 0, 0);
                break;
        }
        return result;
    }

    private void FixedUpdate()
    {
        this.LifesLabel.text = "Lifes: " + this.Lifes;
        this.ScoreLabel.text = "Score: " + this.Score;
        this.LevelLabel.text = "Level: " + this.Level;
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;


        if (this.EnemyNumber == 0)
            StartCoroutine(this.PlayerWon());
    }
}