﻿using System;
using UnityEngine;

[Serializable]
public class ImpromptuNeedler : ItemBase
{
    private int _baseExplosionDmg = 8;
    private int _explosionDmg;

    private void Start()
    {
        PlayerEvents.OnPlayerDamageInstance += AttachNeedle;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnPlayerDamageInstance -= AttachNeedle;
    }

    public override void StackEffect()
    {
        base.StackEffect();

        if (player == null) return;

        if (stackCount > 1) _explosionDmg = _baseExplosionDmg * (stackCount / 3);
        else _explosionDmg = _baseExplosionDmg;
    }

    private void AttachNeedle(GameObject obj, float dmg, Vector3 loc)
    {
        // will need to make a needle manager scrip
        //
        // attach script to enemy if they dont have it already
        // spawn visual needle to sit there and explode
        // have needle script check for number of needles
        // detonate after time or when hit max needles
        // do bonus damage to enemy from needle script
    }
    
    public override void RemoveStack()
    {
        base.RemoveStack();
        
        if (stackCount > 1) _explosionDmg = _baseExplosionDmg * (stackCount / 3);
        else _explosionDmg = _baseExplosionDmg;
    }
}
