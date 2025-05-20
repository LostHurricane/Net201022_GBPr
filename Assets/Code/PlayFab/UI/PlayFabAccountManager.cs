using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using TMPro;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Linq;
using UnityEditor.Graphs;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _titleLabel;

    [SerializeField]
    private GameObject _newCharacterCreatePanel;

    [SerializeField]
    private Button _createCharacterButton;

    [SerializeField]
    private TMP_InputField _inputfield;

    [SerializeField]
    private List<SlotCharacterWidget> _slots;

    private string _characterName;  

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccount, OnError);
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalog, OnError);
        //PlayFabServerAPI.GetRandomResultTables(new PlayFab.ServerModels.GetRandomResultTablesRequest(), OnGetTables, OnError);


        GetCharacters();

        foreach (var slot in _slots)
        {
            slot.SlotButton.onClick.AddListener(OpenCreateNewCharacter);
        }

        _inputfield.onValueChanged.AddListener(OnNameChanged);
        _createCharacterButton.onClick.AddListener(CreateCharacter);
    }

    private void CreateCharacter()
    {
        PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
        {
            CharacterName = _characterName,
            ItemId = "character_token"
        },
        result =>
        {
            UpdateCharactersStatistics(result.CharacterId);
        },
        OnError
        );
    }

    private void UpdateCharactersStatistics(string characterId)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest()
        {
            CharacterId = characterId,
            CharacterStatistics = new Dictionary<string, int>()
            {
                {"Level", UnityEngine.Random.Range(1, 5) },
                {"Gold", UnityEngine.Random.Range(0, 900) }
            }
        },
        result =>
        {
            Debug.Log("UpdateCharactersStatistics");
            CloseCreateNewCharacter();
            GetCharacters();
        },
        OnError);
    }

    private void OnNameChanged(string changedName)
    {
        _characterName = changedName;
    }

    private void OpenCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(true);
    }

    private void CloseCreateNewCharacter()
    {
        _newCharacterCreatePanel.SetActive(false);
    }

    private void GetCharacters()
    {
        PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
            result =>
            {
                Debug.Log($"Characters count: {result.Characters.Count}");
                ShowCharactersInSlot(result.Characters);
            }, OnError);
    }

    private void ShowCharactersInSlot(List<CharacterResult> characters)
    {
        if (characters.Count == 0) 
        {
            foreach (var slot in _slots)
            {
                slot.ShowEmptySlot();
            }
            return; 
        }
        else if (characters.Count <= _slots.Count)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                var currentSlot = _slots[i];
                var character = characters[i];
                
                PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
                {
                    CharacterId = character.CharacterId,
                },
                result =>
                {
                    var level = result.CharacterStatistics["Level"].ToString();
                    var gold = result.CharacterStatistics["Gold"].ToString();
                    var slot = _slots.First(s => !s.IsFilled);
                    slot.ShowInfoCharacterSlot( character.CharacterName, level, gold);

                    if (i == characters.Count)
                    {
                        foreach (var cSlot in _slots)
                        {
                            if (!cSlot.IsFilled)
                            {
                                cSlot.ShowEmptySlot();
                            }
                        }
                    }
                }, OnError);

            }

            

            //if (characters.Count != _slots.Count)
            //{
            //    var diff = _slots.Count - characters.Count;
            //    for (var j = diff - 1; j < _slots.Count; j++)
            //    {
            //        _slots[j].ShowEmptySlot();
            //    }
            //}
        }
        else
        {
            Debug.LogError("add slots for characters");
        }


        
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
