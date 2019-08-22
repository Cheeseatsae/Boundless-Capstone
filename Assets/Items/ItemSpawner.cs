using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{

    public GameObject pickup;
    public DropTable table;

    public float toSpawnOnStart;

    public List<GameObject> itemSpawnPoints;
    private List<GameObject> usedSpawnPoints;

    private void Start()
    {
        if (!isServer) return;

        for (int i = 0; i < toSpawnOnStart; i++)
        {
            
        }
    }

    void SpawnPickup()
    {
        if (!isServer) return;
        
        
        
    }
}
