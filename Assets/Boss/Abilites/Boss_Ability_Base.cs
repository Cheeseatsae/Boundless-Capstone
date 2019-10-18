using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Ability_Base : MonoBehaviour
{
    public float abilityCD;

    public float damage;

    public bool onCd;

    public GameObject target;

    public float castTime;

    public bool isCasting = false;

    public Boss_Model model;

    public virtual void Awake()
    {
        model = gameObject.GetComponent<Boss_Model>();
    }


    public virtual void Cast()
    {
        
    }

    public IEnumerator Cooldown()
    {
        onCd = true;
        yield return new WaitForSeconds(abilityCD);
        onCd = false;
    }
    public IEnumerator Casting()
    {
        isCasting = true;
        yield return new WaitForSeconds(castTime);
        isCasting = false;
        StartCoroutine(Cooldown());
    }


    
}
