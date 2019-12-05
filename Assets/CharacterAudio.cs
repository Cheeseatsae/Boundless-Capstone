using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public List<StudioEventEmitter> sounds; 
    
    // Start is called before the first frame update
    void Awake()
    {
        sounds.AddRange(GetComponentsInChildren<StudioEventEmitter>());
    }

    public void PlaySound(int sound)
    {
        sounds[sound].Play(Jukebox.SFXVolume);
    }

    public void StopSound(int sound, bool fade)
    {
        sounds[sound].AllowFadeout = fade;
        sounds[sound].Stop();
    }
}
