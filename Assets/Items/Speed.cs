using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Speed : ItemBase
{
    
    public override void StackEffect()
    {
        base.StackEffect();

        var p = GetComponent<PlayerModel>();
        if (p == null) return;
        
        p.speed += 5;
        p.maxSpeed += 5;

    }

    public override void RemoveStack()
    {
        base.RemoveStack();
    }
    
}
