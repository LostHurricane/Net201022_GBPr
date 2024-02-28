using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using System.Linq;

public class ConnectAndJoinRandomLb : MonoBehaviour,
    IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
{
    [SerializeField]
    private ServerSettings _settings;

    [SerializeField]
    TMP_Text _stateUIText;

    [SerializeField]
    private LobbyView _lobbyView;

    private LoadBalancingClient _lbc;

    private const string GAME_MODE_KEY = "gm"; 
    private const string AI_MODE_KEY = "ai";

    private const string MAP_PROP_KEY = "C0";
    private const string GOLD_PROP_KEY = "C1";
    
    private TypedLobby _sqlLobby = new TypedLobby ("customSqlLobby", LobbyType.SqlLobby);

    private void Start()
    {
        _lbc = new LoadBalancingClient();
        _lbc.AddCallbackTarget(this);
    }

    public void Connect()
    {
        

        _lbc.ConnectUsingSettings(_settings.AppSettings);
        
    }

    public void CreateRoom()
    {
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 12,
            PublishUserId = true,
            //CustomRoomPropertiesForLobby = new[] { MAP_PROP_KEY , GOLD_PROP_KEY },
            //CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { {GOLD_PROP_KEY, 400 }, {MAP_PROP_KEY, "Map3" } },
        };


        var enterRoomParams = new EnterRoomParams
        {
            RoomName = "new room",
            RoomOptions = roomOptions,
            Lobby = default
        };

        _lbc.OpCreateRoom(enterRoomParams);
    }

    // Update is called once per frame
    void Update()
    {
        if (_lbc == null)
            return;

        _lbc.Service();

        var state = _lbc.State.ToString();
        _stateUIText.text = state;
    }

    private void OnDestroy()
    {
        _lbc.RemoveCallbackTarget(this);

    }

    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master");
        //_lbc.OpJoinRandomRoom();
        _lbc.OpJoinLobby(_sqlLobby);
    }

    

    public void OnCreatedRoom()
    {
        Debug.Log("Room created");

    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnDisconnected(DisconnectCause cause)
    {
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnJoinedLobby()
    {
        Debug.Log("Lobby joined");
        _lobbyView.OnJoinedLobby();

    }

    public void OnJoinedRoom()
    {
        Debug.Log("Room joined");

    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed");
        _lbc.OpCreateRoom(new EnterRoomParams());
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        

    }

    public void OnLeftLobby()
    {
        _lobbyView.OnLeftLobby();
    }

    public void OnLeftRoom()
    {
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        _lobbyView.OnLobbyStatisticsUpdate(lobbyStatistics);
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _lobbyView.OnRoomListUpdate(roomList);
    }

    
}
