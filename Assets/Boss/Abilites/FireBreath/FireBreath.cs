using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreath : Boss_Ability_Base
{
    public GameObject fireBreath;
    public ParticleSystem fireParticle;
    public Transform fireStartPos;
    public float precast;
    public GameObject flame;
    public int amountOfTicks;
    public int damagePerTick;
    public float damageTimer;
    public ColliderScript cols;

    public override void Start()
    {
        base.Start();
    }


    public override void Cast()
    {
        base.Cast();
        
        Flame();

    }

    public void Update()
    {
        if (flame != null)
        {
            flame.transform.LookAt(model.target.transform);
        }
    }


    public IEnumerator PreCast()
    {
        yield return new WaitForSeconds(precast);
        Flame();
    }
    
    public void Flame()
    {
        flame = Instantiate(fireBreath, fireStartPos.position, Quaternion.identity);
        flame.transform.parent = fireStartPos;
        cols = flame.GetComponentInChildren<ColliderScript>();
        fireParticle = flame.GetComponentInChildren<ParticleSystem>();
        
        cols.EnterTrigger += DoDamage;
        cols.ExitTrigger += StopDamage;
        StartCoroutine(Casting());
    }

    public IEnumerator Damage(Collider player)
    {
        {
            for (int i = 0; i < amountOfTicks; i++)
            {
                Health h = player.GetComponent<Health>();
                if (h != null) h.DoDamage(damagePerTick);

                yield return new WaitForSeconds(damageTimer);
            }
        }
    }
    private Coroutine co;
    public void DoDamage(Collider other)
    {
        if (other.GetComponent<PlayerModel>())
        {
            co = StartCoroutine(Damage(other));
        }
        
    }
    private void StopDamage(Collider other)
    {
        if (other.GetComponent<PlayerModel>())
        {
            StopCoroutine(co);
        }
    }

    public void StopFire()
    {
        fireParticle.Stop();
    }

    public override void FinishCast()
    {
        base.FinishCast();
        Destroy(flame);
    }
}
