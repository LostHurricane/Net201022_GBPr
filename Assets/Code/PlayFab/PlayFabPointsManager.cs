using Photon.Pun;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System;
using UnityEditor;
using PlayFab.ClientModels;
using UnityEngine.Events;
using UnityEditor.VersionControl;

namespace Lesson7
{

    public class PlayFabPointsManager
    {
        private const string TitleId = "C822B";
        private const string AuthGuidKey = "auth_guid_key_l7";
        private const string _pointsStatID = "points_stat_ID";

        public UnityEvent<string> OnLoginSuccesEvent;
        public UnityEvent<string> OnPlayFabErrorEvent;

        public int Points { get; private set; }

        private bool _generateNewId = false;
        private bool _needCreation;
        private string _loginId;

        public PlayFabPointsManager()
        {
            //PlayerPrefs.DeleteKey(AuthGuidKey);
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = TitleId;
            }

            _needCreation = PlayerPrefs.HasKey(AuthGuidKey);
            if (!_needCreation)
            {
                _loginId = Guid.NewGuid().ToString();

                Debug.Log($"play fab New ID generated\n{_loginId}");
            }
            else
            {
                _loginId = PlayerPrefs.GetString(AuthGuidKey);
            }

            LogIn();
        }


        public void LogIn()
        {
            if (PlayFabClientAPI.IsClientLoggedIn())
                return;

            var request = new LoginWithCustomIDRequest
            {
                CustomId = _loginId,
                CreateAccount = !_needCreation
            };

            PlayFabClientAPI.LoginWithCustomID(request,
                result =>
                {
                    PlayerPrefs.SetString(AuthGuidKey, _loginId);
                    ProccesLoginSucces(result);

                    if (!_needCreation)
                    {
                        Points = 500;
                        SetUserData(_pointsStatID, Points.ToString());
                        Debug.Log($"points seted as {Points}");

                    }
                    else
                    {
                        Points = int.Parse(GetUserData(_pointsStatID));
                        Debug.Log($"points loaded {Points}");

                    }

                },
                ProccesError);
        }

        public void AddPoints(int point)
        {
            Points += point;
            SetUserData(_pointsStatID, Points.ToString());
        }

        private void ProccesLoginSucces(LoginResult result)
        {
            var message = $"Complete Login";
            Debug.Log(message);
            OnLoginSuccesEvent?.Invoke(message);
        }

        private void SetUserData(string key, string value)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
            {
                Data = new Dictionary<string, string>
            {
                { key, value }
            }
            },
            result =>
            {
                Debug.Log($"user Data {_pointsStatID} is set");
            },
            ProccesError);
        }

        private string GetUserData(string key)
        {
            string resultValue = null;

            PlayFabClientAPI.GetUserData(new()
            {
                //PlayFabId = _loginId
            },
            result =>
            {
                if (result.Data.ContainsKey(key))
                {
                    Debug.Log($"key {key}, result {result.Data[key].Value}");
                    resultValue = result.Data[key].Value;
                }
            },
            ProccesError);

            return resultValue;
        }


        private void ProccesError(PlayFabError error)
        {
            var errorMessge = error.GenerateErrorReport();
            Debug.Log(errorMessge);
            OnPlayFabErrorEvent?.Invoke(errorMessge);
        }
    }
}