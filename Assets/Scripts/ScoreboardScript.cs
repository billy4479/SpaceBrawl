using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreboardScript : MonoBehaviour
{
    string ScoreText;
    TextMeshProUGUI ScoreboardText;
    SaveManager saveManager;


    public void Awake()
    {
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();

        for (int i = 0; i < 9; i++)
        {
            ScoreText = ScoreText + "#" + (i + 1) + ".\t" + SaveManager.scores.name[i] + "\t" + SaveManager.scores.score[i] + "\n";
        }

        ScoreboardText = GameObject.Find("ScoreLabel").GetComponent<TextMeshProUGUI>();
        ScoreboardText.text = ScoreText;

        saveManager.WriteScoreToFile();
    }

    public void OnButtonPress()
    {
        SceneManager.LoadScene("Menu");
    }

}
