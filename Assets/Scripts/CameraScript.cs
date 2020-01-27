using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameManager gm;

    private Vector3 player;
    private Vector3 oldPlayer;
    public List<GameObject> anchors;

    void Start()
    {
        transform.position = new Vector3(0, 0, -10);
    }

    void FixedUpdate()
    {
        try
        {
            player = gm.CurrentPlayer.transform.position;
        }
        catch
        {
            player = oldPlayer;
        }
        Vector3 distFromOne = player - anchors[0].transform.position;
        Vector3 distFromTwo = player - anchors[1].transform.position;

        bool followOnlyY = false;
        bool followOnlyX = false;

        if (distFromOne.x > -gm.screenSize.x)
        {
            followOnlyY = true;
        }
        if (distFromOne.y > -gm.screenSize.y)
        {
            followOnlyX = true;
        }
        if (distFromTwo.x < gm.screenSize.x)
        {
            followOnlyY = true;
        }
        if (distFromTwo.y < gm.screenSize.y)
        {
            followOnlyX = true;
        }

        if (followOnlyX && followOnlyY)
        { }
        else if (followOnlyX)
        {
            transform.position = new Vector3(player.x, transform.position.y, -10f);
        }
        else if (followOnlyY)
        {
            transform.position = new Vector3(transform.position.x, player.y, -10f);
        }
        else
        {
            transform.position = new Vector3(player.x, player.y, -10f);
        }
        oldPlayer = player;
    }


}
