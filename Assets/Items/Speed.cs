using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Speed : ItemBase
{

    private const float HardSpeedCap = 65;
    private const float SpeedBoost = 5f;
    const string effect = "Movement Speed Increased";
    
    public override void StackEffect()
    {
        base.StackEffect();

        if (player == null) return;

        if (player.maxSpeed > HardSpeedCap) return;
        player.speed = player.baseSpeed + (stackCount * SpeedBoost * 10);
        player.maxSpeed = player.baseMaxSpeed + (stackCount * SpeedBoost);
        LevelManager.instance.LogText(effect);
    }

    public override void RemoveStack()
    {
        base.RemoveStack();
    }
    
}
