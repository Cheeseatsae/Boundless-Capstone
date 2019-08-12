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

    public GameObject laser;
    public Transform laserPoint;
    public int abilityDuration;
    public LineRenderer lineRenderer;
    public ParticleSystem particleSystem;
    public Vector3 dir;
    public GameObject newLaser;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        RpcUpdateVisuals();

    }

    [Command]
    public void CmdBlaster()
    {
        if (onCooldown) return;
        onCooldown = true;
        damageTimer = abilityDuration / amountofTicks;

        StartCoroutine(RunBlaster());
        RpcVisuals();

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
        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    public override void Enter()
    {
        CmdBlaster();
    }

    [ClientRpc]
    public void RpcVisuals()
    {
        newLaser = Instantiate(laser, laserPoint.position, player.transform.rotation);
        newLaser.transform.parent = laserPoint.transform;
        lineRenderer = newLaser.GetComponentInChildren<LineRenderer>();
        
        
    }
    [ClientRpc]
    public void RpcUpdateVisuals()
    {
        if(lineRenderer != null)
        {
            //Debug.Log("this is running");
            lineRenderer.SetPosition(0, player.transform.position);
            lineRenderer.SetPosition(1, player.target);
        }
        
    }
}
