using UnityEngine;
using Photon.Pun;


public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarPool _carPool;
    [SerializeField] private SpawnPoints _spawnPoints;
    [SerializeField] private RaceStarter _raceStarter;
    [SerializeField] private GameObject _roomCamera;
    [SerializeField] private Skidmarks _skidmarksController;

    private void Start()
    {
        int indexCar = SaveManager.instance.CurrCar;
        SpawnPlayer(indexCar);
    }


    private GameObject Spawn(int indexCar)
    {
        GameObject player = _carPool.Spawn(indexCar);
        _spawnPoints.SetTransformToNextPoint(player.transform);

        return player;
    }


    private void AddPlayerToRaceStarter(GameObject player)
    {
        CarMovement carMovement = player.GetComponent<CarMovement>();
        _raceStarter.AddToListCars(carMovement);
    }


    private void SpawnPlayer(int indexCar)
    {
        _roomCamera.SetActive(false);

        GameObject player = Spawn(indexCar);
        AddPlayerToRaceStarter(player);
    }
}