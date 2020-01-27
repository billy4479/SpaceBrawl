using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public GameObject star;
    public Transform[] anchors;
    public float divider = 4f;

    void Awake()
    {
        foreach (var anchor in anchors)
            anchor.parent = null;
    }

    void Start()
    {

        int starNumX = Mathf.RoundToInt((Mathf.Abs(anchors[0].transform.position.x) + Mathf.Abs(anchors[1].transform.position.x)) / divider);
        int starNumy = Mathf.RoundToInt((Mathf.Abs(anchors[0].transform.position.y) + Mathf.Abs(anchors[1].transform.position.y)) / divider);
        star.transform.localScale = transform.localScale;

        for (int i = 0; i < starNumX; i++)
        {
            for (int j = 0; j < starNumy; j++)
            {
                var newstar = Instantiate(star, new Vector3(Random.Range((float)i, (float)i + 1f) * divider, Random.Range((float)j, (float)j + 1f) * divider, 0) + anchors[1].transform.position, Quaternion.identity);
                newstar.transform.SetParent(transform);
            }
        }
    }
}
