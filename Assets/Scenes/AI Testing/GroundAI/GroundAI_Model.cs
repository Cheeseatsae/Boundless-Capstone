using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundAI_Model : MonoBehaviour
{

    public GameObject target;
    private float minDistance = Mathf.Infinity;
    private List<GameObject> Players = new List<GameObject>();

    private NavMeshAgent navmesh;
    // Start is called before the first frame update

    private void Awake()
    {
        navmesh = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Players.Add(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject player in Players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < minDistance)
            {
                target = player;
                minDistance = distance;
            }
        }
        navmesh.destination = target.transform.position;
    }
}
