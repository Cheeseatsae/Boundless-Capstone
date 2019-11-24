using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimController : MonoBehaviour
{

    public Boss_Model model;

    public void StartFire()
    {
        model.fireBreath.Cast();
    }

    public void StopFire()
    {
        model.fireBreath.StopFire();
    }
    
    

}
