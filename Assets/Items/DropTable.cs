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
        public Color lightColour;
        public GameObject objectToAdd;
        public ItemBase scriptToAdd;
    }
    
    [SerializeField] public ItemContainer[] Items = new ItemContainer[0];

}

