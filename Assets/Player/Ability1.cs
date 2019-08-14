using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Ability1 : AbilityBase
{
    public PlayerModel player;
    public GameObject bulletPref;
    [HideInInspector] public float rangeInMetres;

    public float baseCooldown;
    [HideInInspector] public float cooldown;
    private bool onCooldown;

    public bool firing;
    
    public float projectileSpeed;

    private void Start()
    {
        firing = false;
        onCooldown = false;
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
        
        bullet.GetComponent<Projectile>().RpcSetVelocity(bulletRb.velocity);
        bullet.GetComponent<Damager>().RpcSetDamage((int)player.attackDamage);
        //Debug.Log("test");
    }

    [Command]
    public void CmdColourChange()
    {
        RpcColourChange();
    }

    [ClientRpc]
    private void RpcColourChange()
    {
        bulletPref.AddComponent<BoxCollider>();
        bulletPref.transform.localScale *= 2;
    }
    
    IEnumerator StartCooldown()
    {
        yield return new WaitForSecondsRealtime(cooldown);
        onCooldown = false;
    }
    
    public override void Enter()
    {
        if(!isLocalPlayer) return;

        firing = true;
        
        
    }

    public override void Exit()
    {
        if(!isLocalPlayer) return;

        firing = false;
    }

    private void FixedUpdate()
    {
        if (onCooldown) return;
        if (!firing) return;

        onCooldown = true;
        cooldown = baseCooldown / (0.1f * player.attackSpeed);
        
        CmdFire(player.attackRange, player.target);
        StartCoroutine(StartCooldown());
    }
}
