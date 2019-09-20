using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float cameraSpeed = 120f;

    public GameObject followObj;
    
    public float clamp = 80;

    public float sensitivity = 150;

    public Vector3 followOffset;

    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float rotY;
    public float rotX;

    public bool invertX;
    public bool invertY;

    public static CameraControl playerCam;

    private float step;
    
    private void Awake()
    {
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        playerCam = this;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotX = rotation.x;
        rotY = rotation.y;
        transform.position = Vector3.Lerp(transform.position, followObj.transform.position, 0.9f);
        step = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!followObj) return;
            
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = mouseX * (invertX ? -1 : 1);
        finalInputZ = mouseY * (invertY ? -1 : 1);

        rotY += finalInputX * sensitivity * Time.deltaTime;
        rotX += finalInputZ * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clamp, clamp);

        Quaternion localRot = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRot;

        Quaternion r = followObj.transform.rotation;
        followObj.transform.rotation = Quaternion.Euler(r.x, rotY, r.z);
        
    }

    private void FixedUpdate()
    {
        if (!followObj) return;
        
        UpdatePos();
    }

    private void UpdatePos()
    {
        Vector3 target = followObj.transform.position + followOffset;
        float dis = Vector3.Distance(target, transform.position);
        
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        step = cameraSpeed * Time.deltaTime;
    }
    
}
