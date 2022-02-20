using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageInputs : MonoBehaviour
{
    [SerializeField] GameObject joystick, jumpButton, nub;
    bool justReleased, justPressed, pressedDown;
    [SerializeField] float deadzone; // between 0 and 1
    RectTransform joystickTransform;
    private void Start()
    {
        joystickTransform = joystick.GetComponent<RectTransform>();
    }

    private void Update()
    {
        //reset jump triggers
        justPressed = false;
        justReleased = false;

        Vector2 joystickInput = GetJoystick();

        RectTransform nubR = nub.GetComponentInChildren<RectTransform>();
        nubR.localPosition = joystickInput * joystick.GetComponent<RectTransform>().rect.height/4;
        //Debug.Log(joystickInput);
        
    }

    public Vector2 GetJoystick()
    {

        float touchRadius = joystick.transform.localScale.x / 2;

        if (Input.touchCount <= 0) return new Vector2(0, 0);
        Touch touch = Input.GetTouch(0);

        Vector2 toReturn = new Vector2(touch.position.x - (joystickTransform.anchoredPosition.x), touch.position.y - joystickTransform.anchoredPosition.y); // get vector that gives touch vs. joystick origin

        if (toReturn.magnitude > 750) return new Vector2(0, 0); // Make sure that the finger isn't TOO far away from the joystick.

        toReturn /= joystickTransform.rect.width; // "normalize" vector to get each value between -1 and 1
        toReturn = new Vector2(Mathf.Min(Mathf.Max(-1, toReturn.x), 1), Mathf.Min(Mathf.Max(-1, toReturn.y), 1)) * 2; // make sure no value goes above 1 or below -1

        if (Mathf.Abs(toReturn.x) < deadzone) toReturn = new Vector2(0, toReturn.y);
        if (Mathf.Abs(toReturn.y) < deadzone) toReturn = new Vector2(toReturn.x, 0);

        return toReturn;
    }

    

    public void JumpPress()
    {
        Debug.Log("Press");
        justPressed = true;
        pressedDown = true;
    }

    public void jumpRelease()
    {
        Debug.Log("Release");
        justReleased = true;
        pressedDown = false;
    }

    public bool wasJumpPressed()
    {
        return justPressed;
    }

    public bool wasJumpReleased()
    {
        return justReleased;
    }

    public bool isJumpDown()
    {
        return pressedDown;
    }
}
