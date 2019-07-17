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
        base.OnServerAddPlayer(conn, extraMessage);
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

    // on player spawn generate prefabs per player where necessary 
    // 
    // have items that require create instance of spawnable prefab and update it, then replace the old one
    // will need to pass player, ability, new prefab/added component 
    
}
