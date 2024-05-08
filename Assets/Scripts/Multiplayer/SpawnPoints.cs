using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;


public class SpawnPoints : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] _spawnPositions;

    private bool _roomPropertiesIsUpdated;
    private int CurrentIndex { get => (int)PhotonNetwork.CurrentRoom.CustomProperties ["CurrentSpawnIndex"]; }
    
    public int CountFreePositions { get => _spawnPositions.Length - CurrentIndex; }
    public bool RoomPropertiesIsUpdated { get => _roomPropertiesIsUpdated; }


    private void UpdateIndex()
    {
        Hashtable properties = new Hashtable()
        {
            { "CurrentSpawnIndex", CurrentIndex + 1 }
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    }


    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        _roomPropertiesIsUpdated = true;
    }


    public void SetTransformToNextPoint(Transform transform)
    {
        int index = CurrentIndex;

        if (index == -1)
            return;

        Transform point = _spawnPositions[index];

        transform.position = point.position;
        transform.rotation = point.rotation;

        _roomPropertiesIsUpdated = false;
        UpdateIndex();
    }
}