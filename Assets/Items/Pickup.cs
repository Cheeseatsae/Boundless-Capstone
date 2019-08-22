using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pickup : NetworkBehaviour
{
    [HideInInspector] public GameObject spawnPoint;
    public DropTable table;
    public DropTable.ItemContainer item;

    public void PickItem(int i)
    {
        item = table.Items[i];
    }

    private void Start()
    {
        StartCoroutine(Setup());
    }


    IEnumerator Setup()
    {
        yield return new WaitForSecondsRealtime(1);

        foreach (ItemSpawner.NetworkItem i in ItemSpawner.instance.spawnedItems)
        {
            if (i.item == gameObject) item = table.Items[i.itemId];
        }
        
        SetupItemVisuals();
    }
    
    public void SetupItemVisuals()
    {
        transform.localScale = item.objectToAdd.transform.localScale;
        transform.rotation = item.objectToAdd.transform.rotation;

        GetComponent<MeshRenderer>().material = item.objectToAdd.GetComponent<MeshRenderer>().sharedMaterial;
        GetComponent<MeshFilter>().mesh = item.objectToAdd.GetComponent<MeshFilter>().sharedMesh;
        GetComponent<Light>().color = item.lightColour;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if (!other.GetComponent<PlayerModel>()) return;
        
        TakeItem(other.gameObject);
    }

    public void TakeItem(GameObject other)
    {
        // get type of script
        ItemBase i = item.scriptToAdd;
        Type t = i.GetType();

        ItemBase ibfound = (ItemBase)other.GetComponent(t);
        
        // getcomponent to see if player already has script
        if (ibfound != null)
        {
            ibfound.StackEffect();
        }
        else
        {
            ItemBase ib = (ItemBase)other.AddComponent(t);
            ib.StackEffect();
        }
        
        ItemPickedUp();
        NetworkServer.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }

    void ItemPickedUp()
    {
        ItemSpawner.instance.RemoveNetworkItem(this.gameObject, spawnPoint);
    }
    
}
