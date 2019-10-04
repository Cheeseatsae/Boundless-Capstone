using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Wallet : MonoBehaviour
{

    public int currency;

    private List<Pickup> pickupsInRange = new List<Pickup>();

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Pickup obj = other.GetComponent<Pickup>();
        
        if (obj != null) pickupsInRange.Add(obj);
    }

    private void OnTriggerExit(Collider other)
    {
        Pickup obj = other.GetComponent<Pickup>();

        if (pickupsInRange.Contains(obj)) pickupsInRange.Remove(obj);
    }
}
