using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AirAiModel : AIBaseModel
{
    public GameObject target;
    private float minDistance = Mathf.Infinity;
    public AIManager aiManager;
    // Start is called before the first frame update
    void Start()
    {
        aiManager = FindObjectOfType<AIManager>();
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!isServer) return;
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
    }
}
