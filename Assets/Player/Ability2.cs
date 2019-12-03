using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
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

    public GameObject particle;

    public GameObject currentParticle;

    public Transform aimTransform;
    
    public GameObject currentProjectile;
    
    private float chargeTime = 0;
    
    public PlayerModel.AnimationAction AnimationEventAbility2;

    private void Start()
    {
        player = GetComponent<PlayerModel>();
        
        onCooldown = false;
        player.controller.OnMouse1Up += ReleaseInput;
    }

    private void OnDestroy()
    {
        player.controller.OnMouse1Up -=ReleaseInput;
    }
    
    void Charge()
    {
        currentProjectile = Instantiate(projectilePref, aimTransform.position + aimTransform.forward, Quaternion.identity);
        currentParticle = Instantiate(particle, aimTransform.position, Quaternion.identity);
        currentParticle.transform.SetParent(aimTransform);
        chargeTime = 0;
        StartCoroutine(Hold());
    }

    IEnumerator StartCooldown()
    {
        yield return new WaitForSecondsRealtime(cooldown);
        onCooldown = false;
    }

    IEnumerator Hold()
    {
        Vector3 oldScale = currentProjectile.transform.localScale;
        Vector3 newScale = oldScale * 2.3f;
        
        while (chargeTime < chargeDuration)
        {
            chargeTime += Time.deltaTime;
            float chargeAmount = chargeTime / chargeDuration;
            
            currentProjectile.transform.position = aimTransform.position + aimTransform.forward + (aimTransform.up * 0.3f) + (aimTransform.right * 0.3f);
            currentProjectile.transform.localScale = Vector3.Lerp(oldScale, newScale, chargeAmount);
            damage = (int)Mathf.Lerp(baseDamage, baseDamage * 2, chargeAmount);
            explosionRadius = Mathf.Lerp(baseExplosionRadius, baseExplosionRadius * 2, chargeAmount);
            
            yield return null;
        }
        
        ReleaseInput();
    }
    
    private void ReleaseInput()
    {
        Launch(player.target);
        Destroy(currentParticle);
    }
    
    private void Launch(Vector3 target)
    {
        if (currentProjectile == null) return;

        player.attackOccupied = false;
        chargeTime = chargeDuration + 1;
        StartCoroutine(StartCooldown());
        
        Rigidbody bulletRb = currentProjectile.GetComponent<Rigidbody>();
        Vector3 dir = (target - transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;

        ChargeProjectile c = currentProjectile.GetComponent<ChargeProjectile>();
        
        c.damage = damage;
        c.explosionRadius = explosionRadius;
        c.Fire(player.attackRange);
        
    }

    public override void Enter()
    {
        if (player.attackOccupied) return;
        if (onCooldown) return;
        AnimationEventAbility2?.Invoke();
        player.attackOccupied = true;
        PlayerUI.instance.RMouseCooldown();
        onCooldown = true;
        chargeTime = 0;

        explosionRadius = baseExplosionRadius;
        baseDamage = (int)(player.attackDamage * 1.5f);
        damage = baseDamage;
        
        currentProjectile = null;
        
        Charge();
    }
}
