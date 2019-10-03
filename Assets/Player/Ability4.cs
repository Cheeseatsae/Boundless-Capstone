using System.Collections;
using System.Collections.Generic;
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
            cols = Physics.OverlapCapsule(transform.forward, player.target, damageRadius);

            foreach (Collider c in cols)
            {
                if (c.GetComponent<PlayerModel>()) continue;

                Health h = c.GetComponent<Health>();
                if (h != null) h.DoDamage(damagePerTick);
            }

            yield return new WaitForSeconds(damageTimer);
        }
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
        damagePerTick = baseDamagePerTick + (int)(player.attackDamage / amountofTicks);
        Blaster();
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
            l.transform.position = player.transform.localPosition;
            l.transform.rotation = player.view.rotation;
            
            yield return null;
        }               
    }

}
