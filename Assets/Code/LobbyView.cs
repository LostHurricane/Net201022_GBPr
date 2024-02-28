using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour//, ILobbyCallbacks
{
    [SerializeField] private GameObject _lobbyPanel;
    [SerializeField] private GameObject _infoTextPrefab;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private RectTransform _parentRoomList;

    private Dictionary<string, RoomInfo> cashedRoomList = new Dictionary<string, RoomInfo>();
    private List<GameObject> roomTextsList = new List<GameObject>();

  

    public void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby lobbyView");
        _lobbyPanel.SetActive(true);

        cashedRoomList.Clear();

    }

    public void OnLeftLobby()
    {
        cashedRoomList.Clear();
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
      
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCashedRoomList(roomList);
    }


    private void UpdateCashedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                cashedRoomList.Remove(info.Name);
            }
            else
            {
                cashedRoomList[info.Name] = info;
            }
        }

        DisplayRoomList();
    }

    private void DisplayRoomList()
    {
        foreach(var go in roomTextsList)
        {
            Destroy(go);
        }

        foreach (var roomData in cashedRoomList)
        {
            var str = $"{roomData.Key} - current {roomData.Value.PlayerCount},  max {roomData.Value.MaxPlayers}";

            var prot = Instantiate(_infoTextPrefab, _parentRoomList).GetComponent<TMP_Text>();
            prot.text = str;
        }
    }
}
