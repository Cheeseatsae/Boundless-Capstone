using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class AIManager : NetworkBehaviour
{
    public List<GameObject> aiList = new List<GameObject>();
    public GameObject airAi;
    public int amountToSpawn;

    public float distanceCheck;
    public float elapsedTime;

    public GameObject groundAI;
    public LayerMask layer;
    public int maxAI;
    public int maxEnemySpawnDistance;
    public int minEnemySpawnDistance;
    public int numberofAi;
    public int numberofKills;
    public float secondsBetweenSpawn;

    //AI Spawning Variables
    public Vector3 spawningLocation;
    public GameObject toSpawn;

    private void Start()
    {
        aiList.Add(groundAI);
        aiList.Add(airAi);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && isServer)
            CmdSpawn();
        //Debug.Log("fucking work... plz");

        elapsedTime += Time.deltaTime;

        if (elapsedTime > secondsBetweenSpawn)
        {
            elapsedTime = 0;
            if (numberofAi < maxAI) CmdSpawn();
        }

        if (numberofKills <= 0) StartCoroutine(RunEndLevel());
    }

    public Vector3 GetLocation(GameObject player)
    {
        //Debug.Log("Run Location");
        var returnLocation = new Vector3(0, 0, 0);

        if (CustomNetManager.players.Count < 1) return Vector3.zero;

        var angle = new float();
        var dir = new Vector3();

        var location = new Vector3();
        var hit = new RaycastHit();

        var iterations = 0;


        while (hit.point == Vector3.zero && iterations < 15)
        {
            angle = Random.Range(0.0f, Mathf.PI * 2);
            dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            dir *= Random.Range(minEnemySpawnDistance, maxEnemySpawnDistance);
            location = new Vector3(player.transform.position.x + dir.x, 100, player.transform.position.z + dir.z);
            //Debug.Log(location);
            Physics.Raycast(location, Vector3.down, out hit, 200, layer);
            Debug.DrawRay(location, Vector3.down, Color.red, 5);
            //Debug.Log(hit.point);

            iterations++;
        }

        if (hit.point != Vector3.zero)
            //Debug.Log("Didnt hit nothing");
            returnLocation = hit.point;
        //Debug.Log(returnLocation);

        else
            returnLocation = Vector3.zero;


        //Debug.Log(returnLocation);

        return returnLocation;
    }

    public GameObject PickWhatToSpawn()
    {
        var spawnThis = Random.Range(0, aiList.Count);
        var aiToSpawn = aiList[spawnThis];
        Debug.Log(aiToSpawn);
        return aiToSpawn;
    }


    [Command]
    public void CmdSpawn()
    {
        foreach (var player in CustomNetManager.players)
            for (var i = 0; i < amountToSpawn; i++)
            {
                toSpawn = PickWhatToSpawn();
                spawningLocation = GetLocation(player);
                if (spawningLocation == Vector3.zero)
                    return;
                if (toSpawn == airAi) spawningLocation.y = spawningLocation.y + 15;

                numberofAi++;
                //Debug.Log("spawn" + toSpawn);
                var ai = Instantiate(toSpawn, spawningLocation, Quaternion.identity);
                NetworkServer.Spawn(ai);
            }
    }

    [Command]
    public void CmdGotKillCount()
    {
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();
    }

    private IEnumerator RunEndLevel()
    {
        yield return new WaitForSeconds(2);
        CmdGotKillCount();
    }
}