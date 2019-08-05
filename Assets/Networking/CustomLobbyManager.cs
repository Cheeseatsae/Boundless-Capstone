using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomLobbyManager : NetworkLobbyManager
{
    
    public AIManager managerRef;
    public static AIManager aiManager;
    public static List<GameObject> players = new List<GameObject>();
    public List<GameObject> conns = new List<GameObject>();

    [Header("Player Prefabs")]
    public GameObject playerBulletPrefab;

    public override void OnLobbyServerPlayersReady()
    {
        base.OnLobbyServerPlayersReady();
        
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

        if (SceneManager.GetActiveScene().name == GameplayScene)
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
        
            
            //SetupPlayer(conn.playerController.gameObject);
        
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
    
    private void SetupPlayer(GameObject p)
    {
        Vector3 spawnPos = new Vector3(0,-15000,0);
        GameObject obj = Instantiate(playerBulletPrefab, spawnPos, Quaternion.identity);
        
        NetworkServer.Spawn(obj);

        p.GetComponent<Ability1>().bulletPref = obj;
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
            managerRef = FindObjectOfType<AIManager>();
            aiManager = managerRef;
            
        }

        OnLobbyServerSceneChanged(sceneName);
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
    
    void SceneLoadedForPlayer(NetworkConnection conn, GameObject lobbyPlayer)
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



        // replace lobby player with game player
        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer);
        
        

    }

}
