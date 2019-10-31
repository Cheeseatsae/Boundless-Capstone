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
    private GameObject follower2;
    private GameObject follower3;
    public Vector3 targetDir;
    public Vector3 targetDir2;
    public Vector3 targetDir3;
    public Rigidbody rb1;
    public Rigidbody rb2;
    public Rigidbody rb3;
    public float followSpeed;
    public float waitTime;
    public float betweenCast;


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
                if (follower != null)
                {
                    targetDir = (model.target.transform.position - follower.transform.position).normalized;
                    rb1.velocity = targetDir * followSpeed;
                    follower.GetComponent<FollowerMerge>().isLeader = true;
                }
                
                if (follower2 != null)
                {
                    targetDir2 = (model.target.transform.position - follower2.transform.position).normalized;
                    rb2.velocity = targetDir2 * followSpeed;
                }

                if (follower3 != null)
                {
                    targetDir3 = (model.target.transform.position - follower3.transform.position).normalized;
                    rb3.velocity = targetDir3 * followSpeed;
                }
            }
            

           
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
        rb1 = follower.GetComponent<Rigidbody>();
        positionAtCast = model.target.transform.position;
        yield return new WaitForSeconds(betweenCast);
        follower2 = Instantiate(followFountain, positionAtCast,Quaternion.identity);
        rb2 = follower2.GetComponent<Rigidbody>();
        positionAtCast = model.target.transform.position;
        yield return new WaitForSeconds(betweenCast);
        follower3 = Instantiate(followFountain, positionAtCast,Quaternion.identity);
        rb3 = follower3.GetComponent<Rigidbody>();
        StartCoroutine(Casting());
    }
    
    
    
}
