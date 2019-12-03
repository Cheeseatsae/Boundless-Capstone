using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGroundBullet : MonoBehaviour
{
    public int damage = 20;
    
    public float waitTime;
    public GameObject emission;


    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<PlayerModel>())
        {
            Health healthComp = other.GetComponent<Health>();
            healthComp.DoDamage(damage);
            Instantiate(emission, transform.position, Quaternion.identity);

            Destroy(this.gameObject);

        } 
        else if (other.gameObject.layer == 10)
        {
            Instantiate(emission, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }

    }

    

}