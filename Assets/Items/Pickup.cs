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
        
        if (other.gameObject.GetComponent<PlayerModel>())
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
    }
}
