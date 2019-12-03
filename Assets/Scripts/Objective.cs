using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public AIManager aiManager;
    
    public Text text;
    public String[] objectiveList;
    private int _objectiveNumber = 0;
    
    public void Awake()
    {
        aiManager.EventKillNumberChanged += UpdateNumberOfEventKills;
        aiManager.KillLimitMet += ChangeObjective;
        

    }

    public void Start()
    {
        LevelManager.instance.portal.PortalActivate += ChangeObjective;
        LevelManager.instance.agni.GetComponent<Health>().BossDead += ChangeObjective;
        objectiveList[0] = "Defeat Enemies: " + "0/" + aiManager.killLimit;
        objectiveList[1] = "Activate the Portal at the Temple";
        objectiveList[2] = "Defeat Agni Baal the Living Volcano";
        objectiveList[3] = "Proceed through the Portal";
        text.text = objectiveList[0];

    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        aiManager.EventKillNumberChanged -= UpdateNumberOfEventKills;
    }

    void UpdateNumberOfEventKills(int num)
    {
        objectiveList[0] = "Defeat Enemies: " + num + "/" + aiManager.killLimit;
        text.text = objectiveList[0];
    }

    public void ChangeObjective()
    {
        //Debug.Log("run cunt");
        _objectiveNumber++;
        Jukebox.instance?.IncrementProgressionVariable();
        text.text = objectiveList[_objectiveNumber];
    }
}
