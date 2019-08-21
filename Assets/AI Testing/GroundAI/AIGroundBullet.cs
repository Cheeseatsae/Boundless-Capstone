using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AIGroundBullet : NetworkBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        
        if (other.GetComponent<PlayerModel>())
        {
            Health healthComp = other.GetComponent<Health>();
            healthComp.CmdDoDamage(damage);
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
            
        } 
        else if (other.gameObject.layer == 10)
        {
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
        }

    }
}