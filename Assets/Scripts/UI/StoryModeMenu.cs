using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryModeMenu : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene((int)Scenes.Menu);
    }
}
