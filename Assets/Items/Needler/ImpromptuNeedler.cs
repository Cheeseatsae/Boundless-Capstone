using System;
using UnityEngine;

[Serializable]
public class ImpromptuNeedler : ItemBase
{
    private int _baseExplosionDmg = 3;
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

    private void AttachNeedle(GameObject obj, int dmg, Vector3 loc)
    {
        // will need to make a needle manager script
        // 
        // attach script to enemy if they dont have it already
        // spawn visual needle to sit there and explode
        // have needle script check for number of needles
        // detonate after time or when hit max needles
        // do bonus damage to enemy from needle script

        Pincushion cushion = obj.GetComponent<Pincushion>();
        
        if (cushion == null)
        {
            // add pincushion
            cushion = obj.AddComponent<Pincushion>();
        }
        // attach needle
        // set damage & number to explode
        // start coroutine for needle
        cushion.AttachNeedle(_explosionDmg, loc);
    }
    
    public override void RemoveStack()
    {
        base.RemoveStack();
        
        if (stackCount > 1) _explosionDmg = _baseExplosionDmg * (stackCount / 3);
        else _explosionDmg = _baseExplosionDmg;
    }
}
