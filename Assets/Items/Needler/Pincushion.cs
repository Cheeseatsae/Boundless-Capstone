using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Pincushion : MonoBehaviour
{
    public GameObject needle;
    private List<NeedleScript> activeNeedles = new List<NeedleScript>();

    private int damage;
    private Health health;

    private const float DetonationTime = 2.75f;
    private const int DetonationRequirement = 5;

    private void Awake()
    {
        needle = Resources.Load("Needle", typeof(GameObject)) as GameObject;

        health = GetComponent<Health>();
        health.EventDeath += DestroyNeedlesOnDeath;
    }

    private void OnDestroy()
    {
        health.EventDeath -= DestroyNeedlesOnDeath;
    }

    public void AttachNeedle(int dmg, Vector3 loc)
    {
        NeedleScript n = Instantiate(needle, loc, Quaternion.identity, transform).GetComponent<NeedleScript>();
        activeNeedles.Add(n);
        
        damage = dmg;
        StartCoroutine(NeedleTimer(n));
    }

    private void Update()
    {
        if (activeNeedles.Count > DetonationRequirement)
        {
            foreach (NeedleScript n in activeNeedles)
            {
                n.Detonate();
            }

            health.DoDamage(damage * (DetonationRequirement + 1));
            StopAllCoroutines();
            activeNeedles.Clear();
        }
    }

    private void DestroyNeedlesOnDeath()
    {
        foreach (NeedleScript n in activeNeedles)
        {
            n.Detonate();
        }
    }
    
    private IEnumerator NeedleTimer(NeedleScript n)
    {
        yield return new WaitForSeconds(DetonationTime);
        activeNeedles.Remove(n);
        n.Detonate();
        if (health != null) health.DoDamage(damage);
    }
}
