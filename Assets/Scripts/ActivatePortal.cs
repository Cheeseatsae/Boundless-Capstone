using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePortal : Interactable
{
    
    public delegate void Portal();
    public event Portal PortalActivate;

    public bool firstActive = false;
    public override void Interact()
    {
        if (active)
        {
            base.Interact();
            PortalActivate?.Invoke();

        }
    }

    public void Start()
    {
        LevelManager.aiManager.KillLimitMet += Objective1Complete;
        active = false;
    }

    public void Objective1Complete()
    {
        active = true;
    }
}
