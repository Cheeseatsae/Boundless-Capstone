using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public GameObject hitParticle;
    private Damager damager;

    private void Awake()
    {
        damager = GetComponent<Damager>();
        damager.OnHitEvent += SpawnParticle;
    }

    private void OnDestroy()
    {
        damager.OnHitEvent -= SpawnParticle;
    }

    [ClientRpc]
    public void RpcSetVelocity(Vector3 v)
    {
        Rigidbody r = GetComponent<Rigidbody>();
        if (r != null) r.velocity = v;
        else Destroy(this.gameObject);
    }

    void SpawnParticle()
    {
        GameObject g = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(g, 2f);
    }
}
