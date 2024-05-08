using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BotsSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private CarPool _carPool;
    [SerializeField] private SpawnPoints _spawnPoints;
    [SerializeField] private RaceStarter _raceStarter;
    [SerializeField] private WayPoint _firstWayPoint;


    public event Action OnAllBotsSpawned;


    private GameObject SpawnBot()
    {
        GameObject car = _carPool.GetRandomCar();
        GameObject bot = PhotonNetwork.InstantiateRoomObject(car.name, car.transform.position, car.transform.rotation);
        _spawnPoints.SetTransformToNextPoint(bot.transform);

        return bot;
    }


    [PunRPC]
    private void AddWayPointToBotRPC(int viewID)
    {
        PhotonView botView = PhotonNetwork.GetPhotonView(viewID);
        BotsCarMovement botsCarMovement = botView.GetComponentInChildren<BotsCarMovement>();
        botsCarMovement.SetWayPoint(_firstWayPoint);
        botsCarMovement.enabled = true;
    }


    private void AddWayPointToBot(GameObject bot)
    {
        PhotonView botView = bot.GetComponent<PhotonView>();
        _photonView.RPC("AddWayPointToBotRPC", RpcTarget.All, botView.ViewID);
    }


    private void AddToRaceStarter(GameObject bot)
    {
        CarMovementMultiplayer carMovement = bot.GetComponent<CarMovementMultiplayer>();
        _raceStarter.AddToListCars(carMovement);
    }


    private IEnumerator SpawnByWaitUpdate(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitUntil(() => _spawnPoints.RoomPropertiesIsUpdated);

            GameObject bot = SpawnBot();
            AddWayPointToBot(bot);
            AddToRaceStarter(bot);
        }

        OnAllBotsSpawned?.Invoke();
    }



    public void Spawn()
    {
        int amount = _spawnPoints.CountFreePositions;
        StartCoroutine(SpawnByWaitUpdate(amount));
    }
}