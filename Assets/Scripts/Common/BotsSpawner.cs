using Photon.Pun;
using System.Collections;
using UnityEngine;


public class BotSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarPool _carPool;
    [SerializeField] private SpawnPoints _spawnPoints;

    private bool _roomProprtiesIsUpdated;


    private IEnumerator SpawnByWaitUpdate(int amount)
    {
        _roomProprtiesIsUpdated = true;

        for (int i = 0; i < amount; i++)
        {
            yield return new WaitUntil(() => _roomProprtiesIsUpdated);

            Transform bot = _carPool.SpawnRandom();
            _spawnPoints.SetTransformToNextPoint(bot);

            _roomProprtiesIsUpdated = false;
        }
    }


    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        _roomProprtiesIsUpdated = true;
    }


    public void Spawn()
    {
        int amount = _spawnPoints.CountFreePositions;
        StartCoroutine(SpawnByWaitUpdate(amount));
    }
}