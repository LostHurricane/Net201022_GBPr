using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
//using PlayFab.PfEditor.EditorModels;

public class CreateAccountWindow : AccountDataWindowBase
{
    [SerializeField] TMP_InputField _mailField;
    [SerializeField] Button _createAccountButton;

    private string _mail;

    protected override void SubscriptionElementsUI ()
    {
        base.SubscriptionElementsUI ();

        _mailField.onValueChanged.AddListener(UpdateEMail);
        _createAccountButton.onClick.AddListener(CreateAccount);
    }

    private void CreateAccount()
    {
        var registrationRequest = new RegisterPlayFabUserRequest
        {
            Email = _mail,
            Username = _username,
            Password = _password,
        };

        PlayFabClientAPI.RegisterPlayFabUser(registrationRequest, ProcessResult, ProcessError);

    }

    private void ProcessResult(RegisterPlayFabUserResult result)
    {
        Debug.Log ($"Registration succces: {_username}");
        EnterInGameScene();
    }

    private void ProcessError(PlayFab.PlayFabError error)
    {
        var result = error.GenerateErrorReport();
        Debug.Log($"Error!\n{result}");

    }

    private void UpdateEMail(string mail)
    {
        _mail = mail;
    }
}
