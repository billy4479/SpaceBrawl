using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("Menu");
    }
}