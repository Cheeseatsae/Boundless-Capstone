using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{

    public GameObject audioWindow;

    public GameObject settingsWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenAudio()
    {
        audioWindow.SetActive(true);
        settingsWindow.SetActive(false);
    }

    public void CloseAudio()
    {
        audioWindow.SetActive(false);
        settingsWindow.SetActive(true);
    }
}
