using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Damage : ItemBase
{

    private const float DamageBoost = 5;
    
    public override void StackEffect()
    {
        base.StackEffect();

        var p = GetComponent<PlayerModel>();
        if (p == null) return;

        p.attackDamage = p.baseAttackDamage + (stackCount * DamageBoost);

    }

    public override void RemoveStack()
    {
        base.RemoveStack();
    }
    
}
