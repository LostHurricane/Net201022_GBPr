using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField]
    private Button _signInButton;

    protected override void SubscriptionElementsUI()
    {
        base.SubscriptionElementsUI();

        _signInButton.onClick.AddListener(SignIn);
    }

    private void SignIn()
    {
        var request = new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password,
        };

        PlayFabClientAPI.LoginWithPlayFab(request, ProcessResult, ProcessError);
    }

    private void ProcessResult(LoginResult result)
    {
        Debug.Log($"Sign in succces: {_username}");
        EnterInGameScene();
    }

    private void ProcessError(PlayFab.PlayFabError error)
    {
        var result = error.GenerateErrorReport();
        Debug.Log($"Error!\n{result}");

    }
}
