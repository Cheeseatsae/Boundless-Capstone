using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "DropTable", order = 1)]
public class DropTable : ScriptableObject
{

    [Serializable]
    public class ItemContainer
    {
        public int itemId;
        public bool physical;
        public GameObject objectToAdd;
        public ItemBase scriptToAdd;
    }
    
    [SerializeField] public ItemContainer[] commonItems = new ItemContainer[0];
    [SerializeField] public ItemContainer[] uncommonItems = new ItemContainer[0];
    [SerializeField] public ItemContainer[] uniqueItems = new ItemContainer[0];
    [SerializeField] public ItemContainer[] legendaryItems = new ItemContainer[0];

}

