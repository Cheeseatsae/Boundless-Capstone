using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AIBaseModel : MonoBehaviour
{
    
    public GameObject target;
    private float maxDistance = Mathf.Infinity;
    public Rigidbody rb;
    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Targeting();
    }

    private void ReceiveNewTarget()
    {
        
    }
    
    private void Targeting()
    {
        if (LevelManager.instance.player != null)
        {
            target = LevelManager.instance.player;
        }
        

    }
}