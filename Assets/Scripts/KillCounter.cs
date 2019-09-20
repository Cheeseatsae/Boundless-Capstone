using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    public AIManager aiManager;
    public Text text;
    
    private void Awake()
    {
        text = GetComponent<Text>();
        text.text = "";
        aiManager.EventKillNumberChanged += UpdateNumberOfEventKills;
    }

    private void OnDestroy()
    {
        aiManager.EventKillNumberChanged -= UpdateNumberOfEventKills;
    }

    void UpdateNumberOfEventKills(int num)
    {
        text.text = "Required kills: " + num;
    }
}
