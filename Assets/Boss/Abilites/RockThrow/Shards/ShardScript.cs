using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardScript : MonoBehaviour
{
    public LayerMask layer;
    public bool hit = false;
    public GameObject player;
    
    //Damage
    public int damage;
    // Start is called before the first frame update
    public float destroyTimer;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerModel>())
        {
            Health health = other.gameObject.GetComponent<Health>();
            health.DoDamage(damage);
            Destroy(this.gameObject);
        } 
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        StartCoroutine(Destroy());

    }

    private void Start()
    {
        if (player != null)
        {
            transform.LookAt(player.transform);
        }
        
    }

    public IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTimer);
        Destroy(this.gameObject);
    }


}
