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

    private void Awake()
    {
        needle = Resources.Load("Needle", typeof(GameObject)) as GameObject;

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
