using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayFabLogin : MonoBehaviour
{
    const string TitleId = "C822B";
    private const string AuthGuidKey = "auth_guid_key";

    //public UnityEvent <LoginResult> OnLoginSuccesEvent;
    //public UnityEvent <PlayFabError> OnLoginErrorEvent;
    
    
    public UnityEvent <string> OnLoginSuccesEvent;
    public UnityEvent <string> OnLoginErrorEvent;
    private bool _needCreation;
    private string _id;

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = TitleId;
        }

        _needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        _id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());


    }


    public void LogIn()
    {
        if (PlayFabClientAPI.IsClientLoggedIn())
            return;

        var request = new LoginWithCustomIDRequest
        {
            CustomId = _id,
            CreateAccount = !_needCreation
        };

        PlayFabClientAPI.LoginWithCustomID(request, 
            result =>
            {
                PlayerPrefs.SetString(AuthGuidKey, _id);
                OnLoginSucces(result);
            },
            OnLoginError);
    }

    private void OnLoginSucces(LoginResult result)
    {
        var message = $"Complete Login";

        Debug.Log(message);
        OnLoginSuccesEvent?.Invoke(message);

    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessge = error.GenerateErrorReport();
        Debug.Log(errorMessge);
        OnLoginErrorEvent?.Invoke(errorMessge);
    }
}
