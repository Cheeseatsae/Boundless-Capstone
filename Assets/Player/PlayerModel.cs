using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Mirror;
using UnityEngine;

public class PlayerModel : NetworkBehaviour
{
    [Header("Movement Variables")]
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
    
    [Header("References")]
    public Rigidbody body;
    public PlayerController controller;
    public Camera myCam;
    public GameObject viewObject;
    public AbilityBase ability1;
    public AbilityBase ability3;

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

    [Header("Attacking Variables")]
    public float baseAttackSpeed;
    [HideInInspector] public float attackSpeed;
    public float baseAttackRange;
    [HideInInspector] public float attackRange;
    public float baseAttackDamage;
    [HideInInspector] public float attackDamage;

    [Header("Aiming Variables")]
    public Vector3 target;
    public float sphereRadius;
    public float maxRayDistance;
    public float aimingThreshold;
    public float playerDistanceThreshold;
    public float fallbackAimDistance;
    public LayerMask mask;

    // Floor checking variables  
    [Serializable]
    public struct Crumb
    {
        public Vector3 pos;
        public Transform obj;
    }
    
    private RaycastHit floorCheck;
    
    [Header("Floor Check Variables")]
    public int floor;
    public List<Crumb> crumbTrail = new List<Crumb>();
    
    private Color c;
    
    private void Awake()
    {
        
        speed = baseSpeed;
        maxSpeed = baseMaxSpeed;
        sprintSpeedMult = baseSprintSpeedMult;
        jumps = baseJumps;
        remainingJumps = jumps;
        jumpHeight = baseJumpHeight;
        attackSpeed = baseAttackSpeed;
        attackRange = baseAttackRange;
        attackDamage = baseAttackDamage;
        
        body = GetComponent<Rigidbody>();

        controller.OnJumpInput += JumpInputDown;
        controller.OnShiftInputDown += ShiftInputDown;
        controller.OnShiftInputUp += ShiftInputUp;
        
        controller.OnForwardInput += UpdateForwardInput;
        controller.OnBackwardInput += UpdateBackInput;
        controller.OnLeftInput += UpdateLeftInput;
        controller.OnRightInput += UpdateRightInput;
        
        controller.OnMouse0Input += OnMouse0Input;
        controller.OnMouse1Input += OnMouse1Input;

        ability1 = GetComponent<Ability1>();
        ability3 = GetComponent<Ability3>();
    }

    private void Start()
    {
        myCam = CameraControl.playerCam.GetComponentInChildren<Camera>();

        if (isLocalPlayer)
        {
            CameraControl.playerCam.followObj = this.gameObject;
            StartCoroutine(MarkPreviousPosition());
            SetupPlayFab();
        }
    }

    private IEnumerator MarkPreviousPosition()
    {
        yield return new WaitForSeconds(0.3f);
        
        Physics.Raycast(transform.position, Vector3.down, out floorCheck, 100);
        
        Crumb firstC = new Crumb { pos = transform.position, obj = floorCheck.collider.gameObject.transform };

        crumbTrail.Add(firstC);

        while (true)
        {
            yield return new WaitForSeconds(1);

            Physics.Raycast(transform.position, Vector3.down, out floorCheck, 100);

            if (floorCheck.point == Vector3.zero) continue;
            if (floorCheck.collider.gameObject.layer != floor) continue;

        if (crumbTrail.Count < 5)
            {
                Crumb newC = new Crumb { pos = transform.position, obj = floorCheck.collider.gameObject.transform };
                crumbTrail.Insert(0, newC);
            }
            else
            {
                crumbTrail.RemoveAt(crumbTrail.Count - 1);
                Crumb newC = new Crumb { pos = transform.position, obj = floorCheck.collider.gameObject.transform };
                crumbTrail.Insert(0, newC);
            }
        }
    }

    public void MovePlayerToSolidGround()
    {
        Vector2 pos = new Vector2(crumbTrail[0].pos.x, crumbTrail[0].pos.z);
        Vector3 objPos = new Vector2(crumbTrail[0].obj.position.x, crumbTrail[0].obj.position.z);

        float x = Mathf.Lerp(pos.x, objPos.x, 0.2f);
        float y = crumbTrail[0].pos.y + 3;
        float z = Mathf.Lerp(pos.y, objPos.y, 0.2f);
        
        Vector3 spawnPos = new Vector3(x,y,z);

        transform.position = spawnPos;
    }

    // test aim location for player
    private void OnDrawGizmos()
    {
        Gizmos.color = c;
        Gizmos.DrawSphere(target, 0.5f);
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
        
        if (isLocalPlayer) Targeting();
    }

    public float camOffset;
    
    private void Targeting()
    {
        viewObject.transform.LookAt(target);

        Transform camTransform = myCam.transform;
        Vector3 camPos = camTransform.position;
        Vector3 camFwd = camTransform.forward;
        
        Ray r = new Ray(camPos + (camFwd * camOffset), camFwd * (camOffset + 1));

        Physics.Raycast(r, out RaycastHit ray, maxRayDistance, mask);
        Physics.SphereCast(r, sphereRadius, out RaycastHit sphere, maxRayDistance, mask);
        
        if (ray.point != Vector3.zero) // if ray hits
        {
            if (Vector3.Distance(ray.point, sphere.point) > aimingThreshold)
            { // if ray and sphere are too far apart use ray
                target = ray.point;
                c = Color.green;
            }
            else // if ray and sphere are close
            {
                if (Vector3.Distance(sphere.point, transform.position) < playerDistanceThreshold)
                { // if sphere is too close to player use ray
                    target = ray.point;
                    c = Color.yellow;
                }
                else
                { // if sphere is far enough from player use sphere
                    target = sphere.point;
                    c = Color.blue;
                }
            }
            
            Debug.DrawLine(transform.position, target, Color.green, 0.05f);
        }
        else // if ray misses
        {
            if (Vector3.Distance(sphere.point, transform.position) > playerDistanceThreshold && sphere.point != Vector3.zero)
            { // if sphere hit is far enough from player
                target = sphere.point;
                c = Color.red;
            }
            else // if sphere is too close
            {
                target = camPos + (camFwd * fallbackAimDistance);
                c = Color.magenta;
            }

            Debug.DrawLine(transform.position, target, Color.red, 0.05f);
        }
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
        ability3.Enter();

        //GetComponent<Ability1>().CmdColourChange();
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

        controller.OnMouse0Input -= OnMouse0Input;
        controller.OnMouse1Input -= OnMouse1Input;

    }

    // PLAYFAB TESTING

    [Header("PlayFab")] 
    public GameObject playFabUserObj;

    private PlayFabUser playfabUser;
    
    private void SetupPlayFab()
    {
        playfabUser = Instantiate(playFabUserObj, Vector3.zero, Quaternion.identity).GetComponent<PlayFabUser>();
        playfabUser.player = GetComponent<NetworkIdentity>();
    }

}
