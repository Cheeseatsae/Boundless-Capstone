using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFountain : Boss_Ability_Base
{
    public Vector3 positionAtCast;
    public Vector3 positionAtCast1;
    public Vector3 positionAtCast2;
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
    private Rigidbody targetRb;


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
                    rb1.velocity = targetDir * (followSpeed * 2.5f);
                    //follower.transform.position = Vector3.MoveTowards(follower.transform.position, model.target.transform.position, (followSpeed * Time.deltaTime) * 1.5f);
                    
                }
                
                if (follower2 != null)
                {
                    targetDir2 = (model.target.transform.position - follower2.transform.position).normalized;
                    rb2.velocity = targetDir2 * followSpeed;
                    //follower2.transform.position = Vector3.MoveTowards(follower2.transform.position, model.target.transform.position, (followSpeed * Time.deltaTime));
                }

                if (follower3 != null)
                {
                    targetDir3 = (model.target.transform.position - follower3.transform.position).normalized;
                    rb3.velocity = targetDir3 * followSpeed;
                    //follower3.transform.position = Vector3.MoveTowards(follower3.transform.position, model.target.transform.position, (followSpeed * Time.deltaTime));
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
        Vector3 pos = model.target.transform.position;
        targetRb = model.target.gameObject.GetComponent<Rigidbody>();
        yield return new WaitForSeconds(waitTime);
        follower = Instantiate(followFountain, positionAtCast,Quaternion.identity);
        follower.GetComponent<FollowerMerge>().isLeader = true;
        rb1 = follower.GetComponent<Rigidbody>();
        positionAtCast1 = model.target.transform.position + targetRb.velocity;
        //yield return new WaitForSeconds(betweenCast);
        follower2 = Instantiate(followFountain, positionAtCast1,Quaternion.identity);
        rb2 = follower2.GetComponent<Rigidbody>();
        positionAtCast2 = new Vector3(pos.x - 10f, pos.y,
            pos.z);
        //yield return new WaitForSeconds(betweenCast);
        follower3 = Instantiate(followFountain, positionAtCast2,Quaternion.identity);
        rb3 = follower3.GetComponent<Rigidbody>();
        StartCoroutine(Casting());
    }

    public override void FinishCast()
    {
        base.FinishCast();
        Destroy(follower);
        Destroy(follower2);
        Destroy(follower3);

    }
}
