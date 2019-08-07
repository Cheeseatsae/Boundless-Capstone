using System.Collections;
using System.Collections.Generic;
using Mirror;
using System;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerBrain : NetworkBehaviour
{
    private CustomLobbyManager lobbyManager;
    
    public SyncListPrefabs playerPrefabs;
    public static ServerBrain server;
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        server = this;
        lobbyManager = FindObjectOfType<CustomLobbyManager>();

        lobbyManager.OnPlayerSpawn += SetupPlayerReferences;
    }

    private void SetupPlayerReferences(NetworkIdentity p, NetworkIdentity obj)
    {
        if (isServer)
        {

            PrefabContainer container = new PrefabContainer();
            container.player = p;
            container.prefab1 = obj;
            playerPrefabs.Add(container);

            Debug.LogWarning("Player " + p.gameObject.name + "Has prefab ", obj.gameObject);
        }
        
        StartCoroutine(SetupOtherPlayerReferences(p));
    }

    IEnumerator SetupOtherPlayerReferences(NetworkIdentity currentPlayer)
    {
        while (playerPrefabs.Count < 1)
        {
            yield return new WaitForSeconds(0.2f);
        } 
        
        foreach (PrefabContainer container in playerPrefabs)
        {
            if (container.player.netId == currentPlayer.netId) continue;

            container.player.GetComponent<Ability1>().bulletPref = container.prefab1.gameObject;
        }

        yield return null;
    }

    private void OnDestroy()
    {
        lobbyManager.OnPlayerSpawn -= SetupPlayerReferences;
    }
}

public struct PrefabContainer
{
    public NetworkIdentity player;
    public NetworkIdentity prefab1;
}

public class SyncListPrefabs : SyncList<PrefabContainer> {}