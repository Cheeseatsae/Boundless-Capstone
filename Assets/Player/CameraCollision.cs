using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraCollision : MonoBehaviour
{

    public float minDist;

    public float maxDist;

    public float smoothing;

    public Vector3 direction;

    public float distance;

    public Camera cam;
    
    
    // Start is called before the first frame update
    void Start()
    {
        direction = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPos = transform.parent.TransformPoint(direction * maxDist);
        RaycastHit hit;
        if (Physics.Linecast(transform.parent.position, desiredPos, out hit))
        {
            distance = Mathf.Clamp((hit.distance * 0.9f), minDist, maxDist);

        }
        else
        {
            distance = maxDist;
        }

        transform.localPosition =
            Vector3.Lerp(transform.localPosition, direction * distance, Time.deltaTime * smoothing);

        float c = cam.nearClipPlane;
        Transform camPos = cam.transform;
        bool clipping = true;

        while (clipping)
        {
            Vector3 pos1 = cam.ViewportToWorldPoint(new Vector3(0, 0, c));
            Vector3 pos2 = cam.ViewportToWorldPoint(new Vector3(0.5f, 0, c));
            Vector3 pos3 = cam.ViewportToWorldPoint(new Vector3(1, 0, c));
            Vector3 pos4 = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, c));
            Vector3 pos5 = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, c));
            Vector3 pos6 = cam.ViewportToWorldPoint(new Vector3(0, 1, c));
            Vector3 pos7 = cam.ViewportToWorldPoint(new Vector3(0.5f, 1, c));
            Vector3 pos8 = cam.ViewportToWorldPoint(new Vector3(1, 1, c));
        
        
            Debug.DrawLine(camPos.position, pos1, Color.red);
            Debug.DrawLine(camPos.position, pos2, Color.red);
            Debug.DrawLine(camPos.position, pos3, Color.red);
            Debug.DrawLine(camPos.position, pos4, Color.red);
            Debug.DrawLine(camPos.position, pos5, Color.red);
            Debug.DrawLine(camPos.position, pos6, Color.red);
            Debug.DrawLine(camPos.position, pos7, Color.red);
            Debug.DrawLine(camPos.position, pos8, Color.red);

            if (Physics.Linecast(camPos.position, pos1, out hit))
            {
                
            } else if (Physics.Linecast(camPos.position, pos2, out hit))
            {
                
            }else if (Physics.Linecast(camPos.position, pos3, out hit))
            {
                
            }else if (Physics.Linecast(camPos.position, pos4, out hit))
            {
                
            }else if (Physics.Linecast(camPos.position, pos5, out hit))
            {
                
            }else if (Physics.Linecast(camPos.position, pos6, out hit))
            {
                
            }else if (Physics.Linecast(camPos.position, pos7, out hit))
            {
                
            }else if (Physics.Linecast(camPos.position, pos8, out hit))
            {
                
            }
            else
            {
                clipping = false;
            }

            if (clipping)
            {
                transform.localPosition += camPos.forward * 0.3f;
            }
        }


    }
}
