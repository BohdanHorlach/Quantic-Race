using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private CarPool _carPool;
    [SerializeField] private SpawnPoints _spawnPoints;
    [SerializeField] private GameObject _roomCamera;
    [SerializeField] private Skidmarks _skidmarksController;

    private void Start()
    {
        int indexCar = SaveManager.instance.CurrCar;
        SpawnPlayer(indexCar);
    }


    private void SpawnPlayer(int indexCar)
    {
        _roomCamera.SetActive(false);

        GameObject car = _carPool.GetCarFromIndex(indexCar);
        GameObject player = PhotonNetwork.Instantiate(car.name, car.transform.position, car.transform.rotation);
        _spawnPoints.SetTransformToNextPoint(player.transform);

        Debug.Log("Spawned Player", player);
    }
}