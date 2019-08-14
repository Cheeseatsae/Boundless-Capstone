using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : NetworkBehaviour
{
    public Slider killsSlider;

    public Text killsText;
    
    public Slider aiSlider;

    public Text aiText;
    // Start is called before the first frame update
    void Start()
    {
        //if //(is)
        {
            killsSlider.enabled = true;
            aiSlider.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //killsText.text = killsSlider.value.ToString();
        //aiText.text = aiSlider.value.ToString();

    }
}
