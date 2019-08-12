using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Mirror;
using PlayFab.ClientModels;
using UnityEngine.Serialization;

public class Ability2 : AbilityBase
{

    public PlayerModel player;
    private Rigidbody body;
    public GameObject projectilePref;
    public int baseDamage;
    private int damage;
    public float chargeDuration;
    public float projectileSpeed;
    public float baseExplosionRadius;
    private float explosionRadius;
    public float cooldown;
    private bool onCooldown;
    public GameObject currentProjectile;
    private float chargeTime = 0;
    private bool charging = false;
    
    private void Start()
    {
        player = GetComponent<PlayerModel>();
        damage = baseDamage;
        onCooldown = false;
        player.controller.OnMouse1Up += ReleaseInput;
    }

    private void OnDestroy()
    {
        player.controller.OnMouse1Up -=ReleaseInput;
    }

    [Command]
    void CmdCharge(Vector3 target)
    {
        
        currentProjectile = Instantiate(projectilePref, transform.position + transform.forward, Quaternion.identity);
        NetworkServer.Spawn(currentProjectile);
        RpcCharge(currentProjectile);
        
        Debug.Log("Spawning Projectile AAAAAAAAAAAAAAAAAAAAAAAAAA");
        
    }

    [ClientRpc]
    void RpcCharge(GameObject c)
    {
        charging = true;
        currentProjectile = c;
        StartCoroutine(Hold());
    }
    
    IEnumerator StartCooldown()
    {
        yield return new WaitForSecondsRealtime(cooldown);
        onCooldown = false;
    }

    IEnumerator Hold()
    {
        while (charging)
        {
            currentProjectile.transform.position = player.view.position + player.view.forward;
            yield return null;
        }
    }
    
    private void ReleaseInput()
    {
        if (isLocalPlayer) CmdLaunch(player.target);
    }

    [Command]
    private void CmdLaunch(Vector3 target)
    {
        if (currentProjectile == null) return;
        RpcLaunch();
        
        Rigidbody bulletRb = currentProjectile.GetComponent<Rigidbody>();
        Vector3 dir = (target - transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;

        ChargeProjectile c = currentProjectile.GetComponent<ChargeProjectile>();

        c.damage = damage;
        c.explosionRadius = explosionRadius;
        c.RpcFire(bulletRb.velocity);

    }
    
    [ClientRpc]
    private void RpcLaunch()
    {
        charging = false;
    }
    
    public override void Enter()
    {
        Debug.Log("Ability 2 go baby weeeeeeeeeeeee");
        //if (!isLocalPlayer) return;
        if (onCooldown) return;
        onCooldown = true;
        
        currentProjectile = null;

        // clients now accurately shoot but there's still a delay
        CmdCharge(player.target);
        StartCoroutine(StartCooldown());
        // https://vis2k.github.io/Mirror/Concepts/GameObjects/SpawnObject
    }
}
