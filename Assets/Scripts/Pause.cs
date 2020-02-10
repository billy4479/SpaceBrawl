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
    public AudioManager audioManager;

    void Start() { audioManager = AudioManager.instance; }

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
        gameManager.SuspendInput = false;
        audioManager.UnpauseSound("Music");
        PauseMenu.SetActive(false);
    }

    public void pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        gameManager.SuspendInput = true;
        audioManager.PauseSound("Music");
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

}
