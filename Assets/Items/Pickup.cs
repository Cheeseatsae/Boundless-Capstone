using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pickup : Interactable
{
    [HideInInspector] public GameObject spawnPoint;
    public DropTable table;
    public DropTable.ItemContainer item;

    public void PickItem(int i)
    {
        item = table.Items[i];
    }
    
    public void PickItem()
    {
        item = table.Items[Random.Range(0,table.Items.Length)];
    }

    private void Start()
    {
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

    // for turning off physics on contact
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 10) return;
        
        GetComponent<BoxCollider>().isTrigger = true;
        Destroy(GetComponent<Rigidbody>());
    }
//
//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.GetComponent<PlayerModel>()) return;
//        
//        TakeItem(other.gameObject);
//    }
//
    public override void Interact(GameObject other)
    {
        TakeItem(other);
    }

    public void TakeItem(GameObject other)
    {
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
            ib.player = other.GetComponent<PlayerModel>();
            ib.StackEffect();
        }
        
        ItemPickedUp();
        Destroy(this.gameObject);
    }

    void ItemPickedUp()
    {
        // Effects or some shit
    }
    
}
