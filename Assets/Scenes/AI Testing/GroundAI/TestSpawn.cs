using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TestSpawn : NetworkBehaviour
{
    public GameObject groundAI;

    public Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CmdFire();
        }
    }
    
    void CmdFire()
    {
        GameObject AI = Instantiate(groundAI, spawnPoint.position, Quaternion.identity);
        NetworkServer.Spawn(AI);
        Debug.Log("test");
    }
}
