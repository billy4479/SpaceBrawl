/*using UnityEngine;
using TMPro;

public class AssetsHolder : MonoBehaviour
{
    #region Singletone

    public static AssetsHolder instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion Singleton

    public EnemyStats[] Enemy_Stats;
    public GameObject Enemy_Base;
    public GameObject Bullet_Base;
    public GameObject Stella;
    public GameObject Player_Current;
    public PlayerStats[] Player_Stats;
    public int Player_Stats_Index;

    public TextMeshProUGUI Label_Score;
    public TextMeshProUGUI Label_Level;
    public GameObject Pause_Menu;

    public Joystick Joystick_Position;
    public Joystick Joystick_Fire;
    public RectTransform Joystick_Position_Transform;
    public RectTransform Joystick_Fire_Transform;
    public RectTransform Canvas;

    public Transform[] Anchors;
}*/

public enum Scenes
{
    Menu = 0,
    Game = 1,
    Scoreboard = 2,
    Credits = 3,
    Options = 4,
    StoryMode = 5
}