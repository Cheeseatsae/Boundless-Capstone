using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Damager : MonoBehaviour
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

        healthComp.DoDamage(damage);
        if (!destroyOnDamage) return;
        Destroy(this.gameObject);
        
    }
    

    public void SetDamage(int d)
    {
        damage = d;
    }

    public void DoDamage(GameObject other)
    {
        if (!other.GetComponent<Health>() || !other.GetComponent<AIBaseModel>()) return;

        Health healthComp = other.GetComponent<Health>();
        healthComp.DoDamage(damage);
        
        if (!destroyOnDamage) return;
        Destroy(gameObject);
    }
    
}
