using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePortal : Interactable
{
    
    public delegate void Portal();
    public event Portal PortalActivate;

    
    public GameObject portal;
    public GameObject aura;
    public bool firstActive = false;
    public override void Interact()
    {
        if (active)
        {
            base.Interact();
            PortalActivate?.Invoke();
            aura.SetActive(false);
            active = false;

        }
    }

    public void Start()
    {
        LevelManager.aiManager.KillLimitMet += Objective1Complete;
        LevelManager.instance.agni.GetComponent<Health>().BossDead += OpenPortal;
        active = false;
    }

    public void Objective1Complete()
    {
        active = true;
        aura.SetActive(true);
    }

    public void OpenPortal()
    {
        portal.SetActive(true);
    }
}
