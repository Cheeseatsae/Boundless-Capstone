using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    
    public KeyCode jump {get; set;}
    public KeyCode forward {get; set;}
    public KeyCode backward {get; set;}
    public KeyCode left {get; set;}
    public KeyCode right {get; set;}
    public KeyCode mouse0 { get; set; }
    public KeyCode mouse1 { get; set; }
    public KeyCode shift { get; set; }
    public KeyCode interact { get; set; }
    public KeyCode qkey { get; set; }
    public KeyCode rkey { get; set; }
    
    public string sJump = "Space";
    public string sForward = "W";
    public string sBack = "S";
    public string sLeft = "A";
    public string sRight = "D";
    public string sMouse0 = "Mouse0";
    public string sMouse1 = "Mouse1";
    public string sShift = "LeftShift";
    public string sInteract = "E";
    public string sQKey = "Q";
    public string sRKey = "R";
    
    public delegate void InputAxis(float i);

    public event InputAxis OnForwardInput;
    public event InputAxis OnBackwardInput;
    public event InputAxis OnLeftInput;
    public event InputAxis OnRightInput;

    public delegate void InputAction();

    public event InputAction OnJumpInput;
    public event InputAction OnMouse0Input;
    public event InputAction OnMouse1Input;
    public event InputAction OnShiftInputDown;
    public event InputAction OnShiftInputUp;
    public event InputAction OnInteractInput;
    public event InputAction OnQKeyInput;
    public event InputAction OnRKeyInput;
    
    private void Awake()
    {
        jump = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("jumpKey", sJump));
        forward = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forwardKey", sForward));
        backward = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backwardKey", sBack));
        left = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", sLeft));
        right = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", sRight));
        shift = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("shiftKey", sShift));
        mouse0 = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Mouse0", sMouse0));
        mouse1 = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Mouse1", sMouse1));
        interact = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("interactKey", sInteract));
        qkey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("qKey", sQKey));
        rkey = (KeyCode) Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rKey", sRKey));
    }

    public void Update()
    {
        
        if(!isLocalPlayer)
            return;

        // Button Push
        if (Input.GetKeyDown(jump))
            OnJumpInput?.Invoke();
        
        if (Input.GetKeyDown(shift))
            OnShiftInputDown?.Invoke();
        
        if (Input.GetKeyUp(shift))
            OnShiftInputUp?.Invoke();
        
        if (Input.GetKeyDown(mouse0))
            OnMouse0Input?.Invoke();
            
        if (Input.GetKeyDown(mouse1))
            OnMouse1Input?.Invoke();
        
        if (Input.GetKeyDown(interact))
            OnInteractInput?.Invoke();
        
        if (Input.GetKeyDown(qkey))
            OnQKeyInput?.Invoke();
            
        if (Input.GetKeyDown(rkey))
            OnRKeyInput?.Invoke();
            
        // Axis
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