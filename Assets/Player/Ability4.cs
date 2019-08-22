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
    public int baseDamagePerTick;
    private int damagePerTick;
    public bool onCooldown;
    public float amountofTicks;
    public float cooldown;

    public GameObject laser;
    public Transform laserPoint;
    public int abilityDuration;
    public Vector3 dir;
    public GameObject newLaser;

    [Command]
    public void CmdBlaster()
    {
        if (onCooldown) return;
        onCooldown = true;
        damageTimer = abilityDuration / amountofTicks;

        StartCoroutine(RunBlaster());
        newLaser = Instantiate(laser, player.transform.forward, Quaternion.identity);
        //newLaser.transform.parent = laserPoint.transform;
        NetworkServer.Spawn(newLaser);
        RpcVisuals(newLaser);
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
        NetworkServer.Destroy(newLaser);
        Destroy(newLaser, abilityDuration);
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
        CmdBlaster();
    }

    [ClientRpc]
    public void RpcVisuals(GameObject las)
    {
        StartCoroutine(Hold(las));

    }
    [Command]
    public void CmdVisuals()
    {
        if (newLaser != null)
        {
            newLaser.transform.position = player.transform.localPosition;
        }
        //RpcVisuals();
    }
  
    IEnumerator Hold(GameObject l)
    {
        float time = 0;
        while (time < abilityDuration)
        {
            time += Time.deltaTime;
            l.transform.position = player.transform.localPosition;
            l.transform.rotation = player.view.rotation;
            
//            Vector3 targetDir = player.target - transform.position;
//            float step = speed * Time.deltaTime;
//            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
//            
//            l.transform.rotation = Quaternion.LookRotation(newDir);
//            
            yield return null;
        }               
    }

}
