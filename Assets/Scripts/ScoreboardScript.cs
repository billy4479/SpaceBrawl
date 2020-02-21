using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            ScoreText = ScoreText + "#" + (i + 1) + ".    " +
            saveManager.scores.name[i] + "    " +
            saveManager.scores.date[i] + "    " +
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