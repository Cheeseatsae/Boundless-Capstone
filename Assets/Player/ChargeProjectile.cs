using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ChargeProjectile : MonoBehaviour
{

    public GameObject explosion;
    private float lifetime;
    [HideInInspector] public float explosionRadius;
    [HideInInspector] public int damage;
    [HideInInspector] public bool fired = false;

    private void Awake()
    {
        fired = false;
    }

    private void Start()
    {
        lifetime = explosion.GetComponent<ParticleSystem>().main.startLifetime.constant;
    }
    
    public void Fire(float range)
    {
        fired = true;
        Destroy(gameObject, range);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerModel>()) return;
        if (other.isTrigger) return;
        
        if (fired) Explode();
    }

    private void Explode()
    {
        GameObject e = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(e, lifetime);

        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider col in cols)
        {
            if (col.GetComponent<PlayerModel>()) continue;

            Health h = col.GetComponent<Health>();
            if (h != null) h.DoDamage(damage);
        }
        Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }
}
