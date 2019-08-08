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
    
    private void Start()
    {
        player = GetComponent<PlayerModel>();
        damage = baseDamage;
        player.controller.OnMouse1Up += CmdForceLaunch;
    }

    private void OnDestroy()
    {
        player.controller.OnMouse1Up -= CmdForceLaunch;
    }

    [Command]
    void CmdCharge()
    {
        if (onCooldown) return;

        onCooldown = true;

        
        // PROJECTILE NOT SPAWNING AFTER SECOND ATTEMPT
        // ALSO CLIENT ISNT ABLE TO SPAWN PRETTY MUCH AT ALL AT THE MOMENT ITS TOTALLY SCREWED
        
        
        currentProjectile = Instantiate(projectilePref, transform.forward, Quaternion.identity);
        NetworkServer.Spawn(currentProjectile);
        
        RpcCharge(currentProjectile);
    }

    [ClientRpc]
    void RpcCharge(GameObject p)
    {
        currentProjectile = p;
        StartCoroutine(Charge());
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSecondsRealtime(cooldown);
        onCooldown = false;
    }

    IEnumerator Charge()
    {
        Vector3 scale = currentProjectile.transform.localScale * 2;
        float radius = explosionRadius * 1.5f;
        
        while (chargeTime < chargeDuration)
        {
            chargeTime += Time.deltaTime;

            currentProjectile.transform.position = transform.position + transform.forward;
            
            damage = (int)(baseDamage + (baseDamage * chargeTime / (chargeDuration / 2)));
            currentProjectile.transform.localScale = Vector3.Lerp(currentProjectile.transform.localScale,scale,chargeTime / chargeDuration);

            explosionRadius = Mathf.Lerp(explosionRadius,radius , chargeTime / chargeDuration);
            
            yield return null;
        }
        
        if (hasAuthority) CmdLaunch();
    }

    [Command]
    private void CmdForceLaunch()
    {
        RpcForceLaunch();
    }

    [ClientRpc]
    private void RpcForceLaunch()
    {
        chargeTime = chargeDuration + 1;
    }
    
    [Command]
    public void CmdLaunch()
    {
        if (currentProjectile == null) return;
        
        StartCoroutine(StartCooldown());
        Rigidbody bulletRb = currentProjectile.GetComponent<Rigidbody>();
        Vector3 dir = (player.target - transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;

        ChargeProjectile c = currentProjectile.GetComponent<ChargeProjectile>();

        c.damage = damage;
        c.explosionRadius = explosionRadius;
        c.RpcFire(bulletRb.velocity);  
        
    }
    
    public override void Enter()
    {
        if(!isLocalPlayer) return;

        chargeTime = 0;
        explosionRadius = baseExplosionRadius;
        damage = baseDamage;
        currentProjectile = null;

        // clients now accurately shoot but there's still a delay
        CmdCharge();
    
        // https://vis2k.github.io/Mirror/Concepts/GameObjects/SpawnObject
    }
}
