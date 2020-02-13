using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreboardScript : MonoBehaviour
{
    private string ScoreText;
    private TextMeshProUGUI ScoreboardText;
    private SaveManager saveManager;


    public void Start()
    {
        saveManager = SaveManager.instance;
        ScoreText = "";

        for (int i = 0; i < 9; i++)
        {
            ScoreText = ScoreText + "#" + (i + 1) + ". \t " +
            saveManager.scores.name[i] + " \t " +
            saveManager.scores.date[i] + " \t " +
            saveManager.scores.score[i] + "\n";
        }

        ScoreboardText = GameObject.Find("ScoreLabel").GetComponent<TextMeshProUGUI>();
        ScoreboardText.text = ScoreText;
    }

    public void OnButtonPress()
    {
        SceneManager.LoadScene("Menu");
    }

}
