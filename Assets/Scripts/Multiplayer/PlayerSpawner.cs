using UnityEngine;
using Photon.Pun;


public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private PositionCalculator _positionCalculator;
    [SerializeField] private SceneNavigator _sceneNavigator;
    [SerializeField] private SpawnPoints _spawnPoints;
    [SerializeField] private RaceStarter _raceStarter;
    [SerializeField] private GameObject _roomCamera;


    private void Start()
    {
        GameObject selectedCar = UserDataManager.selectedGameOptionsSO.selectedCarInformationSO.multiplayerUserPrefab;
        SpawnPlayer(selectedCar);
    }


    private GameObject Spawn(GameObject prefab)
    {
        GameObject player = PhotonNetwork.Instantiate(prefab.name, prefab.transform.position, prefab.transform.rotation);
        _spawnPoints.SetTransformToNextPoint(player.transform);

        return player;
    }


    private void AddPlayerToRaceStarter(GameObject player)
    {
        CarMovementMultiplayer carMovement = player.GetComponent<CarMovementMultiplayer>();
        _raceStarter.AddToListCars(carMovement);
    }


    private void InitializeDisplayUIPlayer(GameObject player)
    {
        DisplayUIMultiplayer display = player.GetComponent<DisplayUIMultiplayer>();
        
        display.Initialize(_positionCalculator, _sceneNavigator);
    }


    private void SpawnPlayer(GameObject selectedCar)
    {
        _roomCamera.SetActive(false);

        GameObject player = Spawn(selectedCar);
        AddPlayerToRaceStarter(player);
        InitializeDisplayUIPlayer(player);
    }
}