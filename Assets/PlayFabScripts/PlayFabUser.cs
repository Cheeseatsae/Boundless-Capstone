using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayFabUser : MonoBehaviour
{
    private PlayFabUserManager playfab;
    public string playerID;
    
    private void Start()
    {
        playfab = GetComponent<PlayFabUserManager>();

        playerID = GetPlayerID();
        playfab.TryLogin(playerID);

        AIManager.OnLevelEnd += SendPlayTime;
    }

    private void OnDestroy()
    {
        AIManager.OnLevelEnd -= SendPlayTime;
    }

    private void SendPlayTime()
    {
        playfab.SetUserData("PlayTime", Time.time.ToString());
    }

    public string GetPlayerID()
    {
        string playerid = "";
        
        if (PlayerPrefs.HasKey("LogonName"))
        {
            playerid = PlayerPrefs.GetString("LogonName");
        }
        else
        {

            string ID = "";

            for (int i = 0; i < 8; i++)
            {
                ID += Random.Range(0, 9).ToString();
            }

            playerid = "Player" + ID;
            PlayerPrefs.SetString("LogonName", playerid);
        }

        Debug.Log("Player name is " + playerid);
        return playerid;
    }
    
}
