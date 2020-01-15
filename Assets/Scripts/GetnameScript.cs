using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GetnameScript : MonoBehaviour
{
    public TMP_InputField InputField;
    public TextMeshProUGUI Message;
    public GameObject errormsg;
    private int Score;
    public void EndGame(int aScore)
    {
        Score = aScore;
        errormsg.SetActive(false);
        Message.text = "Congraturations!\nYou made a new record! Your score was " + Score + "!";
    }

    public void OnButtonClick()
    {
        if (InputField.text.Length > 0 && InputField.text.Length <= 10)
        {
            Time.timeScale = 1f;
            int position = 0;
            for (int i = 0; i < 9; i++)
            {
                if (Score > SaveManager.scores.score[i])
                {
                    position = i;
                    break;
                }
            }

            for (int i = 9; i > position - position; i--)
            {
                SaveManager.scores.score[i] = SaveManager.scores.score[i - 1];
                SaveManager.scores.name[i] = SaveManager.scores.name[i - 1];
            }

            SaveManager.scores.name[position] = InputField.text;
            SaveManager.scores.score[position] = Score;

            SceneManager.LoadScene("Scoreboard");
        }
        else
        {
            errormsg.SetActive(true);
        }

    }
    public void HideErr()
    {
        errormsg.SetActive(false);
    }
}
