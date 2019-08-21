using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AIAirBullet : NetworkBehaviour
{
    public int damage = 20;
    
    public GameObject damageZone;
    public AIDamager aiDamager;
    public GameObject damager;

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        
        if (other.GetComponent<PlayerModel>())
        {   
            damager = Instantiate(damageZone, gameObject.transform.position, Quaternion.identity);
            aiDamager = damager.GetComponent<AIDamager>();
            NetworkServer.Spawn(damager);
            CmdExplosion();
            
            Health healthComp = other.GetComponent<Health>();
            healthComp.CmdDoDamage(damage);
            
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
            
        } 
        else if (other.gameObject.layer == 10)
        {
            if (gameObject.CompareTag("AirAi"))
            {
                damager = Instantiate(damageZone, gameObject.transform.position, Quaternion.identity);
                aiDamager = damager.GetComponent<AIDamager>();
                NetworkServer.Spawn(damager);
                CmdExplosion();
            }
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
        }

    }

    
    [Command]
    public void CmdExplosion()
    {
        aiDamager.ExplosionDamage();
    }


}
