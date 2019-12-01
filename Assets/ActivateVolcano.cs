using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateVolcano : MonoBehaviour
{

    public float waitTime;
    public float destroyTime;

    public MeshCollider[] shardList;

    public Transform explosionPoint;

    public float explosionForce;

    public float radius;

    public float upModifier;
    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.portal.PortalActivate += Explosion;
        //shardList = gameObject.GetComponentsInChildren<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explosion()
    {
        StartCoroutine(ExploWithWait());
    }

    public IEnumerator ExploWithWait()
    {
        yield return new WaitForSeconds(waitTime);
        foreach (MeshCollider collider in shardList)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(explosionForce,explosionPoint.position, radius, upModifier);
                StartCoroutine(DestroyShard(collider.gameObject));
            }
        }
        
        
    }

    public IEnumerator DestroyShard(GameObject shard)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(shard);
    }
}
