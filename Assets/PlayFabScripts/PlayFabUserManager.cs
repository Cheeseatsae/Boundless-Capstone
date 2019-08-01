using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabUserManager : MonoBehaviour
{
    
    public void TryLogin(string playerName)
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            PlayFabSettings.TitleId = "BO149";
        
        var request = new LoginWithCustomIDRequest { CustomId = playerName, CreateAccount = true};
        
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login Success - successful API call. ID is " + result.PlayFabId);
        PlayerPrefs.SetString("PlayfabID", result.PlayFabId);
    }

    void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Login Failure - something went wrong");
        Debug.LogError(error.GenerateErrorReport());
    }
    
    public bool SetUserData(string dataname, string data)
    {
        return TrySetUserData(dataname, data);
    }
    
    private bool TrySetUserData(string dataname, string data)
    {
        bool success = false;
        
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>()
            {
                {dataname, data},
            }
        },
        result =>
        {
            Debug.Log("Successfully updated user data");
            success = true;
        },
        error =>
        {
            Debug.Log("Got error setting user data");
            Debug.LogError(error.GenerateErrorReport());
            success = false;
        });
        
        return success;
    }
    
    public string GetUserData(string dataname)
    {
        return TryGetUserData(dataname);
    }

    private string TryGetUserData(string dataname)
    {
        string r = "";
        
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
//                PlayFabId = PlayerID,
                Keys = null
            },
            result =>
            {
                Debug.Log("Got User Data:");
                if (result.Data == null || !result.Data.ContainsKey(dataname))
                {
                    Debug.Log("No " + dataname);
                    r = "null";
                }
                else
                {
                    Debug.Log(dataname + ": " + result.Data[dataname].Value);
                    r = result.Data[dataname].Value;
                }
            },
            (error) =>
            {
                Debug.Log("Got error retrieving " + dataname + " from user data:");
                Debug.LogError(error.GenerateErrorReport());
                r = "error";
            });
        
        return r;
    }
    
}