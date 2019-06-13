using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public DropTable table;
    public DropTable.ItemContainer item;

    private void Awake()
    {
        item = table.commonItems[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        // get type of script
        ItemBase i = item.scriptToAdd;
        Type t = i.GetType();

        // getcomponent to see if player already has script
        if (other.gameObject.GetComponent(t))
            Debug.Log("AAAAAA");
        
        // add script
        ItemBase ib = (ItemBase)other.gameObject.AddComponent(t);
        
        ib.StackEffect();
    }
}
