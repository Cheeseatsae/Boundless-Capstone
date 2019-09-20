using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGroundBullet : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<PlayerModel>())
        {
            Health healthComp = other.GetComponent<Health>();
            healthComp.DoDamage(damage);
            Destroy(this.gameObject);
            
        } 
        else if (other.gameObject.layer == 10)
        {
            Destroy(this.gameObject);
        }

    }
}