using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class UserManager : MonoBehaviour
{
    public TextMeshProUGUI userName;

    public TextMeshProUGUI userID;
    private PlayFabManager _playFabManager;
    public TextMeshProUGUI messageText;

    public static UserManager Instance;

    public void OnLoginSuccess(LoginResult result)
    {
        userName.text = result.InfoResultPayload.PlayerProfile.DisplayName;
        userID.text = result.InfoResultPayload.PlayerProfile.PlayerId;
    }
    public void ProfileButton()
    {
        var request = new GetPlayerProfileResult
        {
        };
    }
    private void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        messageText.color = Color.red;
        print(error.GenerateErrorReport());
    }
}