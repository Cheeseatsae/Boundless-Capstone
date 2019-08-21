using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AIBullet : NetworkBehaviour
{
    public int damage = 20;

    public GameObject damageZone;
    public AIDamager aiDamager;
    public GameObject damager;

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        
        if (other.GetComponent<Health>() && other.GetComponent<PlayerModel>())
        {
            Health healthComp = other.GetComponent<Health>();
            healthComp.CmdDoDamage(damage);
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
            
        }
        if (other.gameObject.layer == 10 || other.gameObject.GetComponent<PlayerModel>())
        {
            if (gameObject.CompareTag("AirAi"))
            {
                damager = Instantiate(damageZone, gameObject.transform.position, Quaternion.identity);
                NetworkServer.Spawn(damager);
                aiDamager = damager.GetComponent<AIDamager>();
                CmdExplosion();
            }
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Health>() && other.gameObject.GetComponent<PlayerModel>())
        {
            Health healthComp = other.gameObject.GetComponent<Health>();
            healthComp.CmdDoDamage(damage);

            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
            
        }

        if (other.gameObject.layer == 10 || other.gameObject.GetComponent<PlayerModel>())
        {
            if (gameObject.CompareTag("AirAi"))
            {
                damager = Instantiate(damageZone, gameObject.transform.position, Quaternion.identity);
                NetworkServer.Spawn(damager);
                aiDamager = damager.GetComponent<AIDamager>();
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
