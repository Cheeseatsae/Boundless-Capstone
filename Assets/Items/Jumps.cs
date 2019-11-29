using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Jumps : ItemBase
{

    const string effect = "Number of Jumps increased";
    public override void StackEffect()
    {
        base.StackEffect();

        if (player == null) return;

        player.jumps = player.baseJumps + stackCount;
        LevelManager.instance.LogText(effect);
    }

    public override void RemoveStack()
    {
        base.RemoveStack();
    }
    
}
