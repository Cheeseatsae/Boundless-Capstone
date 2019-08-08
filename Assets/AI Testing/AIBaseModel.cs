using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AIBaseModel : NetworkBehaviour
{
    
    public GameObject target;
    private float maxDistance = Mathf.Infinity;
    public AIManager aiManager;
    public Rigidbody rb;
    // Start is called before the first frame update
    public virtual void Start()
    {
        aiManager = FindObjectOfType<AIManager>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!isServer) return;
        Targetting();
    }
    
    public void Targetting()
    {
        GameObject newTarget = this.gameObject;
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
