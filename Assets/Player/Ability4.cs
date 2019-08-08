using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Ability4 : AbilityBase
{
    public PlayerModel player;
    public Collider[] cols;
    public float damageRadius;
    public float damageTimer;
    public int damagePerTick;
    public bool onCooldown;
    public float amountofTicks;
    public float cooldown;

    public int abilityDuration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Command]
    public void CmdBlaster()
    {
        if (onCooldown) return;
        onCooldown = true;
        damageTimer = abilityDuration / amountofTicks;

        StartCoroutine(RunBlaster());

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
                if (h != null) h.CmdDoDamage(damagePerTick);
            }

            yield return new WaitForSeconds(damageTimer);
        }
        
        StartCoroutine(Cooldown());
    }
    
    
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    public override void Enter()
    {
        CmdBlaster();
    }
}
