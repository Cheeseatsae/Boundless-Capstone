using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = System.Random;

public class AIManager : NetworkBehaviour
{
    
    public float distanceCheck;
    public int maxEnemySpawnDistance;
    public int minEnemySpawnDistance;

    public GameObject groundAI;
    public GameObject airAi;
    
    //AI Spawning Variables
    public Vector3 spawningLocation;
    public int amountToSpawn;
    public List<GameObject> aiList = new List<GameObject>();
    public GameObject toSpawn;
    public LayerMask layer;

    private void Start()
    {
        aiList.Add(groundAI);
        aiList.Add(airAi);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && isServer)
        {
            CmdSpawn();
            //Debug.Log("fucking work... plz");
        }
    }
    
    public Vector3 GetLocation()
    {
        Debug.Log("Run Location");
        Vector3 returnLocation = new Vector3(0,0,0);
        
        if (CustomNetManager.players.Count < 1)
        {
            return Vector3.zero;
        }
        foreach (GameObject player in CustomNetManager.players)
        {
            float angle = new float();
            Vector3 dir = new Vector3();
            
            Vector3 location = new Vector3();
            RaycastHit hit = new RaycastHit();

            int iterations = 0;
            

            while (hit.point == Vector3.zero && iterations < 15) 
            {
                angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
                dir = new Vector3(Mathf.Sin(angle),0,Mathf.Cos(angle));
                dir *= UnityEngine.Random.Range(minEnemySpawnDistance, maxEnemySpawnDistance);
                location = new Vector3(player.transform.position.x + dir.x, 100, player.transform.position.z + dir.z);
                Debug.Log(location);
                Physics.Raycast(location, Vector3.down, out hit, 200, layer);
                Debug.DrawRay(location, Vector3.down, Color.red, 5);
                Debug.Log(hit.point);
                
                iterations++;

            }
            if (hit.point != Vector3.zero)
            {
                Debug.Log("Didnt hit nothing");
                returnLocation = hit.point;
                Debug.Log(returnLocation);
            }

            else
            {
                returnLocation = Vector3.zero;
            }
            
        }
        //Debug.Log(returnLocation);

        return returnLocation;
    }

    public GameObject PickWhatToSpawn()
    {
        int spawnThis = UnityEngine.Random.Range(0, aiList.Count);
        GameObject aiToSpawn = aiList[spawnThis];
        Debug.Log(aiToSpawn);
        return aiToSpawn;
        
    }


    [Command]
    public void CmdSpawn()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            toSpawn = PickWhatToSpawn();
            spawningLocation = GetLocation();
            if (spawningLocation == Vector3.zero)
                return;
            if (toSpawn == airAi)
            {
                spawningLocation.y = spawningLocation.y + 15;
            }
            Debug.Log("spawn" + toSpawn);
            GameObject ai = Instantiate(toSpawn, spawningLocation, Quaternion.identity);
            NetworkServer.Spawn(ai);

        }
    }
    
}
