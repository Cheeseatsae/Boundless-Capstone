using System;
using UnityEngine;

public class Pincushion : MonoBehaviour
{
    private GameObject needle;

    private void Start()
    {
        needle = Resources.Load("Assets/Items/Needler/Needle.prefab") as GameObject;
    }
    
    
}
