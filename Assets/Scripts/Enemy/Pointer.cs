using UnityEngine;

public class Pointer : MonoBehaviour
{
    private GameManager gm;
    private Rigidbody2D enemy;
    private Rigidbody2D player;

    private readonly float coeff = 3f;
    private readonly float moveInside = .5f;
    private Vector2 realPos = new Vector2();
    private Vector2 fakeScreenSize;

    private float[] angles;
    private Transform cam;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        cam = Camera.main.transform;
        enemy = transform.parent.GetComponent<Rigidbody2D>();
        transform.parent = null;
    }

    private void Start()
    {
        fakeScreenSize = gm.screenSize - Vector2.one * moveInside;
        angles = new float[] {
            Mathf.Atan2(fakeScreenSize.x, fakeScreenSize.y) * Mathf.Rad2Deg, //Grande (60,64225)
            Mathf.Atan2(fakeScreenSize.y, fakeScreenSize.x) * Mathf.Rad2Deg  //Piccolo (29,35775)
        };
    }

    private void FixedUpdate()
    {
        cam = Camera.main.transform;
        Vector2 direction = (enemy.position - player.position).normalized;

        //Rotation
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);

        #region Set Scale
        float scale = coeff / Vector2.Distance(enemy.position, player.position);
        if (scale < 0.1f)
            scale = 0.1f;
        transform.localScale = new Vector3(scale, scale * 2.5f, 1f);
        #endregion

        //Variables
        Vector2 finalPos = new Vector2();
        float cat;

        float angleDeg = CodeMonkey.Utils.UtilsClass.GetAngleFromVectorFloat(direction) - 90f;
        if (angleDeg > 180f) angleDeg -= 360f;
        float angleRad = angleDeg * Mathf.Deg2Rad;

        direction = (enemy.position - (Vector2)cam.position).normalized;
        angleDeg = CodeMonkey.Utils.UtilsClass.GetAngleFromVectorFloat(direction) - 90f;
        if (angleDeg > 180f) angleDeg -= 360f;

        //Chose the correct side and compute the position
        #region IF

        //Bottom
        if ((angleDeg < -90 - angles[1] && angleDeg > -180f) || (angleDeg > 90 + angles[1] && angleDeg <= 180f))
        {
            cat = Mathf.Tan(Mathf.PI - angleRad) * (gm.screenSize.y + (player.position.y - cam.position.y)) - player.position.x;
            realPos = new Vector2(-cat, -gm.screenSize.y + cam.position.y);

            cat = Mathf.Tan(Mathf.PI - angleRad) * (fakeScreenSize.y + (player.position.y - cam.position.y)) - player.position.x;
            finalPos = new Vector2(-cat, -fakeScreenSize.y + cam.position.y);
        }

        //Right
        if (angleDeg >= -90 - angles[1] && angleDeg <= -angles[0])
        {
            cat = Mathf.Tan(Mathf.PI * 0.5f - angleRad) * (-gm.screenSize.x + (player.position.x - cam.position.x)) + player.position.y;
            realPos = new Vector2(gm.screenSize.x + cam.position.x, cat);

            cat = Mathf.Tan(Mathf.PI * 0.5f - angleRad) * (-fakeScreenSize.x + (player.position.x - cam.position.x)) + player.position.y;
            finalPos = new Vector2(fakeScreenSize.x + cam.position.x, cat);
        }

        //Left
        if (angleDeg <= 90 + angles[1] && angleDeg >= angles[0])
        {
            cat = Mathf.Tan(Mathf.PI * 0.5f - angleRad) * (-gm.screenSize.x - (player.position.x - cam.position.x)) - player.position.y;
            realPos = new Vector2(-gm.screenSize.x + cam.position.x, -cat);

            cat = Mathf.Tan(Mathf.PI * 0.5f - angleRad) * (-fakeScreenSize.x - (player.position.x - cam.position.x)) - player.position.y;
            finalPos = new Vector2(-fakeScreenSize.x + cam.position.x, -cat);
        }

        //Up
        if (angleDeg > -angles[0] && angleDeg < angles[0])
        {
            cat = Mathf.Tan(angleRad) * (-gm.screenSize.y + (player.position.y - cam.position.y)) + player.position.x;
            realPos = new Vector2(cat, gm.screenSize.y + cam.position.y);

            cat = Mathf.Tan(angleRad) * (-fakeScreenSize.y + (player.position.y - cam.position.y)) + player.position.x;
            finalPos = new Vector2(cat, fakeScreenSize.y + cam.position.y);
        }

        #endregion IF

        #region Enable or disable renderer
        if (Vector2.Distance(cam.position, enemy.position) > Vector2.Distance(cam.position, realPos))
            GetComponent<Renderer>().enabled = true;
        else
            GetComponent<Renderer>().enabled = false;
        #endregion

        transform.position = finalPos;
    }

    private void OnDrawGizmosSelected()
    {
        if (enemy != null && player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(enemy.position, player.position);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(enemy.position, realPos);

            Vector2[] fakeScreenVerices = {
                new Vector2(fakeScreenSize.x + cam.position.x, fakeScreenSize.y + cam.position.y),
                new Vector2(-fakeScreenSize.x + cam.position.x, fakeScreenSize.y + cam.position.y),
                new Vector2(-fakeScreenSize.x + cam.position.x, -fakeScreenSize.y + cam.position.y),
                new Vector2(fakeScreenSize.x + cam.position.x, -fakeScreenSize.y + cam.position.y)
            };

            Gizmos.color = Color.green;
            Gizmos.DrawLine(fakeScreenVerices[0], fakeScreenVerices[2]);
            Gizmos.DrawLine(fakeScreenVerices[1], fakeScreenVerices[3]);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(fakeScreenVerices[0], fakeScreenVerices[1]);
            Gizmos.DrawLine(fakeScreenVerices[1], fakeScreenVerices[2]);
            Gizmos.DrawLine(fakeScreenVerices[2], fakeScreenVerices[3]);
            Gizmos.DrawLine(fakeScreenVerices[3], fakeScreenVerices[0]);

        }
    }
}