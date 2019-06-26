using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerModel : NetworkBehaviour
{
    public float baseSpeed;
    [HideInInspector] public float speed;
    public float baseMaxSpeed;
    [HideInInspector] public float maxSpeed;
    public float baseSprintSpeedMult;
    [HideInInspector] public float sprintSpeedMult;
    public int baseJumps;
    [HideInInspector] public int jumps;
    private int remainingJumps;
    public float baseJumpHeight;
    [HideInInspector] public float jumpHeight;
    
    public Rigidbody body;
    public PlayerController controller;
    public Camera myCam;
    public Vector3 target;

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

    public AbilityBase ability1;

    private void Awake()
    {
        
        speed = baseSpeed;
        maxSpeed = baseMaxSpeed;
        sprintSpeedMult = baseSprintSpeedMult;
        jumps = baseJumps;
        remainingJumps = jumps;
        jumpHeight = baseJumpHeight;
        
        body = GetComponent<Rigidbody>();

        controller.OnJumpInput += JumpInputDown;
        controller.OnShiftInputDown += ShiftInputDown;
        controller.OnShiftInputUp += ShiftInputUp;
        
        controller.OnForwardInput += UpdateForwardInput;
        controller.OnBackwardInput += UpdateBackInput;
        controller.OnLeftInput += UpdateLeftInput;
        controller.OnRightInput += UpdateRightInput;
        
        controller.OnMouse0Input += OnMouse0Input;

        ability1 = GetComponent<Ability1>();
    }

    private void Start()
    {
        myCam = CameraControl.playerCam.GetComponentInChildren<Camera>();

        if (isLocalPlayer)
            CameraControl.playerCam.followObj = this.gameObject;
    }

    // for aiming at items - Not going to work, should have consistent aiming direction for everything not separate ones for items;
    private void Update()
    {
        if (!isLocalPlayer) return;
        
        target = myCam.transform.position + (myCam.transform.forward * 10);
        
        RaycastHit h;
        if (Physics.Linecast(transform.position, target, out h))
        {
            Debug.DrawLine(transform.position, h.point, Color.green, 0.05f);
        }
        
    }

    // test aim location for player
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(target, 0.1f);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = body.velocity;
        
        Vector3 force = new Vector3(_rightInput - _leftInput, 0, _forwardInput - _backInput) * 10 * speed * Time.deltaTime;
        body.AddRelativeForce(force);

        // if no input is detected and we're on the ground, smooth movement to a stop quickly
        if (_backInput + _leftInput + _rightInput + _forwardInput == 0)
        {
            float x = Mathf.Lerp(velocity.x, 0, 0.2f);
            float z = Mathf.Lerp(velocity.z, 0, 0.2f);
            body.velocity = new Vector3(x, velocity.y, z);
        }

        // Clamped x + z magnitude
        Vector2 v = new Vector2(body.velocity.x, body.velocity.z);
        v = Vector2.ClampMagnitude(v, maxSpeed);
        body.velocity = new Vector3(v.x, body.velocity.y, v.y);
    }

    private void OnCollisionEnter(Collision other)
    {
        foreach (ContactPoint c in other.contacts)
        {
            Debug.DrawRay(c.point, c.normal, Color.red, 2);

            if (c.normal.y > 0.4f)
                remainingJumps = jumps;

        }
    }
    
    // Actions to perform when input is received 
    
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

    private void JumpInputDown()
    {
        if (remainingJumps <= 0) return;
        
        CmdJump();
        body.velocity = new Vector3(body.velocity.x, jumpHeight, body.velocity.z);
        remainingJumps--;
    }

    [Command]
    private void CmdJump()
    {
        RpcJump();
    }

    [ClientRpc]
    private void RpcJump()
    {
        body.velocity = new Vector3(body.velocity.x, jumpHeight, body.velocity.z);
    }
    
    private void ShiftInputDown()
    {
        speed *= sprintSpeedMult;
        maxSpeed *= sprintSpeedMult;
    }
    
    private void ShiftInputUp()
    {
        speed /= sprintSpeedMult;
        maxSpeed /= sprintSpeedMult;
    }

    private void OnMouse0Input()
    {
        ability1.Enter();
    }
    private void OnMouse1Input()
    {
        
    }

    private void OnDestroy()
    {
        controller.OnJumpInput -= JumpInputDown;
        controller.OnShiftInputDown -= ShiftInputDown;
        controller.OnShiftInputUp -= ShiftInputUp;
        
        controller.OnForwardInput -= UpdateForwardInput;
        controller.OnBackwardInput -= UpdateBackInput;
        controller.OnLeftInput -= UpdateLeftInput;
        controller.OnRightInput -= UpdateRightInput;
        
    }

}
