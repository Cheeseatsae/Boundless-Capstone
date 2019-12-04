using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public bool suction = false;
    private float distance = 1f;
    public GameObject suctionPoint;
    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (suction)
        {
            CameraControl.playerCam.followObj = suctionPoint;
            CameraControl.playerCam.gameObject.transform.position = suctionPoint.transform.position + suctionPoint.transform.TransformDirection(Vector3.forward) * distance;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerModel>())
        {
            suction = true;
        }
    }
}
