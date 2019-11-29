using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{
    
    public delegate void TriggerCols(Collider collider);
    
    public event TriggerCols EnterTrigger;    

    public event TriggerCols ExitTrigger;
    
    public delegate void Cols(Collision collider);
    public event Cols EnterCollider;

    public event Cols ExitCollider;

    private void OnTriggerEnter(Collider other)
    {
        EnterTrigger?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ExitTrigger?.Invoke(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        EnterCollider?.Invoke(other);
    }

    private void OnCollisionExit(Collision other)
    {
        ExitCollider?.Invoke(other);
    }
}
