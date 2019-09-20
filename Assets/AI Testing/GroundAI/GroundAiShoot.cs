using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GroundAiShoot : AIAbilityBase
{

    public GameObject bulletPref;
    public GroundAI_Model model;
    public float coolDown;
    public float projectileSpeed;

    private void Awake()
    {
        model = GetComponent<GroundAI_Model>();
    }

    public override void Enter()
    {
        base.Enter();
        
    }


    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPref, transform.position + transform.forward, Quaternion.identity);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * projectileSpeed;
        
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(coolDown);
        Fire();
    }
}
