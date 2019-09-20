using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
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
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < toSpawnOnStart; i++)
        {
            SpawnPickup();
        }
    }

    void SpawnPickup()
    {
        if (usedSpawnPoints.Count >= itemSpawnPoints.Count) return;

        GameObject spawn = GetSpawnPoint();

        if (spawn == null) return;
        
        usedSpawnPoints.Add(spawn);
        
        GameObject obj = Instantiate(pickup, spawn.transform.position + offset, Quaternion.identity);
        Pickup p = obj.GetComponent<Pickup>();

        p.spawnPoint = spawn;
        p.PickItem(Random.Range(0,table.Items.Length));
        p.SetupItemVisuals();
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

}


