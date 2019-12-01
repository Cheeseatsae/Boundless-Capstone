using System;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [HideInInspector] public PlayerModel player;
    private static int currency;

    public List<Interactable> interactablesInRange = new List<Interactable>();

    public delegate void CurrencyUpdate();
    public static CurrencyUpdate OnCurrencyUpdate;
    
    private void OnTriggerEnter(Collider other)
    {
        Interactable obj = other.GetComponent<Interactable>();

        if (obj == null) return;
        if (obj.active)
        {
            LevelManager.instance.uiManager.interaction.SetActive(true);
        }
        if (interactablesInRange.Contains(obj)) return;
        
        interactablesInRange.Add(obj);
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable obj = other.GetComponent<Interactable>();
        LevelManager.instance.uiManager.interaction.SetActive(false);

        if (interactablesInRange.Contains(obj)) interactablesInRange.Remove(obj);
        
    }

    public static void ChangeMoney(int amount)
    {
        currency += amount;
        OnCurrencyUpdate?.Invoke();
    }
    
    public static int GetMoney()
    {
        return currency;
    }
    
    public void AttemptInteract()
    {
        if (interactablesInRange.Count <= 0) return;
        
        if (interactablesInRange.Count == 1)
        {
            SortType(interactablesInRange[0]);
            return;
        }

        float shortestDist = float.MaxValue;
        Interactable toActivate = null;
        
        foreach (Interactable i in interactablesInRange)
        {
            float d = Vector3.Distance(i.gameObject.transform.position, transform.position);
            if (d < shortestDist)
            {
                shortestDist = d;
                toActivate = i;
            }
        }
        
        SortType(toActivate);
    }

    // interacts correctly based on what the object is
    private void SortType(Interactable i)
    {
        if (i.GetComponent<Pickup>())
        {
            i.Interact(player.gameObject);
        } 
        else if (i.GetComponent<ItemBox>())
        {
            i.Interact(currency);
        }
        else
        {
            i.Interact();
        }
        
        interactablesInRange.Remove(i);
    }
}