using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private AssetsHolder assetsHolder;
    private GameManager gameManager;
    private Vector3 player;
    private Vector3 oldPlayer;
    private Transform[] anchors;
    private Transform playerTransform;

    private void Start()
    {
        gameManager = GameManager.instance;
        assetsHolder = AssetsHolder.instance;
        playerTransform = assetsHolder.Player.transform;
        anchors = assetsHolder.Anchors;
        transform.position = new Vector3(0, 0, -10);
    }

    private void FixedUpdate()
    {
        try
        {
            player = playerTransform.position;
        }
        catch
        {
            player = oldPlayer;
        }

        Vector3 distFromOne = player - anchors[0].transform.position;
        Vector3 distFromTwo = player - anchors[1].transform.position;

        bool followOnlyY = false;
        bool followOnlyX = false;

        if (distFromOne.x > -gameManager.screenSize.x)
        {
            followOnlyY = true;
        }
        if (distFromOne.y > -gameManager.screenSize.y)
        {
            followOnlyX = true;
        }
        if (distFromTwo.x < gameManager.screenSize.x)
        {
            followOnlyY = true;
        }
        if (distFromTwo.y < gameManager.screenSize.y)
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