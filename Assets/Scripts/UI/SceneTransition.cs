using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour {
    public static IEnumerator Exit(float duration, Color color, Transform canvas, int iterations = 20) {
        var obj = Instantiate(new GameObject("SceneTransition_Fade"), Vector3.zero, Quaternion.identity, canvas);
        var imgRenderer = obj.AddComponent<Image>();
        imgRenderer.color = color;
        var panel = obj.GetComponent<RectTransform>();
        panel.anchorMin = Vector2.zero;
        panel.anchorMax = Vector2.one;
        panel.pivot = Vector2.one * .5f;
        panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2*Screen.width);
        panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2*Screen.height);

        Color finalColor = new Color(0, 0, 0, 0);
        float t = 0;
        float step = 1f / (float)iterations;
        float delay = duration / (float)iterations;
        while (t < 1) {
            imgRenderer.color = Color.Lerp(finalColor, color, t);
            t += step;
            yield return new WaitForSeconds(delay);
        }
        Destroy(obj);
    }
    public static IEnumerator Entry(float duration, Color color, Transform canvas, int iterations = 20) {
        var obj = Instantiate(new GameObject("SceneTransition_Fade"), Vector3.zero, Quaternion.identity, canvas);
        var imgRenderer = obj.AddComponent<Image>();
        imgRenderer.color = color;
        var panel = obj.GetComponent<RectTransform>();
        panel.anchorMin = Vector2.zero;
        panel.anchorMax = Vector2.one;
        panel.pivot = Vector2.one * .5f;
        panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 2*Screen.width);
        panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 2*Screen.height);


        Color initialColor = new Color(0, 0, 0, 0);
        float t = 0;
        float step = 1f / (float)iterations;
        float delay = duration / (float)iterations;
        while (t < 1) {
            imgRenderer.color = Color.Lerp(color, initialColor, t);
            t += step;
            yield return new WaitForSeconds(delay);
        }
        Destroy(obj);
    }
}