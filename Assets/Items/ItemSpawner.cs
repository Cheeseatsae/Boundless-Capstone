using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : NetworkBehaviour
{
    public static ItemSpawner instance;
    
    public GameObject pickup;
    public DropTable table;

    public int toSpawnOnStart;
    public Vector3 offset;

    public int killRequirement;
    private int killCount;
    
    public List<GameObject> itemSpawnPoints;
    public List<GameObject> usedSpawnPoints;

    public SyncListItems spawnedItems;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!isServer) return;

        CustomLobbyManager.aiManager.EventKillNumberChanged += SpawnOnEnemyDeath;
        
        for (int i = 0; i < toSpawnOnStart; i++)
        {
            SpawnPickup();
        }
    }

    private void OnDestroy()
    {
        CustomLobbyManager.aiManager.EventKillNumberChanged -= SpawnOnEnemyDeath;
    }

    void SpawnOnEnemyDeath(int i)
    {
        killCount++;

        if (killCount < killRequirement) return;
        
        killCount = 0;
        SpawnPickup();
    }
    
    void SpawnPickup()
    {
        if (!isServer) return;
        if (usedSpawnPoints.Count >= itemSpawnPoints.Count) return;

        GameObject spawn = GetSpawnPoint();

        if (spawn == null) return;
        
        usedSpawnPoints.Add(spawn);
        
        GameObject obj = Instantiate(pickup, spawn.transform.position + offset, Quaternion.identity);
        Pickup p = obj.GetComponent<Pickup>();

        p.spawnPoint = spawn;
        p.PickItem(Random.Range(0,table.Items.Length));
        p.SetupItemVisuals();

        NetworkServer.Spawn(obj);
        AddToNetworkItems(obj);
    }

    GameObject GetSpawnPoint()
    {

        GameObject obj = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Count - 1)];
        int i = 0;

        while (usedSpawnPoints.Contains(obj) && i < itemSpawnPoints.Count)
        {
            i++;
            obj = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Count - 1)];
        }

        return i >= itemSpawnPoints.Count ? null : obj;
    }

    void AddToNetworkItems(GameObject obj)
    {
        NetworkItem i = new NetworkItem();
        i.item = obj;
        i.itemId = obj.GetComponent<Pickup>().item.itemId;
        spawnedItems.Add(i);
    }

    public void RemoveNetworkItem(GameObject obj, GameObject s)
    {
        NetworkItem toRemove = new NetworkItem();
        
        foreach (NetworkItem item in spawnedItems)
        {
            if (item.item == obj)
            {
                toRemove = item;
                break;
            }
        }

        spawnedItems.Remove(toRemove);
        usedSpawnPoints.Remove(s);
    }
    
    public struct NetworkItem
    {
        public GameObject item;
        public int itemId;
    }

    public class SyncListItems : SyncList<NetworkItem> {}
}


