using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;


public class SpawnPoints : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] _spawnPositions;

    private int CurrentIndex { get => (int)PhotonNetwork.CurrentRoom.CustomProperties ["CurrentSpawnIndex"]; }
    public int CountFreePositions { get => _spawnPositions.Length - CurrentIndex; }


    private void Start()
    {
        Debug.Log(CurrentIndex);
    }


    private void UpdateIndex()
    {
        Hashtable properties = new Hashtable()
        {
            { "CurrentSpawnIndex", CurrentIndex + 1 }
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    }


    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log(propertiesThatChanged["CurrentSpawnIndex"]);
    }


    public void SetTransformToNextPoint(Transform transform)
    {
        int index = CurrentIndex;

        if (index == -1)
            return;

        Transform point = _spawnPositions[index];

        transform.position = point.position;
        transform.rotation = point.rotation;

        UpdateIndex();

        Debug.Log(CurrentIndex);
    }
}