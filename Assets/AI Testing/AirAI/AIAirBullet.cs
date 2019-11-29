using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAirBullet : MonoBehaviour
{
    public int damage = 20;
    
    public GameObject damageZone;
    public AIDamager aiDamager;
    public GameObject damager;

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<PlayerModel>())
        {   
            damager = Instantiate(damageZone, gameObject.transform.position, Quaternion.identity);
            aiDamager = damager.GetComponent<AIDamager>();
            Explosion();
            Health healthComp = other.GetComponent<Health>();
            healthComp.DoDamage(damage);
            Destroy(this.gameObject);
            
        } 
        else if (other.gameObject.layer == 10)
        {
            damager = Instantiate(damageZone, gameObject.transform.position, Quaternion.identity);
            aiDamager = damager.GetComponent<AIDamager>();
            Explosion();
            Destroy(this.gameObject);
        }

    }
    
    public void Explosion()
    {
        aiDamager.ExplosionDamage();
    }


}
