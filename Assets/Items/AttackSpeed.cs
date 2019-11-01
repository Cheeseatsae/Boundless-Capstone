using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackSpeed : ItemBase
{

    private const float AttackSpeedBoost = 6;
    
    public override void StackEffect()
    {
        base.StackEffect();

        if (player == null) return;

        player.attackSpeed = player.baseAttackSpeed + (stackCount * AttackSpeedBoost);

    }

    public override void RemoveStack()
    {
        base.RemoveStack();
    }
    
}
