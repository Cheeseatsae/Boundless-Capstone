using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockExplode : MonoBehaviour
{

    public GameObject player;
    public float triggerDistance;
    public Rigidbody rigidbody;
    public float explodeTimer;
    //Location
    public float minDisplacement;
    public float maxDisplacement;
    public LayerMask layer;
    public float currentDistance;
    
    //Shards
    public GameObject[] shards;
    public GameObject shard;
    public int numberOfShards;
    public float projectileSpeed;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

        if (player != null)
        {
            currentDistance = Vector3.Distance(this.gameObject.transform.position, player.transform.position);
            if (currentDistance <= triggerDistance)
            {
                //Explode
                Explode();
            }
        }

    }

    public void Explode()
    {
        Spawn();
        Destroy(this.gameObject);
    }
    
    public Vector3 GetLocation(GameObject target)
    {
        //Debug.Log("Run Location");
        var returnLocation = new Vector3(0, 0, 0);

        if (target == null) return Vector3.zero;

        var angle = new float();
        var dir = new Vector3();

        var location = new Vector3();
        var hit = new RaycastHit();

        var iterations = 0;


        while (hit.point == Vector3.zero && iterations < 15)
        {
            angle = Random.Range(0.0f, Mathf.PI * 2);
            dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            dir *= Random.Range(minDisplacement, maxDisplacement);
            location = new Vector3(target.transform.position.x + dir.x, target.transform.position.y, target.transform.position.z + dir.z);
            
            //Physics.Raycast(location, Vector3.down, out hit, 200, layer);
            Debug.DrawRay(location, Vector3.down, Color.red, 5);
            

            iterations++;
        }

        if (location != Vector3.zero)
            
            returnLocation = location;
        

        else
            returnLocation = Vector3.zero;


        

        return returnLocation;
    }

    public Vector3 GetSpawn(GameObject rock)
    {
        Vector3 spawnPoint = new Vector3(0, 0, 0);

        float xPoint = Random.Range(-rock.transform.localScale.x, rock.transform.localScale.x);
        float yPoint = Random.Range(-rock.transform.localScale.y, rock.transform.localScale.y);
        float zPoint = Random.Range(-rock.transform.localScale.z, rock.transform.localScale.z);
        
        Vector3 newPoint = new Vector3(xPoint,yPoint,zPoint);
        spawnPoint = rock.transform.position + newPoint;
        return spawnPoint;

    }
    
    public void Spawn()
    {
        for (var i = 0; i < numberOfShards; i++)
        {
            Vector3 location = GetSpawn(gameObject);
            int whichShard = Random.Range(0, shards.Length);
            GameObject newShard = Instantiate(shards[whichShard], location, Quaternion.identity);
            Rigidbody rb = newShard.GetComponent<Rigidbody>();
            ShardScript shardScript = newShard.GetComponent<ShardScript>();
            shardScript.player = player;
            Vector3 hitPoint = GetLocation(player);
            Vector3 dir = (hitPoint - transform.position).normalized;
            rb.velocity = dir * projectileSpeed;
        }
            
    }

    private void FixedUpdate()
    {
        if (rigidbody != null)
        {
            transform.LookAt(rigidbody.velocity + transform.position);
        }
    }

    public void Start()
    {
        StartCoroutine(Detonate());
    }

    public IEnumerator Detonate()
    {
        yield return new WaitForSeconds(explodeTimer);
        Explode();

    }
}
