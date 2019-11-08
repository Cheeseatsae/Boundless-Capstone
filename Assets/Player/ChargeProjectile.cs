using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeProjectile : MonoBehaviour
{

    public GameObject explosion;
    private float particleLifetime;
    [HideInInspector] public float explosionRadius;
    [HideInInspector] public int damage;
    [HideInInspector] public bool fired = false;

    private void Awake()
    {
        fired = false;
    }

    private void Start()
    {
        particleLifetime = explosion.GetComponent<ParticleSystem>().main.startLifetime.constant;
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
        Destroy(e, particleLifetime);

        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider c in cols)
        {
            // skipping player
            if (c.GetComponent<PlayerModel>()) continue;

            Health h = c.GetComponent<Health>();
            if (h != null)
            {
                h.DoDamage(damage);
                PlayerEvents.CallPlayerDamageEvent(h.gameObject, damage, c.ClosestPointOnBounds(transform.position));
            }
        }
        Destroy(gameObject);
    }
}
