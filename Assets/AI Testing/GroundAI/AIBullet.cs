﻿using System;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() && other.GetComponent<PlayerModel>())
        {
            Health healthComp = other.GetComponent<Health>();
            healthComp.CmdDoDamage(damage);
            if (gameObject.CompareTag("AirAi"))
            {
                damager = Instantiate(damageZone, this.gameObject.transform.position, Quaternion.identity);
                NetworkServer.Spawn(damager);
                
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

    [Command]
    public void CmdExplosion()
    {
        aiDamager.ExplosionDamage();
    }


}
