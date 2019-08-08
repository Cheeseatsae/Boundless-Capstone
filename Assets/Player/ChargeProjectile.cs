using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ChargeProjectile : NetworkBehaviour
{

    public GameObject explosion;
    private float lifetime;
    public float explosionRadius;
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

    [ClientRpc]
    public void RpcFire(Vector3 v)
    {
        fired = true;
        Rigidbody r = GetComponent<Rigidbody>();
        if (r != null) r.velocity = v;
        else Destroy(this.gameObject);
        
        Destroy(gameObject, 15f);
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
            if (h != null) h.CmdDoDamage(damage);
        }
        Destroy(gameObject);
        NetworkServer.Destroy(gameObject);
    }
}
