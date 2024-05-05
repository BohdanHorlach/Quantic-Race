using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Realtime;


public class RaceStartVote : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private RaceStarter _raceStarter;
    [SerializeField] private BotsSpawner _botsSpawner;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _readyFlagPrefab;
    [SerializeField] private Color _readyColor;
    [SerializeField] private Color _notReadyColor;
    [SerializeField] private Color _notConnectedColor;

    private List<Image> _flagsForReady;
    private int _maxPlayersCount;
    private int _readyPlayersCount;
    private int _connectedPlayersCount;


    private void Awake()
    {
		_canvas.SetActive(true);
        _flagsForReady = new List<Image>();
    }


	private void Start()
	{	
		_readyPlayersCount = 0;
	        _maxPlayersCount = PhotonNetwork.CurrentRoom.MaxPlayers;
        	_connectedPlayersCount = PhotonNetwork.CurrentRoom.PlayerCount;
	        FillFlagsForReady();
        	SetColorToConnectedPlayer();
	}


    private void FillFlagsForReady() 
    {
        for(int i = 0; i < _maxPlayersCount; i++)
        {
            GameObject flagForReady = Instantiate(_readyFlagPrefab, _canvas.transform);
            Image image = flagForReady.GetComponent<Image>();
            image.color = _notConnectedColor;
		_flagsForReady.Add(image);
        }
    }


    private void SetColorToConnectedPlayer()
    {
        for (int i = 0; i < _connectedPlayersCount; i++)
            _flagsForReady[i].color = _notReadyColor;
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        int index = Mathf.Clamp(_connectedPlayersCount - 1, 0, _maxPlayersCount - 1);
        _flagsForReady[index].color = _notConnectedColor;
        _connectedPlayersCount++;
    }


    private void InvokeRaceStarter()
    {
        _raceStarter.StartRace();

        _botsSpawner.OnAllBotsSpawned -= InvokeRaceStarter;
    }


    [PunRPC]
    private void PrepareToRace()
    {
	_canvas.SetActive(false);
        _botsSpawner.OnAllBotsSpawned += InvokeRaceStarter;
        _botsSpawner.Spawn();
    }


    [PunRPC]
    private void AddToReadyCount()
    {
        _flagsForReady[_readyPlayersCount].color = _readyColor;
        _readyPlayersCount++;

        if(_readyPlayersCount == _maxPlayersCount) 
        {
            _photonView.RPC("PrepareToRace", RpcTarget.MasterClient);
        }
    }


    public void IAmReady()
    {
        _photonView.RPC("AddToReadyCount", RpcTarget.AllBuffered);
    }
}