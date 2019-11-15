using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
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
    
    public event TakeDamageDelegate EventTakeDamage;
    
    public event OnDeath EventDeath;

    private void Start()
    {
        healthRegen = baseHealthRegen;
        maxHealth = baseMaxHealth;
        health = baseMaxHealth;
        
        EventTakeDamage += TakeDamage;
        EventDeath += Death;
        
        if (healthRegen > 0)
        {
            StartCoroutine(Regen());
        }       
    }
   
    private void Death()
    {
        if (GetComponent<PlayerModel>())
        {
            //Do Da Death things
            Destroy(gameObject);
            return;
        }

        if (GetComponent<AirAiModel>())
        {
            Destroy(gameObject);
        }

        if (GetComponent<GroundAI_Model>())
        {
            GroundAI_Model model = GetComponent<GroundAI_Model>();
            model.anim.SetTrigger("Dead");
            model.GetComponent<Collider>().enabled = false;
        }
        
        LevelManager.aiManager.AiHasDied();
        
        
    }


    public void DoDamage(int amount)
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
        if (health <= 0)
        {
            
            EventDeath?.Invoke();
        }
    }
    
    
    
    
}
