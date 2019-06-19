using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{

    public int stackCount = 0;
    [HideInInspector] public GameObject player;
    
    // on item pickup - must include stacking effect
    public virtual void StackEffect()
    {
        stackCount++;
        
    }

    // on item remove
    public virtual void RemoveStack()
    {
        if (stackCount <= 0) return;
            
        stackCount--;
    }
    
}
