using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using TMPro;
using PlayFab.ClientModels;
using System;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _titleLabel;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalog, OnError);
        PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest(), OnGetTables, OnError);

    }

    private void OnGetTables(PlayFab.ServerModels.GetRandomResultTablesResult result)
    {
        Debug.Log("OnGetTables succses");
    }

    private void OnGetCatalog(GetCatalogItemsResult result)
    {
        Debug.Log("GetCatalogItemsRequest succses");

        ShowItems(result.Catalog);
    }

    private void ShowItems(List<CatalogItem> catalog)
    {
       foreach (var item in catalog)
        {
            Debug.Log($"Item {item.DisplayName} ID:{item.ItemId} cost {item.RealCurrencyPrices}");

        }
    }

    private void OnGetAccount(GetAccountInfoResult result)
    {
        _titleLabel.text = $"id: {result.AccountInfo.PlayFabId}";
    }

    private void OnError(PlayFabError error)
    {
        var errorMessge = error.GenerateErrorReport();
        Debug.Log(errorMessge);
    }
}
