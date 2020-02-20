using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ScoreBoardButton()
    {
        SceneManager.LoadScene("ScoreBoard");
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void OptionsButton()
    {
        SceneManager.LoadScene("Options");
    }
}