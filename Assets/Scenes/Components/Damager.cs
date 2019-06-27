using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Damager : NetworkBehaviour
{
    public GameObject projectile;
    public int damage;
    
    //effects
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() && other.GetComponent<PlayerModel>())
        {
            Health healthComp = other.GetComponent<Health>();
            healthComp.CmdDoDamage(damage);
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
        }
        
        
    }
}
