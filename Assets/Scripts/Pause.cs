using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Pause : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenu;
    public GameManager gameManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                resume();
            else
            {
                if (Time.timeScale != 0)
                    pause();
            }
        }
    }

    public void resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
    }

    void pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
    }

    public void Menu()
    {
        resume();
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }

    public void BugReport()
    {
        using (var f = new StreamWriter(Application.dataPath + "/debug.txt"))
        {
            var prb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
            var pa = GameObject.Find("Player").GetComponent<Animator>();

            {
                f.WriteLine("Variables:");
                f.WriteLine(gameManager.EnemyNumber);
                f.WriteLine(gameManager.Level);
                f.WriteLine(gameManager.Lifes);
                f.WriteLine(gameManager.Score);

                f.WriteLine("Player:");

                f.WriteLine(prb.velocity);
                f.WriteLine(prb.angularVelocity);
                f.WriteLine(prb.position);
                f.WriteLine(pa.GetBool("Death"));
                f.WriteLine(pa.GetInteger("dir"));

            }

            f.Flush();
            f.Close();
        }
    }

}
