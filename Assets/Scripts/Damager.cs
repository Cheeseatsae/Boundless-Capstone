using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Damager : MonoBehaviour
{
    
    public int damage;
    public bool destroyOnDamage = true;

    public delegate void HitEvent();
    public event HitEvent OnHitEvent;
    
    //effects
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.isTrigger) return;
        if (other.gameObject.GetComponentInParent<PlayerModel>()) return;
        
        OnHitEvent?.Invoke();
        
        DoDamage(other);
    }
    
    public void SetDamage(int d)
    {
        damage = d;
    }

    private void DoDamage(Collision other)
    {
        if (!other.gameObject.GetComponentInParent<Health>())
        {
            //Debug.Log("No health");
            Destroy(this.gameObject);
            return;
        }

        Health healthComp = other.gameObject.GetComponentInParent<Health>();
        //Debug.Log(other.gameObject.name + healthComp.health);
        healthComp.DoDamage(damage);
        PlayerEvents.CallPlayerDamageEvent(other.gameObject, damage, other.contacts[0].point);
        
        if (!destroyOnDamage) return;
        Destroy(this.gameObject);
    }
    
}
