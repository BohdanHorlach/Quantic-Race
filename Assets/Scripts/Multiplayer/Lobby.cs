using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using UnityEngine.UI;


public class Lobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _createInput;
    [SerializeField] private TMP_InputField _joinInput;
    [SerializeField] private TextMeshProUGUI _textToSlider;
    [SerializeField] private Slider _slider;
    [Space]
    [SerializeField, Range(2, 6)] private int _maxPlayers = 6;
    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private string _gameScene = "SampleScene";
    [Space]
    [SerializeField] private RoomItem _roomItemPrefab;
    [SerializeField] private Transform _contentObject;

    private List<RoomItem> _roomItemsList = new List<RoomItem>();


    private void Start()
    {
        PhotonNetwork.JoinLobby();
        ReadSliderValue();
    }


    public void ReadSliderValue()
    {
        _maxPlayers = (int)_slider.value;
        _textToSlider.text = _maxPlayers.ToString();
    }


    public void CreateRoom()
    {
        if (_createInput.text.Length >= 1)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = _maxPlayers;
            
            Hashtable indexSpawn = new Hashtable {
                { "CurrentSpawnIndex", 0 }
            };

            roomOptions.CustomRoomProperties = indexSpawn;
            roomOptions.CustomRoomPropertiesForLobby = new string[] { "CurrentSpawnIndex" };

            PhotonNetwork.CreateRoom(_createInput.text, roomOptions);
        }
    }

    public override void OnJoinedRoom()
    {
        _menuManager.LoadPhotonLevel(_gameScene);
    }

    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
        UpdateRoomList(_roomList);
    }

    void UpdateRoomList(List<RoomInfo> _list)
    {
        foreach (RoomItem item in _roomItemsList)
        {
            Destroy(item.gameObject);
        }
        _roomItemsList.Clear();

        foreach (RoomInfo _room in _list)
        {
            RoomItem _newRoom = Instantiate(_roomItemPrefab, _contentObject);
            _newRoom.SetRoomName(_room.Name);
            _roomItemsList.Add(_newRoom);
        }
    }

    public void JoinRoom(string _name)
    {
        PhotonNetwork.JoinRoom(_name);
    }

    public void JoinRoomBtn()
    {
        PhotonNetwork.JoinRoom(_joinInput.text);
    }
}