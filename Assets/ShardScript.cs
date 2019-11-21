using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardScript : MonoBehaviour
{
    public LayerMask layer;
    public bool hit = false;
    public GameObject player;
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision other)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = true;


    }

    private void Start()
    {
        transform.LookAt(player.transform);
    }


}
