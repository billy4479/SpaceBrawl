using UnityEngine;

public class ControllMethodHandler : MonoBehaviour
{
    private AssetsHolder assetsHolder;
    private RectTransform positionJoystickTransform;
    private RectTransform fireJoystickTransform;
    private RectTransform canvas;
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
        positionJoystickTransform.localPosition = SetUIPositionRight(new Vector2(position, -position));
        fireJoystickTransform.localPosition = SetUIPositionLeft(new Vector2(position, position));
    }

    public void SetPosToggled()
    {
        positionJoystickTransform.localPosition = SetUIPositionLeft(new Vector2(position, position));
        fireJoystickTransform.localPosition = SetUIPositionRight(new Vector2(position, -position));
    }

    #endregion AdjustUIPosition

    private void Start()
    {
        sm = SaveManager.instance;
        assetsHolder = AssetsHolder.instance;
        positionJoystickTransform = assetsHolder.Joystick_Position_Transform;
        fireJoystickTransform = assetsHolder.Joystick_Fire_Transform;
        canvas = assetsHolder.Canvas;

        standardResolution = new Vector2(canvas.rect.width, canvas.rect.height);
        switch (sm.settings.controllMethod)
        {
            default:
                break;

            case SaveManager.ControllMethod.Finger:
                positionJoystickTransform.gameObject.SetActive(false);
                fireJoystickTransform.gameObject.SetActive(false);
                break;

            case SaveManager.ControllMethod.Joystick:
                positionJoystickTransform.gameObject.SetActive(true);
                fireJoystickTransform.gameObject.SetActive(true);

                positionJoystickTransform.anchorMin = new Vector2(1f, 0f);
                positionJoystickTransform.anchorMax = new Vector2(1f, 0f);
                fireJoystickTransform.anchorMin = new Vector2(0f, 0f);
                fireJoystickTransform.anchorMax = new Vector2(0f, 0f);

                SetPosNormal();
                break;

            case SaveManager.ControllMethod.ToggledJoystick:
                positionJoystickTransform.gameObject.SetActive(true);
                fireJoystickTransform.gameObject.SetActive(true);

                positionJoystickTransform.anchorMin = new Vector2(0f, 0f);
                positionJoystickTransform.anchorMax = new Vector2(0f, 0f);
                fireJoystickTransform.anchorMin = new Vector2(1f, 0f);
                fireJoystickTransform.anchorMax = new Vector2(1f, 0f);

                SetPosToggled();

                break;
        }
    }
}