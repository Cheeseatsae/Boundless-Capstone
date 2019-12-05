using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Ability3 : AbilityBase
{

    public PlayerModel player;
    private Rigidbody body;
    public GameObject explosionPref;
    public Vector3 boostDir;
    public float explosionRadius;
    public int damage;
    public float cooldown;
    private bool onCooldown;
    private float lifetime;
    public float jumpTime;

    public ParticleSystem particleSystem;
    public PlayerModel.AnimationAction AnimationEventAbility3;
    
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        lifetime = explosionPref.GetComponent<ParticleSystem>().main.startLifetime.constant;
    }
    
    void BlastOff()
    {
        if (onCooldown) return;
        AnimationEventAbility3?.Invoke();
        damage = (int)(player.attackDamage * 2.2f);
        particleSystem.Play();
        StartCoroutine(TurnOffParticles());
        onCooldown = true;
        StartCoroutine(StartCooldown());
        
        player.audio.PlaySound(5);
        
        body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z);
        body.AddRelativeForce(boostDir);
        //GameObject p = Instantiate(explosionPref, transform.position, Quaternion.identity);

        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider c in cols)
        {
            if (c.GetComponentInParent<PlayerModel>()) continue;

            Health h = c.GetComponentInParent<Health>();
            if (h != null)
            {
                h.DoDamage(damage);
                PlayerEvents.CallPlayerDamageEvent(c.gameObject, damage, c.ClosestPointOnBounds(transform.position));
            }
        }
        
        //Destroy(p, lifetime);        
    }

    IEnumerator StartCooldown()
    {
        PlayerUI.instance.QCooldown();
        yield return new WaitForSecondsRealtime(cooldown);
        onCooldown = false;
    }
    
    public override void Enter()
    {
        BlastOff();
    }

    IEnumerator TurnOffParticles()
    {
        yield return new WaitForSeconds(jumpTime);
        particleSystem.Stop();
    }
}
