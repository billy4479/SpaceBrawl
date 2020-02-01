using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private GameManager gm;
    private Rigidbody2D enemy;
    private Rigidbody2D player;

    private float coeff = 3f;
    private float cat = 0f;
    private Vector2 realPos = new Vector2();

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemy = transform.GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        transform.parent = null;
    }

    private void FixedUpdate()
    {
        try
        {
            player = gm.CurrentPlayer.GetComponent<Rigidbody2D>();
        }
        catch
        {
            Destroy(gameObject);
        }
        Vector2 direction = (enemy.position - player.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90);

        float scale = coeff / Vector2.Distance(enemy.position, player.position);
        if (scale < 0.1f)
            scale = 0.1f;

        transform.localScale = new Vector3(scale, scale * 2.5f, 1f);

        Vector2 finalPos;
        float sub = .5f;
        float ratio = 1f;

        float angleRad = Mathf.Atan2(enemy.position.x - player.position.x, enemy.position.y - player.position.y);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        float maxAngle = Mathf.Atan2(gm.screenSize.x, gm.screenSize.y) * Mathf.Rad2Deg;

        if ((angleDeg > 120f && angleDeg < 180f) || (angleDeg < -120f && angleDeg > -180f))
        {
            cat = Mathf.Tan(angleRad) * gm.screenSize.y - player.position.x;
            realPos = new Vector2(-cat, -gm.screenSize.y + player.position.y);
        }
        if (angleDeg < 120f && angleDeg > 60f)
        {
            cat = Mathf.Tan(angleRad - Mathf.PI / 2f) * -gm.screenSize.x + player.position.y;
            realPos = new Vector2(gm.screenSize.x + player.position.x, cat);
        }
        if (angleDeg < -60f && angleDeg > -120f)
        {
            cat = Mathf.Tan(angleRad - Mathf.PI / 2f) * -gm.screenSize.x - player.position.y;
            realPos = new Vector2(-gm.screenSize.x + player.position.x, -cat);
        }
        if ((angleDeg < 60f && angleDeg > 0f) || (angleDeg > -60f && angleDeg < 0f))
        {
            cat = Mathf.Tan(angleRad) * gm.screenSize.y + player.position.x;
            realPos = new Vector2(cat, gm.screenSize.y + player.position.y);
        }

        ratio = ((realPos.x - player.position.x) / (realPos.y - player.position.y));

        float finY = Mathf.Sqrt(Mathf.Pow(sub, 2f) / (Mathf.Pow(ratio, 2f) + 1f));
        float finX = ratio * finY;

        if (enemy.position.y > player.position.y)
            finalPos = realPos - new Vector2(finX, finY);
        else
            finalPos = realPos + new Vector2(finX, finY);

        transform.position = finalPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(enemy.position, player.position);
        Gizmos.color = Color.red;

        Gizmos.DrawLine(enemy.position, realPos);
    }

}
