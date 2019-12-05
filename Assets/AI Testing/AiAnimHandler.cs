using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAnimHandler : MonoBehaviour
{

    public GroundAI_Model model;

    public Health health;
    // Start is called before the first frame update

    public void RunFire()
    {
        model.Fire();
    }

    public void StartMoving()
    {
        model.anim.SetTrigger("BackToMovement");
        model.navmesh.isStopped = false;
        
        model.ranged = false;
    }

    public void Death()
    {
        Destroy(model.gameObject);
    }

    public void Nova()
    {
        model.nova.Play();
    }
}
