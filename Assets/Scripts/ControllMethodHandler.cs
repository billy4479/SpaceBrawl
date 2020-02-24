using UnityEngine;

public class ControllMethodHandler : MonoBehaviour
{
    public RectTransform joystick;
    public RectTransform fireButton;
    public RectTransform canvas;

    private SaveManager sm;

    #region AdjustUIPosition

    private float position = 200;
    private Vector2 standardResolution;

    private Vector3 SetUIPositionRight(Vector2 givenPos)
    {
        Vector3 result = new Vector3();

        result.x = standardResolution.x / 2 - givenPos.x;
        result.y = -standardResolution.y / 2 - givenPos.y;
        result.z = 0f;
        return result;
    }

    private Vector3 SetUIPositionLeft(Vector2 givenPos)
    {
        Vector3 result = new Vector3();

        result.x = -standardResolution.x / 2 + givenPos.x;
        result.y = -standardResolution.y / 2 + givenPos.y;
        result.z = 0f;
        return result;
    }

    public void SetPosNormal()
    {
        joystick.localPosition = SetUIPositionRight(new Vector2(position, -position));
        fireButton.localPosition = SetUIPositionLeft(new Vector2(position, position));
    }

    public void SetPosToggled()
    {
        joystick.localPosition = SetUIPositionLeft(new Vector2(position, position));
        fireButton.localPosition = SetUIPositionRight(new Vector2(position, -position));
    }

    #endregion AdjustUIPosition

    private void Start()
    {
        sm = SaveManager.instance;
        standardResolution = new Vector2(canvas.rect.width, canvas.rect.height);
        switch (sm.settings.controllMethod)
        {
            default:
                break;

            case SaveManager.ControllMethod.Finger:
                joystick.gameObject.SetActive(false);
                fireButton.gameObject.SetActive(false);
                break;

            case SaveManager.ControllMethod.Joystick:
                joystick.gameObject.SetActive(true);
                fireButton.gameObject.SetActive(true);

                joystick.anchorMin = new Vector2(1f, 0f);
                joystick.anchorMax = new Vector2(1f, 0f);
                fireButton.anchorMin = new Vector2(0f, 0f);
                fireButton.anchorMax = new Vector2(0f, 0f);

                SetPosNormal();
                break;

            case SaveManager.ControllMethod.ToggledJoystick:
                joystick.gameObject.SetActive(true);
                fireButton.gameObject.SetActive(true);

                joystick.anchorMin = new Vector2(0f, 0f);
                joystick.anchorMax = new Vector2(0f, 0f);
                fireButton.anchorMin = new Vector2(1f, 0f);
                fireButton.anchorMax = new Vector2(1f, 0f);

                SetPosToggled();

                break;
        }
    }
}