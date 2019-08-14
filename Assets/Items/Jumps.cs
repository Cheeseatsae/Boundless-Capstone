using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Jumps : ItemBase
{

    public override void StackEffect()
    {
        base.StackEffect();

        var p = GetComponent<PlayerModel>();
        if (p == null) return;

        p.jumps = p.baseJumps + stackCount;

    }

    public override void RemoveStack()
    {
        base.RemoveStack();
    }
    
}
