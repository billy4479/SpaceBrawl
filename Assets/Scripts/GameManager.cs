using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /*
     TO DO:
     * Audio
     * Sistema Nemici Multipli OK
     * Eredità player
     * Menu Personaggi-Difficoltà
     * Tipi di nemici diversi (sprite nuovi?)
    */

    public GameObject[] Enemies;

    public GameObject PlayerPrefab;
    public TextMeshProUGUI LifesLabel;
    public TextMeshProUGUI ScoreLabel;
    public TextMeshProUGUI LevelLabel;
    public GameObject GetNamesPanel;
    public GetnameScript getNameScript;

    public bool SuspendInput = false;
    public int Lifes = 3;
    public int Score = 0;
    public int Level = 1;
    public int EnemyNumber = 0;
    [Range(0f, 1f)]
    public float bounciness;
    [Range(0f, 1f)]
    public float friction;
    private GameObject CurrentPlayer;

    private enum sides { UP, DOWN, RIGHT, LEFT };

    //Bordo
    private float colDepth = 1f;
    private float zPosition = 0f;
    private Vector2 screenSize;
    private Transform topCollider;
    private Transform bottomCollider;
    private Transform leftCollider;
    private Transform rightCollider;
    private Vector3 cameraPos;


    private void Awake()
    {
        this.Score = 0;
        this.Lifes = 3;
        this.Level = 1;
        this.EnemyNumber = 0;
        this.SuspendInput = false;

        CreateBorders();

        CurrentPlayer = Instantiate(this.PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

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
            if (this.Score > SaveManager.scores.score[9])
            {
                Destroy(CurrentPlayer);
                GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
                for (int i = 0; i < bullets.Length; i++)
                {
                    Destroy(bullets[i]);
                }
                GetNamesPanel.SetActive(true);
                this.SuspendInput = true;
                Time.timeScale = 0f;
                getNameScript.EndGame(this.Score);
                return;
            }
            SceneManager.LoadScene("Menu");

            return;
        }

        StartCoroutine(this.PlayerRespawning());
    }

    private IEnumerator PlayerRespawning()
    {
        this.SuspendInput = true;

        Rigidbody2D PlayerRB = CurrentPlayer.GetComponent<Rigidbody2D>();
        PlayerRB.velocity = Vector2.zero;
        PlayerRB.angularVelocity = 0;

        Animator playerAnimator = CurrentPlayer.GetComponent<Animator>();
        playerAnimator.SetBool("Death", true);

        yield return new WaitForSeconds(1);

        playerAnimator.SetBool("Death", false);
        Destroy(CurrentPlayer);

        yield return new WaitForSeconds(3);

        CurrentPlayer = Instantiate(this.PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

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

            Instantiate(this.Enemies[finalIndex], RandomSpawnPosOnBorders(), Quaternion.identity);
        }
        this.EnemyNumber = this.Level;
    }

    private Vector3 RandomSpawnPosOnBorders()
    {
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;
        Vector3 result = new Vector3(0, 0, 0);
        sides side = (sides)Random.Range(0, 4);

        switch (side)
        {
            case sides.UP:
                result = new Vector3(Random.Range(screenSize.x, --screenSize.x), screenSize.y, 0);
                break;
            case sides.DOWN:
                result = new Vector3(Random.Range(screenSize.x, -screenSize.x), -screenSize.y, 0);
                break;
            case sides.RIGHT:
                result = new Vector3(screenSize.x, Random.Range(screenSize.y, -screenSize.y), 0);
                break;
            case sides.LEFT:
                result = new Vector3(-screenSize.x, Random.Range(screenSize.y, -screenSize.y), 0);
                break;
            default:
                Debug.LogError("No random number in enemy spawning function");
                result = new Vector3(0, 0, 0);
                break;
        }
        return result;
    }

    private void Update()
    {
        this.LifesLabel.text = "Lifes: " + this.Lifes;
        this.ScoreLabel.text = "Score: " + this.Score;
        this.LevelLabel.text = "Level: " + this.Level;

        if (this.EnemyNumber == 0)
            StartCoroutine(this.PlayerWon());
    }
    private void CreateBorders()
    {
        //Generate our empty objects
        topCollider = new GameObject().transform;
        bottomCollider = new GameObject().transform;
        rightCollider = new GameObject().transform;
        leftCollider = new GameObject().transform;

        //Name our objects 
        topCollider.name = "TopCollider";
        bottomCollider.name = "BottomCollider";
        rightCollider.name = "RightCollider";
        leftCollider.name = "LeftCollider";

        //Add tag
        topCollider.tag = "Border";
        bottomCollider.tag = "Border";
        rightCollider.tag = "Border";
        leftCollider.tag = "Border";

        //Add the colliders
        topCollider.gameObject.AddComponent<BoxCollider2D>();
        bottomCollider.gameObject.AddComponent<BoxCollider2D>();
        rightCollider.gameObject.AddComponent<BoxCollider2D>();
        leftCollider.gameObject.AddComponent<BoxCollider2D>();

        //Add materials to Colliders
        PhysicsMaterial2D mat = new PhysicsMaterial2D();
        mat.bounciness = this.bounciness;
        mat.friction = this.friction;
        topCollider.GetComponent<BoxCollider2D>().sharedMaterial = mat;
        bottomCollider.GetComponent<BoxCollider2D>().sharedMaterial = mat;
        rightCollider.GetComponent<BoxCollider2D>().sharedMaterial = mat;
        leftCollider.GetComponent<BoxCollider2D>().sharedMaterial = mat;

        //Make them the child of whatever object this script is on, preferably on the Camera so the objects move with the camera without extra scripting
        topCollider.parent = Camera.main.transform;
        bottomCollider.parent = Camera.main.transform;
        rightCollider.parent = Camera.main.transform;
        leftCollider.parent = Camera.main.transform;

        //Generate world space point information for position and scale calculations
        cameraPos = Camera.main.transform.position;
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;

        //Change our scale and positions to match the edges of the screen...   
        rightCollider.localScale = new Vector3(colDepth, screenSize.y * 2, colDepth);
        rightCollider.position = new Vector3(cameraPos.x + screenSize.x + (rightCollider.localScale.x * 0.5f), cameraPos.y, zPosition);
        leftCollider.localScale = new Vector3(colDepth, screenSize.y * 2, colDepth);
        leftCollider.position = new Vector3(cameraPos.x - screenSize.x - (leftCollider.localScale.x * 0.5f), cameraPos.y, zPosition);
        topCollider.localScale = new Vector3(screenSize.x * 2, colDepth, colDepth);
        topCollider.position = new Vector3(cameraPos.x, cameraPos.y + screenSize.y + (topCollider.localScale.y * 0.5f), zPosition);
        bottomCollider.localScale = new Vector3(screenSize.x * 2, colDepth, colDepth);
        bottomCollider.position = new Vector3(cameraPos.x, cameraPos.y - screenSize.y - (bottomCollider.localScale.y * 0.5f), zPosition);

    }
}