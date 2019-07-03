using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TestChangeColour : ItemBase
{
    // Start is called before the first frame update
    public override void StackEffect()
    {
        base.StackEffect();

        var p = GetComponent<Ability1>();
        GameObject projectile = p.bulletPref;
        
        Renderer r = projectile.GetComponent<Renderer>();
        if (p == null) return;
        
        r.sharedMaterial.color = Color.red;
        

    }
}
