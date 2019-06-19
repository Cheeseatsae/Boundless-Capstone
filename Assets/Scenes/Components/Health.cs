using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Health : NetworkBehaviour
{
    public int baseMaxHealth;
    public int health;
    [HideInInspector] public int maxHealth;
    
    public int baseHealthRegen;
    public int healthRegen;
    public float regenTick = 1;
    
    //Events
    public delegate void TakeDamageDelegate(int amount);

    [SyncEvent] 
    public event TakeDamageDelegate EventTakeDamage;
    
    private void Start()
    {
        maxHealth = baseMaxHealth;
        health = baseMaxHealth;
        if (NetworkClient.active)
        {
            EventTakeDamage += TakeDamage;
        }
        if (healthRegen > 0)
        {
            StartCoroutine(Regen());
        }       
    }

    [Command]
    public void CmdDoDamage(int amount)
    {
        EventTakeDamage(amount);
    }

    private void TakeDamage(int amount)
    {
        health -= amount;
        
    }

    public void Update()
    {
        if (health == 0)
        {
            Debug.Log("ima deeeeed");
        }
    }

    private IEnumerator Regen()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(regenTick);
            health += healthRegen;
            health = Mathf.Clamp(health, 0, maxHealth);
            // send event
        }
    }
}
