using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : MonoBehaviour
{
    public List<GameObject> aiList = new List<GameObject>();
    public GameObject airAi;
    public int amountToSpawn;

    public GameObject player;

    public float distanceCheck;
    public float elapsedTime;

    public GameObject groundAI;
    public LayerMask layer;
    public int maxAI;
    public int maxEnemySpawnDistance;
    public int minEnemySpawnDistance;
    public int numberOfAi;

    public delegate void KillNumberChanged(int i);
    public event KillNumberChanged EventKillNumberChanged;

    public int numberOfKills;
    public int killLimit;
    public float secondsBetweenSpawn;
    public bool currentObjective = true;

    public GameObject golumSpawnEffect;
    public GameObject airAiSpawnEffect;
    public int numberofplayers;
    //AI Spawning Variables
    public Vector3 spawningLocation;
    public GameObject toSpawn;

    public delegate void KillObjective();
    public event KillObjective KillLimitMet;
    

    
    private bool limitHit = false;
    public bool stopSpawn = false;
    
    private void Start()
    {
        aiList.Add(groundAI);
        aiList.Add(airAi);
        LevelManager.instance.agni.GetComponent<Health>().BossDead += StopSpawn;
        //KillNumChanged(numberOfKills);
    }



    private void Update()
    {
        if (!stopSpawn)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > secondsBetweenSpawn)
            {
                elapsedTime = 0;
                if (numberOfAi < maxAI) Spawn();
            }
        }

        
        if (numberOfKills >= killLimit)
        {
            //Debug.Log("hit the limit change the things");
            if (limitHit == false)
            {
                KillLimitMet?.Invoke();
                currentObjective = false;
                limitHit = true;
            }

        }
    }



    public Vector3 GetLocation(GameObject player)
    {
        Debug.Log("Run Location");
        var returnLocation = new Vector3(0, 0, 0);

        if (player == null) return Vector3.zero;

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
            //Debug.DrawRay(location, Vector3.down, Color.red, 5);
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

    public void AiHasDied()
    {
        numberOfAi--;
        numberOfKills++;
        KillNumChanged(numberOfKills);
        PlayerInteraction.ChangeMoney(Random.Range(3,8));
    }
    

    void KillNumChanged(int i)
    {
        if (currentObjective)
        {
            EventKillNumberChanged?.Invoke(i);
        }
        
    }
    
    public void Spawn()
    {
        for (var i = 0; i < amountToSpawn; i++)
        {
            toSpawn = PickWhatToSpawn();
            spawningLocation = GetLocation(player);
            if (spawningLocation == Vector3.zero)
                return;
            if (toSpawn == airAi)
            {
                spawningLocation.y = spawningLocation.y + 15;
                GameObject spawnEffect = Instantiate(airAiSpawnEffect, spawningLocation, Quaternion.identity);
                Destroy(spawnEffect,2f);
            }else if (toSpawn == groundAI)
            {
                GameObject spawnEffect = Instantiate(golumSpawnEffect, spawningLocation, Quaternion.identity);
                Destroy(spawnEffect,2f);
            }

            numberOfAi++;
            Debug.Log("spawn" + toSpawn);
            Instantiate(toSpawn, spawningLocation, Quaternion.identity);
                
        }
            
    }

    
    public void GotKillCount()
    {
        //AddEndGame thing
        Debug.Log("Kill achieved");

    }

    private void RunEndLevel()
    {
        
        StartCoroutine(EndLevelCoroutine());
    }
    
    private IEnumerator EndLevelCoroutine()
    {
        yield return new WaitForSeconds(2);
        GotKillCount();
    }

    public void StopSpawn()
    {
        stopSpawn = true;
    }
}