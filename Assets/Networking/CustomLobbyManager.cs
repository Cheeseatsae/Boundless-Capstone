using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mono.CecilX.Cil;
using Mono.CompilerServices.SymbolWriter;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomLobbyManager : NetworkLobbyManager
{
    
    public static AIManager aiManager;
    public static List<GameObject> players = new List<GameObject>();

    [Header("Player Prefabs")]
    public GameObject playerBulletPrefab;
    
    public delegate void PlayerSpawn(NetworkIdentity p, NetworkIdentity obj);
    public event PlayerSpawn OnPlayerSpawn;

    public delegate void SceneChangeComplete();
    public static event SceneChangeComplete OnSceneChangeComplete;
    
    public int kills;
    public int ai;
    
    public Slider killsSlider;
    public Slider aiSlider;

    public override void OnServerReady(NetworkConnection conn)
    {
        if (LogFilter.Debug) Debug.Log("NetworkLobbyManager OnServerReady");
        base.OnServerReady(conn);

        if (conn != null && conn.playerController != null)
        {
            GameObject lobbyPlayer = conn.playerController.gameObject;

            // if null or not a lobby player, dont replace it
            if (lobbyPlayer != null && lobbyPlayer.GetComponent<NetworkLobbyPlayer>() != null)
                SceneLoadedForPlayer(conn, lobbyPlayer);
        }
        
    }

    public override void OnLobbyStartHost()
    {
        
        //killsSlider.enabled = true;
    }
    public override void OnLobbyStartServer()
    {
        
        //killsSlider.enabled = true;
    }
        
        

    public override void OnServerAddPlayer(NetworkConnection conn, AddPlayerMessage extraMessage)
    {
        if (SceneManager.GetActiveScene().name == LobbyScene)
        {
            if (lobbySlots.Count == maxConnections) return;

            allPlayersReady = false;

            if (LogFilter.Debug) Debug.LogFormat("NetworkLobbyManager.OnServerAddPlayer playerPrefab:{0}", lobbyPlayerPrefab.name);

            GameObject newLobbyGameObject = OnLobbyServerCreateLobbyPlayer(conn);
            if (newLobbyGameObject == null)
                newLobbyGameObject = (GameObject)Instantiate(lobbyPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);

            NetworkLobbyPlayer newLobbyPlayer = newLobbyGameObject.GetComponent<NetworkLobbyPlayer>();

            lobbySlots.Add(newLobbyPlayer);

            RecalculateLobbyPlayerIndices();
            

            NetworkServer.AddPlayerForConnection(conn, newLobbyGameObject); 
            //conns.Add(conn.playerController.gameObject);
            
        }
        
    }
    
    private void SetupPlayer(GameObject p)
    {
        Vector3 spawnPos = new Vector3(0,-15000,0);
        GameObject obj = Instantiate(playerBulletPrefab, spawnPos, Quaternion.identity);
        
        NetworkServer.Spawn(obj);
        
        p.GetComponent<Ability1>().bulletPref = obj;
        
        OnPlayerSpawn?.Invoke(p.GetComponent<NetworkIdentity>(), obj.GetComponent<NetworkIdentity>());
    }
    
    void RecalculateLobbyPlayerIndices()
    {
        if (lobbySlots.Count > 0)
        {
            for (int i = 0; i < lobbySlots.Count; i++)
            {
                lobbySlots[i].Index = i;
            }
        }
    }
    
    public override void OnServerSceneChanged(string sceneName)
    {
        if (sceneName != LobbyScene)
        {
            // call SceneLoadedForPlayer on any players that become ready while we were loading the scene.
            foreach (PendingPlayer pending in pendingPlayers)
            {
                SceneLoadedForPlayer(pending.conn, pending.lobbyPlayer);
                
            }
                
            pendingPlayers.Clear();
            
        }

        OnLobbyServerSceneChanged(sceneName);
        OnSceneChangeComplete?.Invoke();
        
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

        if (SceneManager.GetActiveScene().name == "LobbyTest")
        {
            ai = (int) aiSlider.value;
            kills = (int)killsSlider.value;
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
    
    public void SceneLoadedForPlayer(NetworkConnection conn, GameObject lobbyPlayer)
    {
        if (LogFilter.Debug) Debug.LogFormat("NetworkLobby SceneLoadedForPlayer scene: {0} {1}", SceneManager.GetActiveScene().name, conn);

        if (SceneManager.GetActiveScene().name == LobbyScene)
        {
            // cant be ready in lobby, add to ready list
            PendingPlayer pending;
            pending.conn = conn;
            pending.lobbyPlayer = lobbyPlayer;
            pendingPlayers.Add(pending);
            
            return;
        }

        GameObject gamePlayer = OnLobbyServerCreateGamePlayer(conn);       
        
        if (gamePlayer == null)
        {
            // get start position from base class
            Transform startPos = GetStartPosition();
            gamePlayer = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            gamePlayer.name = playerPrefab.name;
                        
        }

        if (!OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer))
            return;
        aiManager.numberOfKills = kills;
        aiManager.amountToSpawn = ai;
        aiManager.maxAI = aiManager.amountToSpawn * 10;
        
        // replace lobby player with game player
        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer);
        players.Add(conn.playerController.gameObject); // add players to list 
        SetupPlayer(conn.playerController.gameObject);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

}
