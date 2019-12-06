using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundSettings : MonoBehaviour
{

    public Slider musicSlider;

    public Slider sfxSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Jukebox.MusicVolume = musicSlider.value;
        Jukebox.SFXVolume = sfxSlider.value;
    }
}
