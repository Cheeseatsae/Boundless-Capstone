using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AirAiModel : AIBaseModel
{
    //Movement
    public float speed;
    public float minGroundDistance;
    public float maxGroundDistance;
    public Vector3 targetDir;
    public float distance;
    public Vector3 direction;
    
    //Avoidance
    public List<GameObject> NearMe = new List<GameObject>();
    public float toClose;

    public float rayDist;
    //Abilities
    public GameObject projectilePref;
    public Transform bulletPos1;
    
    public Transform bulletPos2;

    public Transform currentSpawnPos;
    public float minTargetRange;

    public float projSpeed;

    public bool onCd = false;

    public float flakCooldown;

    public Vector3 targetDirection;

    public Health health;
    
    //Dodge
    public bool dodgeCD;
    public Vector3 dodgeDirection;
    public float dodgeDistance;
    public float dodgeSelect;

    private ColliderScript myCols;

    public ParticleSystem particleSystem;

    public ParticleSystem deathParticleSystem;
    
    private void Awake()
    {
        health = GetComponent<Health>();
        health.OnHealthChange += Dodge;

        myCols = GetComponentInChildren<ColliderScript>();
        
        // I converted the trigger functions on this script to use the collider script
        // but it was never being used in the first place so
        
//        myCols.EnterTrigger += AddObjectNearMe;
//        myCols.ExitTrigger += RemoveObjectNearMe;
    }

    private void OnDestroy()
    {
//        myCols.EnterTrigger -= AddObjectNearMe;
//        myCols.ExitTrigger -= RemoveObjectNearMe;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        
        //Movement
        RaycastHit hit;
        
        if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, rayDist ))
        {
            Debug.DrawLine(gameObject.transform.position, hit.point, Color.red);
            distance = Vector3.Distance(transform.position, hit.point);
            //To close to ground
            if (distance >= 100)
            {
                direction = new Vector3(0,0,0);
            }
            else if(distance <= minGroundDistance)
            {
                //Debug.Log("moveup");
                direction = new Vector3(0,1,0);
                //rb.velocity = -upDir * speed;
            }
            else if (distance >= maxGroundDistance)
            {
                //Debug.Log("movedown");
                direction = new Vector3(0,-1,0);
                //rb.velocity = downDir * speed;
            }

             
        }
        
        if (target != null)
        {
            targetDir = (target.transform.position - transform.position).normalized;
            targetDir.y = targetDir.y + direction.y;
        }
        
        //Get Target Direction and look at rotation
        rb.velocity = (targetDir + dodgeDirection) * speed;
        dodgeDirection = Vector3.Lerp(dodgeDirection, Vector3.zero, Time.deltaTime);
        
        if (target != null)
        {
            
            targetDirection = (target.transform.position - transform.position).normalized;
        }

        if (NearMe.Count != 0)
        {
            Avoidance();
        }
        
        
        //Abilities
        
        if (distance < minTargetRange && onCd == false)
        {
            onCd = true;
            StartCoroutine(FlakAttack());
            StartCoroutine(FlakCooldown()); 
        }
    }
    
    //Calculate Movement Deviation
    public Vector3 RandomVector(float min, float max)
    {
        float x = Random.Range(min, max);
        float y = Random.Range(min, max);
        float z = Random.Range(min, max);
        return new Vector3(x,y,z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AirAiModel>())
        {
            NearMe.Add(other.gameObject);
        }
    }

    private void AddObjectNearMe(Collider other)
    {
        if (other.GetComponent<AirAiModel>())
        {
            NearMe.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (NearMe.Contains(other.gameObject))
        {
            NearMe.Remove(other.gameObject);
        }
    }
    
    private void RemoveObjectNearMe(Collider other)
    {
        if (NearMe.Contains(other.gameObject))
        {
            NearMe.Remove(other.gameObject);
        }
    }

    public void Avoidance()
    {
        foreach (GameObject ai in NearMe)
        {
            if (ai == null) return;
            float dist = Vector3.Distance(transform.position, ai.transform.position);
            if (dist <= toClose)
            {
                Dodge();
            }
        }
    }
    
    public void FlakCannon(int cannon)
    {
        
        if (cannon == 1)
        {
            currentSpawnPos = bulletPos1;
        } else if (cannon == 2)
        {
            currentSpawnPos = bulletPos2;
        }

        GameObject flak = Instantiate(projectilePref, currentSpawnPos.position, Quaternion.identity);
        Rigidbody flakRb = flak.GetComponent<Rigidbody>();
        flakRb.velocity = targetDirection * projSpeed;

    }

    public IEnumerator FlakAttack()
    {
        FlakCannon(1);
        particleSystem.Play();
        yield return new WaitForSeconds(0.5f);
        FlakCannon(2);
        particleSystem.Play();
        

    }

    public IEnumerator FlakCooldown()
    {
        yield return new WaitForSeconds(flakCooldown);
        onCd = false;
    }

    public void Dodge()
    {
        dodgeSelect = Random.Range(1f, 50f);
        Vector3 verticalVariation = Vector3.up * Random.Range(-0.4f, 0.8f);
            
        if (dodgeSelect < 25)
        {
            dodgeDirection = (transform.right + verticalVariation) * dodgeDistance;
        }
        else 
        {
            dodgeDirection = (-transform.right + verticalVariation) * dodgeDistance;
        }
    }


}
