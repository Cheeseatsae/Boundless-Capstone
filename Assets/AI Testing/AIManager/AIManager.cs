using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AIManager : NetworkBehaviour
{
    
    public float distanceCheck;
    public int maxEnemySpawnDistance;
    public int minEnemySpawnDistance;

    public GameObject groundAI;
    
    [Command]
    public void CmdSpawnAroundPlayer()
    {
        foreach (GameObject player in CustomNetManager.players)
        {
            float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2);
            
            Vector3 dir = new Vector3(Mathf.Sin(angle),0,Mathf.Cos(angle));

            dir *= UnityEngine.Random.Range(minEnemySpawnDistance, maxEnemySpawnDistance);

//            RaycastHit hit;
            //Need to fix raycast not working on greybox level
            //if (Physics.Raycast(dir, Vector3.down,out hit, distanceCheck))
            //{
                //if (hit.collider != null)
                //{
                    GameObject AI = Instantiate(groundAI, player.transform.position + dir, Quaternion.identity);
                    NetworkServer.Spawn(AI);
                    //Debug.Log("i did a thing");
                //}
            //}
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && isServer)
        {
            CmdSpawnAroundPlayer();
            //Debug.Log("fucking work... plz");
        }
    }
}
