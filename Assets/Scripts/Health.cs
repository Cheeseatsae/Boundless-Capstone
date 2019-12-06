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
    public delegate void BossDeath();

    public event HealthChange OnHealthChange;
    
    public event TakeDamageDelegate EventTakeDamage;
    
    public event OnDeath EventDeath;
    public static event OnDeath PlayerDeath;
    
    public event BossDeath BossDead;
    private bool bossDead = false;
    public GameObject airAiDeath;
    public ParticleSystem airDeath;
    

    private void Start()
    {
        healthRegen = baseHealthRegen;
        maxHealth = baseMaxHealth;
        health = baseMaxHealth;
        
        EventTakeDamage += TakeDamage;
        EventDeath += Death;
        if (healthRegen > 0)
        {
            if (!LevelManager.instance.paused)
            {
                StartCoroutine(Regen());
            }
        }  
            
    }
   
    private void Death()
    {
        if (GetComponent<PlayerModel>())
        {
            PlayerDeath?.Invoke();
            return;
        }

        if (GetComponent<AirAiModel>())
        {
            GameObject air = Instantiate(airAiDeath,transform.position, Quaternion.identity);
            airDeath = air.GetComponent<ParticleSystem>();
            airDeath.Play();
            StartCoroutine(DestoryObj(air));
            Destroy(gameObject);
        }

        if (GetComponent<GroundAI_Model>())
        {
            GroundAI_Model model = GetComponent<GroundAI_Model>();
            model.anim.SetTrigger("Dead");
            model.navmesh.isStopped = true;
            model.navmesh.velocity = new Vector3(0,0,0);
            model.GetComponent<Collider>().enabled = false;
        }

        if (GetComponent<Boss_Model>())
        {
            Boss_Model model = GetComponent<Boss_Model>();
            model.anim.SetTrigger("Dead");
            model.canCast = false;
            model.alive = false;
            if (!bossDead)
            {
                BossDead?.Invoke();
                bossDead = true;
            }
            
            
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
        while (true && !LevelManager.instance.paused)
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


    public IEnumerator DestoryObj(GameObject obj)
    {
        yield return new WaitForSeconds(1);
        Destroy(obj);
    }
    
}
