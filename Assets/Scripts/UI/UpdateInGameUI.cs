using TMPro;
using UnityEngine;

public class UpdateInGameUI : MonoBehaviour
{
    TextMeshProUGUI score;
    TextMeshProUGUI level;
    GameManager gm;
    private void Awake()
    {
        score = GameObject.Find("ScoreLabel").GetComponent<TextMeshProUGUI>();
        level = GameObject.Find("LevelLabel").GetComponent<TextMeshProUGUI>();
        gm = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        score.text = "Score: " + gm.score;
        level.text = "Level: " + gm.level;
    }
}
