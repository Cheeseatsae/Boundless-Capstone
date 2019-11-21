using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : Boss_Ability_Base
{
    // Start is called before the first frame update

    public GameObject spawnPoint1;
    public GameObject spawnPoint2;
    public GameObject spawnPoint3;
    public float maxHeight = 25f;
    public float gravity = -5f;
    public GameObject rock;

    public Vector3 leftDiviation;
    public Vector3 rightDiviation;
    
    Vector3 CalculateLaunchVelocity(Vector3 target, Vector3 spawn)
    {
        Vector3 origin = spawn;
        
        Vector3 endPoint = target;
        //distance = Vector3.Distance(origin, endPoint);
        float displacementY = endPoint.y - origin.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - origin.x, 0f, endPoint.z - origin.z);
        float time = Mathf.Sqrt(-2 * maxHeight / gravity) +
                      Mathf.Sqrt(2 * (displacementY - maxHeight) / gravity) ;
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * maxHeight);
        Vector3 velocityXZ = displacementXZ / time;
        return velocityXZ + velocityY;
    }

    public void LaunchRock(Vector3 pos, Vector3 spawnPoint)
    {
        GameObject newRock = Instantiate(rock, spawnPoint, Quaternion.identity);
        Rigidbody rb = newRock.GetComponent<Rigidbody>();
        RockExplode rockExplode = newRock.GetComponent<RockExplode>();
        rockExplode.player = model.target;
        
        rb.useGravity = true;
        rb.velocity = CalculateLaunchVelocity(pos, spawnPoint);
        //print(CalculateLaunchVelocity());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RunRockThrow();
        }
    }

    public void RunRockThrow()
    {
        Vector3 position = new Vector3();
        Vector3 spawnPos = new Vector3();
        for (var i = 0; i < 3; i++)
        {
            if (i == 0)
            {
                position = model.target.transform.position;
                spawnPos = spawnPoint1.transform.position;
                
                
                
            }else if (i == 1)
            {
                position = model.target.transform.position + leftDiviation;
                spawnPos = spawnPoint3.transform.position;
            }else if (i == 2)
            {
                position = model.target.transform.position + rightDiviation;
                spawnPos = spawnPoint2.transform.position;
            }
            LaunchRock(position, spawnPos);

        }
    }
}
