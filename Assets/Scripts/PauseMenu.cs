﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    private GameObject pauseMenu;
    private GameManager gameManager;
    private AudioManager audioManager;
    private AssetsHolder assetsHolder;

    private void Start()
    {
        audioManager = AudioManager.instance;
        assetsHolder = AssetsHolder.instance;
        gameManager = GameManager.instance;
        pauseMenu = assetsHolder.Pause_Menu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
            {
                if (Time.timeScale != 0)
                    Pause();
            }
        }
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        gameManager.SuspendInput = false;
        audioManager.UnpauseSound("Music");
        pauseMenu.SetActive(false);
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        gameManager.SuspendInput = true;
        audioManager.PauseSound("Music");
        pauseMenu.SetActive(true);
    }

    public void Menu()
    {
        Resume();
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }
}