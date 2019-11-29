using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Damage : ItemBase
{

    private const float DamageBoost = 5;
    const string effect = "Damage Increased";
    public override void StackEffect()
    {
        base.StackEffect();
        
        if (player == null) return;

        player.attackDamage = player.baseAttackDamage + (stackCount * DamageBoost);
        LevelManager.instance.LogText(effect);
    }

    public override void RemoveStack()
    {
        base.RemoveStack();
    }
    
}
