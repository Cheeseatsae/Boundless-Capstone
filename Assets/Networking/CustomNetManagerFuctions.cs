using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CustomNetManagerFuctions : NetworkManager
{

    public AIManager aiManager;
    
    // Start is called before the first frame update


    public void Awake()
    {
        aiManager = FindObjectOfType<AIManager>();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, AddPlayerMessage extraMessage)
    {
        base.OnServerAddPlayer(conn, extraMessage);
        aiManager.Players.Add(conn.playerController.gameObject);
    }
}
