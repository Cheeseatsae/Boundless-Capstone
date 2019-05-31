using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public float baseSpeed;
    [HideInInspector]     public float speed;
    public float baseSprintSpeedMult;
    [HideInInspector] public float sprintSpeedMult;
    public int baseJumps;
    [HideInInspector] public int jumps;
    public float baseJumpHeight;
    [HideInInspector] public float jumpHeight;
    
    public Rigidbody body;

    public PlayerController controller;
    
    private float _forwardInput;
    private float _backInput;
    private float _leftInput;
    private float _rightInput;

//    public float baseHealth;
//    public float health;
//    public float baseMaxHealth;
//    public float maxHealth;
//    public float baseHealthRegen;
//    public float healthRegen;
//
//    public float baseAttackSpeed;
//    public float attackSpeed;
//    public float baseAttackRange;
//    public float attackRange;
//    public float baseAttackDamage;
//    public float attackDamage;

    private void Awake()
    {
        speed = baseSpeed;
        sprintSpeedMult = baseSprintSpeedMult;
        jumps = baseJumps;
        jumpHeight = baseJumpHeight;
        
        body = GetComponent<Rigidbody>();

        controller.OnJumpInput += UpdateJumpInput;
        
        controller.OnForwardInput += UpdateForwardInput;
        controller.OnBackwardInput += UpdateBackInput;
        controller.OnLeftInput += UpdateLeftInput;
        controller.OnRightInput += UpdateRightInput;
    }

    private void FixedUpdate()
    {
        Vector3 velocity = body.velocity;
        
        Vector3 force = new Vector3(_rightInput - _leftInput, 0, _forwardInput - _backInput) * speed * Time.deltaTime;
        body.AddRelativeForce(force);

        // if no input is detected and we're on the ground, smooth movement to a stop quickly
        if (_backInput + _leftInput + _rightInput + _forwardInput == 0)
        {
            float x = Mathf.Lerp(velocity.x, 0, 0.25f);
            float z = Mathf.Lerp(velocity.z, 0, 0.25f);
            body.velocity = new Vector3(x, velocity.y, z);
        }
    }

    private void OnDestroy()
    {
        controller.OnJumpInput -= UpdateJumpInput;
        
        controller.OnForwardInput -= UpdateForwardInput;
        controller.OnBackwardInput -= UpdateBackInput;
        controller.OnLeftInput -= UpdateLeftInput;
        controller.OnRightInput -= UpdateRightInput;
    }
    
    private void UpdateForwardInput(float i)
    {
        _forwardInput = i;
    }
    
    private void UpdateBackInput(float i)
    {
        _backInput = i;
    }
    
    private void UpdateLeftInput(float i)
    {
        _leftInput = i;
    }
    
    private void UpdateRightInput(float i)
    {
        _rightInput = i;
    }

    private void UpdateJumpInput()
    {
        body.velocity = new Vector3(body.velocity.x, jumpHeight, body.velocity.z);
    }

}
