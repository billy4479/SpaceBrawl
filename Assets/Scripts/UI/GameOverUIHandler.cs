using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lean.Transition;
using TMPro;

public class GameOverUIHandler : MonoBehaviour
{
    [SerializeField] private Image canvas;
    [SerializeField] private GameObject animatedCoso;
    [SerializeField] private TextMeshProUGUI label_Score;
    [SerializeField] private TextMeshProUGUI label_Level;
    [SerializeField] private GameObject button;
    private readonly string labelBase_Score = "Score: ";
    private readonly string labelBase_Level = "Level: ";
    private void Start()
    {
        GameManager.instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver(object sender, GameOverEventArgs e)
    {
        canvas.gameObject.SetActive(true);
        canvas.colorTransition(new Color(0, 0, 0, 1), 1f);
        StartCoroutine(AnimateScore(e.score));
        StartCoroutine(AnimateLevel(e.level));
    }

    private IEnumerator AnimateScore(int score)
    {
        yield return new WaitForSeconds(1f);
        animatedCoso.SetActive(true); //Lo so dovrei fare una funzione a parte
        label_Score.gameObject.SetActive(true);
        button.SetActive(true);
        if (score != 0)
        {
            int counter = 0;
            float delay = 2f / score;
            while (counter != score)
            {
                counter++;
                yield return new WaitForSeconds(delay);
                label_Score.text = labelBase_Score + counter;
            }
        }
        else
            label_Score.text = labelBase_Score + 0;
    }
    private IEnumerator AnimateLevel(int level)
    {
        yield return new WaitForSeconds(1f);
        label_Level.gameObject.SetActive(true);
        int counter = 0;
        float delay = 2f / level;
        while (counter != level)
        {
            counter++;
            yield return new WaitForSeconds(delay);
            label_Level.text = labelBase_Level + counter;
        }
    }

    public void OnButtonClick()
    {
        StopAllCoroutines();
        SceneManager.LoadScene((int)Scenes.Menu);
    }
}
