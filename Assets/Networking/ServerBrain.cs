using System.Collections;
using System.Collections.Generic;
using Mirror;
using System;
using UnityEngine;

public class ServerBrain : NetworkBehaviour
{
    private CustomLobbyManager lobbyManager;
    
    public SyncListPrefabs playerPrefabs;

    private void Awake()
    {
        lobbyManager = GetComponent<CustomLobbyManager>();

        lobbyManager.OnPlayerSpawn += SetupPlayerReferences;
    }

    private void SetupPlayerReferences(NetworkIdentity p, NetworkIdentity obj)
    {
        PrefabContainer container = new PrefabContainer();
        container.player = p.GetComponent<NetworkIdentity>();
        container.prefab1 = obj.GetComponent<NetworkIdentity>();
        playerPrefabs.Add(container);
    }

    private void OnDestroy()
    {
        lobbyManager.OnPlayerSpawn -= SetupPlayerReferences;
    }
}

[Serializable]
public struct PrefabContainer
{
    public NetworkIdentity player;
    public NetworkIdentity prefab1;
}

public class SyncListPrefabs : SyncList<PrefabContainer> {}