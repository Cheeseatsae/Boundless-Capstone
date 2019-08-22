using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Speed : ItemBase
{

    private const float HardSpeedCap = 65;
    private const float SpeedBoost = 5f;
    
    
    public override void StackEffect()
    {
        base.StackEffect();

        var p = GetComponent<PlayerModel>();
        if (p == null) return;

        if (p.maxSpeed > HardSpeedCap) return;
        p.speed = p.baseSpeed + (stackCount * SpeedBoost * 10);
        p.maxSpeed = p.baseMaxSpeed + (stackCount * SpeedBoost);

    }

    public override void RemoveStack()
    {
        base.RemoveStack();
    }
    
}
