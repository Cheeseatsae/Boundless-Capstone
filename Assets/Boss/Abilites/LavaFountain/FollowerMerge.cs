using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMerge : MonoBehaviour
{
    
    public bool isLeader = false;
    // Start is called before the first frame update
    public Vector3 Scale;
    public float xScale;
    public float yScale;
    public float zScale;
    public float xMax = 7;
    public float yMax = 20;
    public float zMax = 7;
    public float damageTimer;
    public int amountOfTicks;
    public int damagePerTick;
    public SciFiBeamStatic beam;
    public void Start()
    {
        if (isLeader)
        {
            xScale = transform.localScale.x;
            yScale = transform.localScale.y;
            zScale = transform.localScale.z;
            Scale = new Vector3(xScale,yScale,zScale);
        }
    }

    private void Update()
    {
        if (isLeader)
        {
            transform.localScale = Scale;

            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isLeader)
        {
            if (other.gameObject.GetComponent<FollowerMerge>())
            {
                other.gameObject.transform.localScale = Vector3.Lerp(other.gameObject.transform.localScale,
                    Vector3.zero, (Time.deltaTime * 6));
                if (other.gameObject.transform.localScale.x < 1f && other.gameObject.transform.localScale.z < 1f)
                {
                    Destroy(other.gameObject);
                }
                
                xScale = Mathf.Lerp(xScale, xMax, Time.deltaTime * 5);
                zScale = Mathf.Lerp(zScale, zMax, Time.deltaTime * 5);
                Scale.x = xScale;
                Scale.z = zScale;
            }
        }




    }

    private Coroutine co;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerModel>())
        {
            co = StartCoroutine(Damage(other));
        }
        
    }
    
    public IEnumerator Damage(Collider player)
    {
        {
            for (int i = 0; i < amountOfTicks; i++)
            {
                Health h = player.GetComponent<Health>();
                if (h != null) h.DoDamage(damagePerTick);

                yield return new WaitForSeconds(damageTimer);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerModel>())
        {
            StopCoroutine(co);
        }
    }
}

