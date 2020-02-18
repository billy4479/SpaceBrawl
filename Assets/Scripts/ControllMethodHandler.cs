using UnityEngine;

public class ControllMethodHandler : MonoBehaviour
{
    public RectTransform joystick;
    public RectTransform fireButton;

    private SaveManager sm;
    private float position = 200;

    private Vector3 SetUIPositionRight(Vector3 givenPos)
    {
        Vector3 result = new Vector3();
        result.x = Screen.width/2 - givenPos.x;
        result.y = -Screen.height/2 - givenPos.y;
        result.z = givenPos.z;
        return result;
    }
    private Vector3 SetUIPositionLeft(Vector3 givenPos)
    {
        Vector3 result = new Vector3();
        result.x = -Screen.width / 2 + givenPos.x;
        result.y = -Screen.height / 2 + givenPos.y;
        result.z = givenPos.z;
        return result;
    }

    private void Start()
    {
        sm = SaveManager.instance;
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
    private void SetPosNormal()
    {
        joystick.localPosition = SetUIPositionRight(new Vector3(position, -position));
        fireButton.localPosition = SetUIPositionLeft(new Vector3(position, position));
    }
    private void SetPosToggled()
    {
        joystick.localPosition = SetUIPositionLeft(new Vector3(position, position));
        fireButton.localPosition = SetUIPositionRight(new Vector3(position, -position));
    }
}