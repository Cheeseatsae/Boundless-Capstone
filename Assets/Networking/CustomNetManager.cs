using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CustomNetManager : NetworkManager
{

    public AIManager aiManager;
    public static List<GameObject> players;
    
    // Start is called before the first frame update


    public override void Awake()
    {
        base.Awake();

        aiManager.enabled = true;
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
        
        players.Add(conn.playerController.gameObject);
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

    // On server start spawn one of each base prefab per player out in nowhere land and assign it to players
    // players will modify those base objects and use them as their prefabs
    // on new scene load, save the objects in their current state and re-spawn them in the next scene + resetup the references
    
}
