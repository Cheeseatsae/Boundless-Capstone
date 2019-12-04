using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public bool suction = false;
    private float distance = 1f;
    public GameObject suctionPoint;
    private CameraCollision camCol;
    public 
    // Start is called before the first frame update
    void Start()
    {
        camCol = CameraControl.playerCam.gameObject.GetComponentInChildren<CameraCollision>();
    }

    // Update is called once per frame
    void Update()
    {
        if (suction)
        {
            CameraControl.playerCam.followObj = suctionPoint;
            CameraControl.playerCam.gameObject.transform.position = suctionPoint.transform.position + suctionPoint.transform.TransformDirection(Vector3.forward) * distance;
            camCol.onPlayer = false;
            camCol.maxDist = Mathf.Lerp(camCol.maxDist, 1f, Time.deltaTime);
            camCol.distance = camCol.maxDist;
            if (camCol.distance <= 2f)
            {
                SceneManager.LoadScene(2);
            }
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
