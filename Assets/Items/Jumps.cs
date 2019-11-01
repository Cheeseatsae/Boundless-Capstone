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

        if (player == null) return;

        player.jumps = player.baseJumps + stackCount;

    }

    public override void RemoveStack()
    {
        base.RemoveStack();
    }
    
}
