using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitParticle;
    private Damager damager;
    public Rigidbody rb;

    private void Awake()
    {
        damager = GetComponent<Damager>();
        damager.OnHitEvent += SpawnParticle;
        rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        damager.OnHitEvent -= SpawnParticle;
    }
    
    public void SetVelocity(Vector3 v)
    {
        Rigidbody r = GetComponent<Rigidbody>();
        if (r != null) r.velocity = v;
        else Destroy(this.gameObject);
    }

    void SpawnParticle()
    {
        GameObject g = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(g, 0.1f);
    }

    public void Update()
    {
        transform.LookAt(transform.position + rb.velocity);
    }
}
