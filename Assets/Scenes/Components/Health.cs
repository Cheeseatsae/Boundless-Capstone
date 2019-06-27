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
    [HideInInspector] public AIManager aiManager;
    public int baseHealthRegen;
    public int healthRegen;
    public float regenTick = 1;
    
    //Events
    public delegate void TakeDamageDelegate(int amount);
    public delegate void OnDeath();
    public delegate void ReCalPlayers(GameObject player);
    [SyncEvent] 
    public event TakeDamageDelegate EventTakeDamage;

    [SyncEvent] 
    public event OnDeath EventDeath;
    
    [SyncEvent] 
    public event ReCalPlayers EventRecal;

    public void Awake()
    {
        aiManager = FindObjectOfType<AIManager>();
    }


    private void Start()
    {
        maxHealth = baseMaxHealth;
        health = baseMaxHealth;
        if (NetworkClient.active)
        {
            EventTakeDamage += TakeDamage;
            EventDeath += Death;
            
        }
        if (healthRegen > 0)
        {
            StartCoroutine(Regen());
        }       
    }

    private void ReCal()
    {
        
    }

    [Command]
    private void CmdDeath()
    {
        EventDeath();
    }
    
    private void Death()
    {
        if (GetComponent<PlayerModel>())
        {
            aiManager.Players.Remove(gameObject);
        }
        
        NetworkServer.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }

    [Command]
    public void CmdDoDamage(int amount)
    {
        EventTakeDamage(amount);
    }

    private void TakeDamage(int amount)
    {
        health -= amount;
        CheckForDeath();
        
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

    public void CheckForDeath()
    {
        if (health <= 0)
        {
            EventDeath();
        }
    }
    
    
}
