using System;
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
        // BASE START
        
        if (LogFilter.Debug) Debug.Log("NetworkManager.OnServerAddPlayer");

        if (playerPrefab == null)
        {
            Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object.");
            return;
        }

        if (playerPrefab.GetComponent<NetworkIdentity>() == null)
        {
            Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab.");
            return;
        }

        if (conn.playerController != null)
        {
            Debug.LogError("There is already a player for this connections.");
            return;
        }

        Transform startPos = GetStartPosition();
        GameObject player = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        NetworkServer.AddPlayerForConnection(conn, player);
        
        // BASE END
        
        aiManager.Players.Add(conn.playerController.gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
        
    public override void OnServerRemovePlayer(NetworkConnection conn, NetworkIdentity player)
    {
        // BASE START
        if (player.gameObject != null)
        {
            NetworkServer.Destroy(player.gameObject);
        }
        // BASE END
    }

    // on player spawn generate prefabs per player where necessary 
    // 
    // have items that require create instance of spawnable prefab and update it, then replace the old one
    // will need to pass player, ability, new prefab/added component 
    
}
