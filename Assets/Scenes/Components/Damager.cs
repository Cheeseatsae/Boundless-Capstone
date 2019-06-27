using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Damager : NetworkBehaviour
{
    
    public int damage;
    
    //effects


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() && other.GetComponent<AIBaseModel>())
        {
            Health healthComp = other.GetComponent<Health>();
            healthComp.CmdDoDamage(damage);
            NetworkServer.Destroy(this.gameObject);
            Destroy(this.gameObject);
        }
        
        
    }
}
