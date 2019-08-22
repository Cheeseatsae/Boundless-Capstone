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
    [HideInInspector] public int healthRegen;
    public float regenTick = 1;
    
    //Events
    public delegate void HealthChange();
    public delegate void TakeDamageDelegate(int amount);
    public delegate void OnDeath();

    public event HealthChange OnHealthChange;
    
    [SyncEvent] 
    public event TakeDamageDelegate EventTakeDamage;

    [SyncEvent] 
    public event OnDeath EventDeath;

    private void Start()
    {
        healthRegen = baseHealthRegen;
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
   
    private void Death()
    {
        if (!isServer) return;
        
        if (GetComponent<PlayerModel>())
        {
            CustomLobbyManager.players.Remove(gameObject);
            
            if (CustomLobbyManager.players.Count < 1)
            {
                CustomLobbyManager.aiManager.CmdGotKillCount();
            }
        }
        
        if (GetComponent<AIBaseModel>())
        {
            CustomLobbyManager.aiManager.AiHasDied();
        }
        
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
    }

    [Command]
    public void CmdDoDamage(int amount)
    {
        EventTakeDamage?.Invoke(amount);
    }

    private void TakeDamage(int amount)
    {
        health -= amount;
        OnHealthChange?.Invoke();
        CheckForDeath();
    }

    private IEnumerator Regen()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(regenTick);
            health += healthRegen;
            health = Mathf.Clamp(health, 0, maxHealth);
            
            OnHealthChange?.Invoke();
        }
    }

    public void CheckForDeath()
    {
        if (!isServer) return;
        if (health <= 0)
        {
            
            EventDeath?.Invoke();
        }
    }
    
    
    
    
}
