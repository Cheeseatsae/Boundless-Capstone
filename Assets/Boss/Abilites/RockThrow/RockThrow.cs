using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockThrow : Boss_Ability_Base
{
    // Start is called before the first frame update

    public GameObject spawnPoint;
    public float maxHeight = 25f;
    public float gravity = -15f;
    public GameObject rock;
    Vector3 CalculateLaunchVelocity()
    {
        Vector3 origin = spawnPoint.transform.position;
        target = model.target;
        Vector3 endPoint = target.transform.position;
        //distance = Vector3.Distance(origin, endPoint);
        float displacementY = endPoint.y - origin.y;
        Vector3 displacementXY = new Vector3(endPoint.x - origin.x, 0f, endPoint.z - origin.z);
        float time = Mathf.Sqrt(-2 * maxHeight / gravity) +
                      Mathf.Sqrt(2 * (displacementY - maxHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * maxHeight);
        Vector3 velocityXZ = displacementXY  / time;
        return velocityXZ + velocityY;
    }

    public void LaunchRock()
    {
        GameObject newRock = Instantiate(rock, spawnPoint.transform.position, Quaternion.identity);
        Rigidbody rb = newRock.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.velocity = CalculateLaunchVelocity();
        print(CalculateLaunchVelocity());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LaunchRock();
        }
    }
}
