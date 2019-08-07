using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : NetworkBehaviour
{
    public AIManager aiManager;
    public Text text;
    
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        aiManager = CustomLobbyManager.aiManager;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Required kills: " + aiManager.numberofKills;
    }
}
