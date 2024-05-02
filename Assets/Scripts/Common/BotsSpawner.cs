using Photon.Pun;
using System.Collections;
using UnityEngine;


public class BotSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarPool _carPool;
    [SerializeField] private SpawnPoints _spawnPoints;
    [SerializeField] private RaceStarter _raceStarter;
    [SerializeField] private WayPoint _firstWayPoint;

    private bool _roomProprtiesIsUpdated;


    private GameObject SpawnBot()
    {
        GameObject bot = _carPool.SpawnRandom();
        _spawnPoints.SetTransformToNextPoint(bot.transform);

        return bot;
    }


    private void AddWayPointToBot(GameObject bot)
    {
        BotsCarMovement botsCarMovement = bot.GetComponentInChildren<BotsCarMovement>();
        botsCarMovement.SetWayPoint(_firstWayPoint);
        botsCarMovement.enabled = true;
    }


    private void AddToRaceStarter(GameObject bot)
    {
        CarMovement carMovement = bot.GetComponent<CarMovement>();
        _raceStarter.AddToListCars(carMovement);
    }


    private IEnumerator SpawnByWaitUpdate(int amount)
    {
        _roomProprtiesIsUpdated = true;

        for (int i = 0; i < amount; i++)
        {
            yield return new WaitUntil(() => _roomProprtiesIsUpdated);

            GameObject bot = SpawnBot();
            AddWayPointToBot(bot);
            AddToRaceStarter(bot);

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