using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMerge : MonoBehaviour
{

    public bool isLeader = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isLeader)
        {
            if (other.gameObject.GetComponent<FollowerMerge>())
            {
                Destroy(other.gameObject);
                transform.localScale = transform.localScale * 2;

            }
        }

    }
}
