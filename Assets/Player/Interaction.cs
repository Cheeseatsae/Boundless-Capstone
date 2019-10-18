using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

public class Interaction : MonoBehaviour
{

    [HideInInspector] public PlayerModel player;
    private static int currency;

    private List<Interactable> interactablesInRange = new List<Interactable>();

    private void OnTriggerEnter(Collider other)
    {
        Interactable obj = other.GetComponent<Interactable>();
        
        if (obj != null) interactablesInRange.Add(obj);
    }

    private void OnTriggerExit(Collider other)
    {
        Interactable obj = other.GetComponent<Interactable>();

        if (interactablesInRange.Contains(obj)) interactablesInRange.Remove(obj);
        
    }

    public static void ChangeMoney(int amount)
    {
        currency += amount;
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
    }
}