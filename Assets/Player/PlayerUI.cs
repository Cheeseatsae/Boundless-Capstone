using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    public static PlayerUI instance;
    [HideInInspector] public PlayerModel player;
    [HideInInspector] public Health playerHealth;

    [HideInInspector] public Ability1 ability1;
    [HideInInspector] public Ability2 ability2;
    [HideInInspector] public Ability3 ability3;
    [HideInInspector] public Ability4 ability4;
    
    public Image lMouse, rMouse, q, r;
    
    public Slider health;

    public Text healthText;
    
    private void Awake()
    {
        instance = this;
    }

    public void Setup(GameObject p)
    {
        ability1 = p.GetComponent<Ability1>();
        ability2 = p.GetComponent<Ability2>();
        ability3 = p.GetComponent<Ability3>();
        ability4 = p.GetComponent<Ability4>();
    }
    
    public void UpdateHealth()
    {
        health.maxValue = playerHealth.maxHealth;
        health.value = playerHealth.health;
        healthText.text = playerHealth.health.ToString();
    }

    public void LMouseCooldown()
    {
        StartCoroutine(RunCooldown(lMouse, ability1.cooldown));
    }
    
    public void RMouseCooldown()
    {
        StartCoroutine(RunCooldown(rMouse, ability2.cooldown));
    }
    
    public void QCooldown()
    {
        StartCoroutine(RunCooldown(q, ability3.cooldown));
    }
    
    public void RCooldown()
    {
        StartCoroutine(RunCooldown(r, ability4.cooldown));
    }

    IEnumerator RunCooldown(Image i, float duration)
    {
        float time = 0;

        while (time < 1)
        {
            i.fillAmount = Mathf.Clamp(time, 0, 1);
            time += Time.deltaTime / duration;
            yield return null;
        }

        i.fillAmount = 1;
    }
}
