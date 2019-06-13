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
        var i = item.scriptToAdd;
        other.gameObject.AddComponent<>();
    }
}
