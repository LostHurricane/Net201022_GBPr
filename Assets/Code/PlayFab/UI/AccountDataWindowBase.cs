using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class AccountDataWindowBase : MonoBehaviour
{
    [SerializeField] TMP_InputField _userNameField;
    [SerializeField] TMP_InputField _passwordField;


    protected string _username;
    protected string _password;

    private void Start()
    {
        SubscriptionElementsUI();
    }

    protected virtual void SubscriptionElementsUI()
    {
        _userNameField.onValueChanged.AddListener(UpdateUsername);
        _passwordField.onValueChanged.AddListener(UpdatePassword);
    }

    private void UpdateUsername(string username)
    {
        _username = username;
    }

    private void UpdatePassword(string password)
    {
        _password = password;
    }

    private void OnDestroy()
    {
        _userNameField.onValueChanged.RemoveListener(UpdateUsername);
        _passwordField.onValueChanged.RemoveListener(UpdatePassword);
    }

    protected void EnterInGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
