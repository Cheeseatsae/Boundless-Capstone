﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public KeyCode jump {get; set;}
    public KeyCode forward {get; set;}
    public KeyCode backward {get; set;}
    public KeyCode left {get; set;}
    public KeyCode right {get; set;}

    public string sJump = "Space";
    public string sForward = "W";
    public string sBack = "S";
    public string sLeft = "A";
    public string sRight = "D";

    public delegate void InputAction(float i);

    public event InputAction OnJumpInput;
    public event InputAction OnForwardInput;
    public event InputAction OnBackwardInput;
    public event InputAction OnLeftInput;
    public event InputAction OnRightInput;
    
    private void Awake()
    {
        jump = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", sJump));
        forward = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forwardKey", sForward));
        backward = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backwardKey", sBack));
        left = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", sLeft));
        right = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", sRight));
    }

    public void Update()
    {

        if (Input.GetKey(jump))
            OnJumpInput?.Invoke(1);
        else
            OnJumpInput?.Invoke(0);
        
        if (Input.GetKey(forward)) 
            OnForwardInput?.Invoke(1);
        else 
            OnForwardInput?.Invoke(0);
        
        if (Input.GetKey(backward)) 
            OnBackwardInput?.Invoke(1);
        else 
            OnBackwardInput?.Invoke(0);
        
        if (Input.GetKey(left)) 
            OnLeftInput?.Invoke(1);
        else
            OnLeftInput?.Invoke(0);
        
        if (Input.GetKey(right)) 
            OnRightInput?.Invoke(1);
        else
            OnRightInput?.Invoke(0);
        
    }
    
}