﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Ability1 : AbilityBase
{
    public PlayerModel player;
    public GameObject bulletPref;
    [HideInInspector] public float rangeInMetres;


    public float projectileSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        rangeInMetres = player.attackRange * projectileSpeed;
    }

    [Command]
    void CmdFire(float lifeTime, Vector3 target)
    {
        GameObject bullet = Instantiate(bulletPref, transform.position + transform.forward, Quaternion.Euler(90,90,0));
        
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = (target - transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;
        
        Destroy(bullet, lifeTime);
        
        NetworkServer.Spawn(bullet);
        //Debug.Log("test");
    }

    public override void Enter()
    {
        if(!isLocalPlayer) return; 
        
        // clients now accurately shoot but there's still a delay
        CmdFire(player.attackRange, player.target);
    }
}
