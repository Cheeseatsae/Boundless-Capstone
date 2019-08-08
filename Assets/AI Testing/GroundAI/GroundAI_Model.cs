using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class GroundAI_Model : AIBaseModel
{
    public GameObject damageZone;
    
    //Ground Slam Variables
    public bool meleeCooldown = false;
    public float targetDistance;
    public float meleeCoolDown;
    public AIDamager slamDamager;
    public GameObject damage;
    
    //Ranged Ability Variables
    public float rangedCoolDown;
    public GameObject bulletPref;
    public bool rangedCooldown = false;
    public float projectileSpeed;
    public bool cantFire = false;

    private NavMeshAgent navmesh;
    // Start is called before the first frame update

    private void Awake()
    {
        navmesh = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        aiManager = FindObjectOfType<AIManager>();
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (!isServer) return;

        if (target != null)
        {
            navmesh.destination = target.transform.position;
            
            RaycastHit hit;
        
            if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit))
            {
            
                if (hit.collider.gameObject == target)
                {
                    if (rangedCooldown == false)
                    {
                        navmesh.isStopped = true;
                        navmesh.velocity = new Vector3(0,0,0);
                        StartCoroutine(RangedWait());
                        
                        rangedCooldown = true;
                        cantFire = false;
                        StartCoroutine(RangedCooldown());
                    }               
                }
            }

            targetDistance = Vector3.Distance(transform.position, target.transform.position);
            if (targetDistance < 5)
            {
                if (meleeCooldown == false)
                {
                    cantFire = true;
                    navmesh.isStopped = true;
                    navmesh.velocity = new Vector3(0,0,0);
                    meleeCooldown = true;
                    CmdSpawnDamager();
                    
                    StartCoroutine(MeleeCooldown());
                }
                
            }
        }
        
    }

    
    
    [Command]
    public void CmdFire()
    {
        if (target == null) return;
        
        GameObject bullet = Instantiate(bulletPref, transform.position + transform.forward, Quaternion.identity);
        
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        Vector3 dir = (target.transform.position - transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;
        NetworkServer.Spawn(bullet);
    }
    
    [Command]
    public void CmdGroundSlam()
    {
        
        slamDamager.SlamDamage();
        navmesh.isStopped = false;

    }
    
    public IEnumerator RangedCooldown()
    {
        yield return new WaitForSeconds(rangedCoolDown);
        rangedCooldown = false;

    }
    
    public IEnumerator MeleeCooldown()
    {
        yield return new WaitForSeconds(meleeCoolDown);
        meleeCooldown = false;

    }
    
    
    public IEnumerator MeleeCharge()
    {
        yield return new WaitForSeconds(2);
        CmdGroundSlam();
    }
    
    [Command]
    public void CmdSpawnDamager()
    {
        damage = Instantiate(damageZone, transform.position, Quaternion.identity);
        
        slamDamager = damage.GetComponent<AIDamager>();
        slamDamager.owner = this.gameObject;
        GetComponent<Health>().EventDeath += slamDamager.Delete;
        damage.transform.localScale = Vector3.one * slamDamager.damageRadius * 2;
        NetworkServer.Spawn(damage);
        StartCoroutine(MeleeCharge());
        //RpcRunCharge();
    }
    

    IEnumerator RangedWait()
    {
        yield return new WaitForSeconds(0.5f);
        CmdFire();
        navmesh.isStopped = false;
    }

}
