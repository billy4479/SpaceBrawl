using UnityEngine;

public class Background : MonoBehaviour
{
    private AssetsHolder assetsHolder;
    private GameObject star;
    [SerializeField]private Transform[] anchors;
    private float divider = 8f;
    private float starSize = 3f;

    private void Awake()
    {
        foreach (var anchor in anchors)
            anchor.parent = null;
    }

    private void Start()
    {
        assetsHolder = AssetsHolder.instance;
        star = assetsHolder.Stella;
        anchors = assetsHolder.Anchors;

        int starNumX = Mathf.RoundToInt((Mathf.Abs(anchors[0].transform.position.x) + Mathf.Abs(anchors[1].transform.position.x)) / divider);
        int starNumy = Mathf.RoundToInt((Mathf.Abs(anchors[0].transform.position.y) + Mathf.Abs(anchors[1].transform.position.y)) / divider);
        star.transform.localScale = new Vector3(starSize, starSize, 1f);

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