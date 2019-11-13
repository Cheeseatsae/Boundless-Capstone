using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GroundAI_Model : AIBaseModel
{
    public GameObject damageZone;

    public Animator anim;
    
    //Ground Slam Variables
    public bool melee = false;
    public bool meleeCooldown = false;
    public float targetDistance;
    public float meleeCoolDown;
    public AIDamager slamDamager;
    public GameObject damage;
    
    //Ranged Ability Variables
    public bool ranged = false;
    public float rangedCoolDown;
    public GameObject bulletPref;
    public bool rangedCooldown = false;
    public float projectileSpeed;
    public bool cantFire = false;

    public float chargeDuration = 2;
    
    public NavMeshAgent navmesh;
    // Start is called before the first frame update

    private void Awake()
    {
        navmesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (target != null)
        {
            navmesh.destination = target.transform.position;
            
            RaycastHit hit;
        
            if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit))
            {
            
                if (hit.collider.gameObject == target)
                {
                    if (rangedCooldown == false && melee == false)
                    {
                        ranged = true;
                        navmesh.isStopped = true;
                        navmesh.velocity = new Vector3(0,0,0);
                        //StartCoroutine(RangedWait());
                        anim.SetBool("CanRanged", true);
                        anim.SetTrigger("Ranged");
                        rangedCooldown = true;
                        cantFire = false;
                        StartCoroutine(RangedCooldown());
                        
                    }               
                }
            }

            targetDistance = Vector3.Distance(transform.position, target.transform.position);
            if (targetDistance < 5)
            {
                if (meleeCooldown == false && ranged == false)
                {
                    melee = true;
                    cantFire = true;
                    navmesh.isStopped = true;
                    navmesh.velocity = new Vector3(0,0,0);
                    meleeCooldown = true;
                    SpawnDamager();
                    
                    StartCoroutine(MeleeCooldown());
                }
                
            }
        }
        
    }

    
    

    public void Fire()
    {
        if (target == null) return;

        GameObject bullet = Instantiate(bulletPref, transform.position + transform.forward, Quaternion.identity);
        
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        Vector3 dir = (target.transform.position - transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;
        
    }
    
    public void GroundSlam()
    {
        
        slamDamager.SlamDamage();
        navmesh.isStopped = false;
        anim.SetTrigger("BackToMovement");
        StartCoroutine(CanRanged());
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
        anim.SetTrigger("GroundSlam");
        yield return new WaitForSeconds(chargeDuration);
        GroundSlam();
        melee = false;
    }
    

    public void SpawnDamager()
    {
        damage = Instantiate(damageZone, transform.position, Quaternion.identity);
        
        slamDamager = damage.GetComponent<AIDamager>();
        slamDamager.owner = this.gameObject;
        GetComponent<Health>().EventDeath += slamDamager.Delete;
        damage.transform.localScale = Vector3.one * slamDamager.damageRadius * 2;
        StartCoroutine(MeleeCharge());
        //RpcRunCharge();
    }
    

    IEnumerator RangedWait()
    {
        yield return new WaitForSeconds(0f);
        anim.SetBool("CanRanged", true);
        anim.SetTrigger("Ranged");
        //Fire();

    }

    public IEnumerator CanRanged()
    {
        yield return new WaitForSeconds(1);
        melee = false;
        
    }

}
