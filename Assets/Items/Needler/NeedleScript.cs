using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleScript : MonoBehaviour
{
    public ParticleSystem needle, explosion;
    
    public void Detonate()
    {
        needle.Stop();
        explosion.Play();
        Destroy(gameObject, explosion.main.duration);
    }
    
}
