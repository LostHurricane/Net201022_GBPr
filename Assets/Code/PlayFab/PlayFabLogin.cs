using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        SetUserData(result.PlayFabId);
        //MakePurchase();
        GetInventory();
    }

    private void GetInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result => ShowInventory(result.Inventory), OnLoginError);

    }

    private void ShowInventory(List<ItemInstance> inventory)
    {
        var first = inventory.First();
        Debug.Log($"inventory, item {first.DisplayName}, id {first.ItemInstanceId}");
        UseItem(first.ItemInstanceId);
    }

    private void UseItem(string itemInstanceId)
    {
        PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
        {
            ItemInstanceId = itemInstanceId,
            ConsumeCount = 1
        }, 
        result => 
        {
            Debug.Log("Item COnsumed");
        }, OnLoginError);
    }

    private void MakePurchase()
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            CatalogVersion = "geometricduelist_catalog",
            ItemId = "optionalskin_id",
            Price = 100,
            VirtualCurrency = "BC"
        },
        result =>
        {
            Debug.Log("MakePurchase succes");
        },
        OnLoginError
        );
    }

    private void SetUserData(string playFabId)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>
            {
                {"time_receve_daily_reward", DateTime.UtcNow.ToString() }
            }
        },
        result =>
        {
            Debug.Log("Set user Data");
            GetUserData(playFabId, "time_receve_daily_reward");
        }, OnLoginError
        );
    }

    private void GetUserData(string playFabId, string v)
    {
        PlayFabClientAPI.GetUserData(new()
        {
            PlayFabId = playFabId
        },
        result =>
        {
            Debug.Log("Result");
            if (result.Data.ContainsKey(v)) 
            {
                Debug.Log ($"key {v}, result {result.Data[v].Value}");
            }
        },
        OnLoginError
        );
    }

    private void OnLoginError(PlayFabError error)
    {
        var errorMessge = error.GenerateErrorReport();
        Debug.Log(errorMessge);
        OnLoginErrorEvent?.Invoke(errorMessge);
    }
}
