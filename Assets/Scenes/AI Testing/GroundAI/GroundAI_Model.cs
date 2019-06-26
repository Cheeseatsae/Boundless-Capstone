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
    public List<GameObject> Players = new List<GameObject>();
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

        RecalPlayerList();        
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach (GameObject player in Players)
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
        }
        
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit))
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
    [Command]
    public void CmdFire()
    {
        
        GameObject bullet = Instantiate(bulletPref, transform.position + transform.forward, Quaternion.identity);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = transform.forward * projectileSpeed;
        NetworkServer.Spawn(bullet);
    }
    
    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(coolDown);
        rangedCooldown = false;

    }

    public void RecalPlayerList()
    {
        Players.Clear();
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Players.Add(player);
        }
    }
}
