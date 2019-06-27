using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class GroundAI_Model : NetworkBehaviour
{

    public GameObject target;
    private float minDistance = Mathf.Infinity;
    //public List<GameObject> Players = new List<GameObject>();
    public AIManager aiManager;
    public GameObject bulletPref;
    public float coolDown;
    public float projectileSpeed;
    public bool rangedCooldown = false;
    public bool meleeCooldown = false;

    private NavMeshAgent navmesh;
    // Start is called before the first frame update

    private void Awake()
    {
        navmesh = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        aiManager = FindObjectOfType<AIManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach (GameObject player in aiManager.Players)
        {
            if (player != null)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance < minDistance)
                {
                    target = player;
                    minDistance = distance;
                }
            }
            
        }

        if (target != null)
        {
            navmesh.destination = target.transform.position;
            
            RaycastHit hit;
        
            if (Physics.Raycast(transform.position, (target.transform.position - transform.position), out hit))
            {
            
                if (hit.collider.gameObject == target)
                {
                    if (rangedCooldown == false)
                    {
                        CmdFire();
                        rangedCooldown = true;
                        StartCoroutine(Cooldown());
                    }               
                }
            }
        }
        

        
    }
    [Command]
    public void CmdFire()
    {
        
        GameObject bullet = Instantiate(bulletPref, transform.position + transform.forward, Quaternion.identity);
        
        
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        Vector3 dir = (target.transform.position - transform.position).normalized;
        bulletRb.velocity = dir * projectileSpeed;
        NetworkServer.Spawn(bullet);
    }
    
    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(coolDown);
        rangedCooldown = false;

    }


}
