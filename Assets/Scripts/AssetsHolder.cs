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

    public GameObject Enemy_1;
    public GameObject Enemy_2;
    public GameObject Enemy_3;
    public GameObject[] Enemy_List;
    public GameObject Bullet_Player;
    public GameObject Bullet_Enemy;
    public GameObject Stella;
    public GameObject Player;

    public TextMeshProUGUI Label_Score;
    public TextMeshProUGUI Label_Level;
    public GameObject Pause_Menu;
    public Joystick joystick;
    public RectTransform Joystick_Transform;
    public RectTransform FireButton_Transform;
    public RectTransform Canvas;

    public Transform[] Anchors;
}