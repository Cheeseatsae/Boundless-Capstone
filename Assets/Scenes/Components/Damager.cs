using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Damager : NetworkBehaviour
{
    
    public int damage;
    public bool useCollider = true;
    public bool destroyOnDamage = true;

    public delegate void HitEvent();
    public event HitEvent OnHitEvent;
    
    //effects
    

    private void OnTriggerEnter(Collider other)
    {
        if (!useCollider) return;
        if (other.isTrigger) return;
        
        OnHitEvent?.Invoke();
        if (!other.GetComponent<Health>() || !other.GetComponent<AIBaseModel>()) return;
        
        Health healthComp = other.GetComponent<Health>();
        if (isServer)
        {
            healthComp.CmdDoDamage(damage);
        }

        if (!destroyOnDamage) return;
        NetworkServer.Destroy(this.gameObject);
        Destroy(this.gameObject);
        
    }
    
    [ClientRpc]
    public void RpcSetDamage(int d)
    {
        damage = d;
    }

    public void DoDamage(GameObject other)
    {
        if (!other.GetComponent<Health>() || !other.GetComponent<AIBaseModel>()) return;

        Health healthComp = other.GetComponent<Health>();
        if (isServer)
        {
            healthComp.CmdDoDamage(damage);
        }
        
        if (!destroyOnDamage) return;
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }
    
}
