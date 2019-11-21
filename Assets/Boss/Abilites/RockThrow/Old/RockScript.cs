using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{

    public float speed;
    private float startTime;

    public float distance;

    public float distCovered;
    public float frac;

    public float PosX;
    public float PosZ;
    public void Start()
    {
        PosX = transform.position.x;
        PosZ = transform.position.z;
        startTime = Time.time;
        distance = Vector3.Distance(gameObject.transform.position, target.transform.position);

    }

    public void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, target.transform.position);
        distCovered = (Time.time - startTime) * speed;
        frac = distCovered / distance;
    }

    public GameObject target;
    private void FixedUpdate()
    {
        PosX = Mathf.Lerp(PosX, target.transform.position.x, 0.05f);
        PosZ = Mathf.Lerp(PosZ, target.transform.position.z, 0.05f);
        //transform.position = new Vector3(PosX, transform.position.y, PosZ);
        
    }
}
