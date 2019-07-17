﻿using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AIDamager : MonoBehaviour
{
    public List<GameObject> PlayerList = new List<GameObject>();

    public GameObject owner;
    
    public Vector3 knockupDirection;

    public int slamDamage;
    // Start is called before the first frame update
    void Start()
    {
        knockupDirection = new Vector3(0,0.5f,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerModel>())
        {
            PlayerList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlayerList.Contains(other.gameObject))
        {
            PlayerList.Remove(other.gameObject);
        }
    }

    [Command]
    public void CmdSlamDamage()
    {
        foreach (GameObject player in PlayerList)
        {
            Rigidbody targetRb = player.GetComponent<Rigidbody>();
            Vector3 dir = (player.transform.position - owner.transform.position) * 3;
            dir.y = 0;
            dir = dir.normalized;
            dir.y = knockupDirection.y;
        
            targetRb.velocity =  dir *15;
            Health health = player.GetComponent<Health>();
            health.CmdDoDamage(slamDamage);
        }
    }
    
    
    
}
