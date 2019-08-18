using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AIBaseModel : NetworkBehaviour
{
    
    public GameObject target;
    private float maxDistance = Mathf.Infinity;
    public Rigidbody rb;
    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!isServer) return;
        Targeting();
    }

    private void ReceiveNewTarget()
    {
        
    }
    
    private void Targeting()
    {
        
        GameObject newTarget = gameObject;
        maxDistance = Mathf.Infinity;
        foreach (GameObject player in CustomLobbyManager.players)
        {
            
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < maxDistance)
            {
                newTarget = player;
                maxDistance = distance;
            }
        }

        target = newTarget;
    }
}