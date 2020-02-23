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

        for (int i = 0; i < 10; i++)
        {
            ScoreText = ScoreText + "#" + (i + 1) + ".\t" +
            saveManager.scores[i].name + "    " +
            saveManager.scores[i].date + "    " +
            saveManager.scores[i].score + "\n";
        }

        ScoreboardText = GameObject.Find("ScoreLabel").GetComponent<TextMeshProUGUI>();
        ScoreboardText.text = ScoreText;
    }

    public void OnButtonPress()
    {
        SceneManager.LoadScene("Menu");
    }
}