using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public float baseSpeed;
    public float speed;
    public float baseSprintSpeedMult;
    public float sprintSpeedMult;
    public int baseJumps;
    public int jumps;
    public float baseJumpHeight;
    public float jumpHeight;

    public Rigidbody body;
    
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
        
        
        
    }

    private void FixedUpdate()
    {
        
    }
}
