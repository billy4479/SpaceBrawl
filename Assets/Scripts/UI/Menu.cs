using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene((int)Scenes.Game);
    }

    public void ScoreBoardButton()
    {
        SceneManager.LoadScene((int)Scenes.Scoreboard);
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene((int)Scenes.Credits);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void OptionsButton()
    {
        SceneManager.LoadScene((int)Scenes.Options);
    }
    public void StoryModeButton()
    {
        SceneManager.LoadScene((int)Scenes.StoryMode);
    }
}