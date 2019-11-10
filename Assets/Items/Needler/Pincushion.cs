using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pincushion : MonoBehaviour
{
    private GameObject needle;
    private List<NeedleScript> activeNeedles = new List<NeedleScript>();

    private int damage;
    private Health health;

    private const float DetonationTime = 2.75f;

    private void Awake()
    {
        // Resource is not being loaded correctly and will need to be instantiated or saved somewhere else or with another method
        needle = Resources.Load("Assets/Items/Needler/Needle.prefab") as GameObject;
        health = GetComponent<Health>();
    }

    public void AttachNeedle(int dmg, Vector3 loc)
    {
        NeedleScript n = Instantiate(needle, loc, Quaternion.identity, transform).GetComponent<NeedleScript>();
        activeNeedles.Add(n);
        
        damage = dmg;
        StartCoroutine(NeedleTimer(n));
    }

    private void FixedUpdate()
    {
        if (activeNeedles.Count > 9)
        {
            foreach (NeedleScript n in activeNeedles)
            {
                n.Detonate();
            }

            health.DoDamage(damage * 11);
            StopAllCoroutines();
            activeNeedles.Clear();
        }
    }
    
    private IEnumerator NeedleTimer(NeedleScript n)
    {
        yield return new WaitForSeconds(DetonationTime);
        activeNeedles.Remove(n);
        n.Detonate();
        health.DoDamage(damage);
    }
}
