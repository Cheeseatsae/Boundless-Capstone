using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public Slider killsSlider;

    public Text killsText;
    
    public Slider aiSlider;

    public Text aiText;

    
    
    // Start is called before the first frame update
    void Start()
    {
//        manager = FindObjectOfType<CustomLobbyManager>();
//
//        manager.aiSlider = aiSlider;
//        manager.killsSlider = killsSlider;
    }

    // Update is called once per frame
    void Update()
    {
        killsText.text = killsSlider.value.ToString();
        aiText.text = aiSlider.value.ToString();

    }
}
