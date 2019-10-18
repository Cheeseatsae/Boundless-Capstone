using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFountain : Boss_Ability_Base
{
    public Vector3 positionAtCast;
    public GameObject fountainSpawn;
    public Transform fountainSpawnPoint;
    public GameObject followFountain;
    private GameObject follower;
    public Vector3 targetDir;
    public Rigidbody rb;
    public float followSpeed;
    public float waitTime;


    public override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (isCasting)
        {
            if (model.target != null)
            {
                targetDir = (model.target.transform.position - follower.transform.position).normalized;
            }
            
            rb.velocity = targetDir * followSpeed;
        }
    }

    public override void Cast()
    {
        positionAtCast = model.target.transform.position;
        //Instantiate(fountainSpawn, fountainSpawnPoint);
        StartCoroutine(BeamDelay());
        base.Cast();
    }




    public IEnumerator BeamDelay()
    {
        yield return new WaitForSeconds(waitTime);
        follower = Instantiate(followFountain, positionAtCast,Quaternion.identity);
        rb = follower.GetComponent<Rigidbody>();
        StartCoroutine(Casting());
    }
    
    
}
