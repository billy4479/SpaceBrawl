using UnityEngine;
using TMPro;

public class AssetsHolder : MonoBehaviour
{
    #region Singleton

    public static AssetsHolder instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    #endregion Singleton

    public EnemyStats[] Enemy_Stats;
    public GameObject Enemy_Base;
    public GameObject Bullet_Player;
    public GameObject Bullet_Enemy;
    public GameObject Stella;
    public GameObject Player_Current;
    public PlayerStats[] Player_Stats;
    public int Player_Stats_Insex;

    public TextMeshProUGUI Label_Score;
    public TextMeshProUGUI Label_Level;
    public GameObject Pause_Menu;

    public Joystick Joystick_Position;
    public Joystick Joystick_Fire;
    public RectTransform Joystick_Position_Transform;
    public RectTransform Joystick_Fire_Transform;
    public RectTransform Canvas;

    public Transform[] Anchors;
}