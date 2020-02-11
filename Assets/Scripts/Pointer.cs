using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private GameManager gm;
    private Rigidbody2D enemy;
    private Rigidbody2D player;

    private float coeff = 3f;
    private Vector2 realPos = new Vector2();

    private Vector2[] points = new Vector2[4];
    private float[] angles;

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemy = transform.GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.parent = null;

        points[0] = new Vector2(gm.screenSize.x, gm.screenSize.y);
        points[1] = new Vector2(-gm.screenSize.x, gm.screenSize.y);
        points[2] = new Vector2(-gm.screenSize.x, -gm.screenSize.y);
        points[3] = new Vector2(gm.screenSize.x, -gm.screenSize.y);


        float[] dotProd = {
        points[0].x * points[1].x + points[0].y * points[1].y,
        points[1].x * points[2].x + points[1].y * points[2].y,
        };

        float[] _angles =  {
            Mathf.Acos(dotProd[0] / (points[0].magnitude * points[1].magnitude)) * Mathf.Rad2Deg,
            Mathf.Acos(dotProd[1] / (points[1].magnitude * points[2].magnitude)) * Mathf.Rad2Deg,
        };
        angles = _angles;

    }

    private void FixedUpdate()
    {
        //Find the player or destroy gameObject
        try
        {
            player = gm.CurrentPlayer.GetComponent<Rigidbody2D>();
        }
        catch
        {
            Destroy(gameObject);
        }

        //Set Direction
        Vector2 direction = (enemy.position - player.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);

        //Set Scale
        float scale = coeff / Vector2.Distance(enemy.position, player.position);
        if (scale < 0.1f)
            scale = 0.1f;
        transform.localScale = new Vector3(scale, scale * 2.5f, 1f);

        //Variables
        Vector2 finalPos;
        float cat = 0f;
        float sub = .5f;
        float ratio = 1f;

        float angleRad = Mathf.Atan2(enemy.position.x - player.position.x, enemy.position.y - player.position.y);
        float angleDeg = angleRad * Mathf.Rad2Deg;

        //Chose the correct side and compute the position
        #region IF
        //Bottom
        if ((angleDeg > 180f - angles[0] * .5f && angleDeg < 180f) || (angleDeg < -180f + angles[0] * .5f && angleDeg > -180f))
        {
            cat = Mathf.Tan(angleRad) * gm.screenSize.y - Camera.main.transform.position.x;
            realPos = new Vector2(-cat, -gm.screenSize.y + Camera.main.transform.position.y);
        }

        //Right
        if (angleDeg < 180f - angles[0] * .5 && angleDeg > angles[0] - angles[1])
        {
            cat = Mathf.Tan(angleRad - Mathf.PI / 2f) * -gm.screenSize.x + Camera.main.transform.position.y;
            realPos = new Vector2(gm.screenSize.x + Camera.main.transform.position.x, cat);
        }

        //Left
        if (angleDeg < -angles[0] + angles[1] && angleDeg > -180f + angles[0] * .5f)
        {
            cat = Mathf.Tan(angleRad - Mathf.PI / 2f) * -gm.screenSize.x - Camera.main.transform.position.y;
            realPos = new Vector2(-gm.screenSize.x + Camera.main.transform.position.x, -cat);
        }

        //Up
        if ((angleDeg < angles[0] * .5f && angleDeg > 0f) || (angleDeg > -angles[0] * .5f && angleDeg < 0f))
        {
            cat = Mathf.Tan(angleRad) * gm.screenSize.y + Camera.main.transform.position.x;
            realPos = new Vector2(cat, gm.screenSize.y + Camera.main.transform.position.y);
        }
        #endregion

        //Taking nearer to the player
        #region OnScreen
        ratio = ((realPos.x - Camera.main.transform.position.x) / (realPos.y - Camera.main.transform.position.y));

        float finY = Mathf.Sqrt(Mathf.Pow(sub, 2f) / (Mathf.Pow(ratio, 2f) + 1f));
        float finX = ratio * finY;

        if (enemy.position.y > Camera.main.transform.position.y)
            finalPos = realPos - new Vector2(finX, finY);
        else
            finalPos = realPos + new Vector2(finX, finY);
        #endregion

        if (Vector2.Distance(player.position, enemy.position) > Vector2.Distance(player.position, realPos))
            GetComponent<Renderer>().enabled = true;
        else
            GetComponent<Renderer>().enabled = false;

        //Setting final position
        transform.position = finalPos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(enemy.position, player.position);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(enemy.position, realPos);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(player.position, points[0]);
        Gizmos.DrawLine(player.position, points[1]);
        Gizmos.DrawLine(player.position, points[2]);
        Gizmos.DrawLine(player.position, points[3]);
    }

}
