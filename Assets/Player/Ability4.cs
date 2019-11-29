using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ability4 : AbilityBase
{
    public PlayerModel player;
    public Collider[] cols;
    public float damageRadius;
    public float damageTimer;
    public int baseDamagePerTick;
    private int damagePerTick;
    public bool onCooldown;
    public float amountofTicks;
    public float cooldown;

    public GameObject laser;
    public int abilityDuration;
    public GameObject newLaser;
    
    private Vector3 aimTarget;

    public Transform aimTransform;
    
    public void Blaster()
    {
        if (onCooldown) return;
        onCooldown = true;
        damageTimer = abilityDuration / amountofTicks;

        StartCoroutine(RunBlaster());
        newLaser = Instantiate(laser, player.transform.forward, Quaternion.identity);
        Destroy(newLaser, abilityDuration);
        Visuals(newLaser);
    }

    IEnumerator RunBlaster()
    {
        
        for (int i = 0; i < amountofTicks; i++)
        {
            cols = Physics.OverlapCapsule(aimTransform.position, player.target, damageRadius);

            foreach (Collider c in cols)
            {
                if (c.GetComponentInParent<PlayerModel>()) continue;

                Health h = c.GetComponentInParent<Health>();
                if (h != null)
                {
                    h.DoDamage(damagePerTick);
                    PlayerEvents.CallPlayerDamageEvent(c.gameObject, damagePerTick, c.ClosestPointOnBounds(aimTransform.position));
                }
            }

            yield return new WaitForSeconds(damageTimer);
        }
        player.attackOccupied = false;
        StartCoroutine(Cooldown());
    }
    
    
    IEnumerator Cooldown()
    {
        PlayerUI.instance.RCooldown();
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    public override void Enter()
    {
        if (player.attackOccupied) return;
        if (!onCooldown)
        {
            player.attackOccupied = true;
            damagePerTick = baseDamagePerTick + (int)(player.attackDamage / amountofTicks);
            Blaster();
        }
        
    }
    
    public void Visuals(GameObject las)
    {
        StartCoroutine(Hold(las));
    }
    
    IEnumerator Hold(GameObject l)
    {
        float time = 0;
        while (time < abilityDuration)
        {
            time += Time.deltaTime;
            
            l.transform.position = aimTransform.position;
            RotateTowards(l.transform, player.target);
            
            yield return null;
        }               
    }

    private const float turnSpeed = 50;
    private void RotateTowards(Transform t, Vector3 p)
    {
        Vector3 targetDir = p - t.position;

        // The step size is equal to speed times frame time.
        float step = turnSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(t.forward, targetDir, step, 0.0f);

        // Move our position a step closer to the target.
        Vector3 newRot = Quaternion.LookRotation(newDir).eulerAngles;
        
        //Vector3 newRot = transform.rotation.eulerAngles;
        Quaternion y = Quaternion.Euler(newRot.x,newRot.y,newRot.z);
        
        t.rotation = y;
    }
}
