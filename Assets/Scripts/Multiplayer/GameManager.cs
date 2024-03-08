using UnityEngine;
using Photon.Pun;


//GameManager трохи некоректна назва
//Можна змінити на PlayerSpawner
//Також щодо точок спану, можна зробити додатковий компонент, SpawnPointController
//у якому буде зберігатись масив точок спавну, [SerializeField] private Transform[] _spawnPoint;
//а в цьому зберегти на нього посилання, [SerializeField] private SpawnPointController
//далі викликати при спані метод GetNextRandomSpawnPoint який буде повертати наступну точку.
//Головне зробити умову що ця точка не використовується перед її поверненням.
//Для цього можеш при старті ініціалізувати список List<Transform> та повертати саме з ньогу,
//а перед поверненням видаляти отриману точку з цього списку.
//Ця задача впринципі може лежати на розробці трас, але якщо хочеш можешь для себе зробити, там з Микитою вже домовишся.
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _roomCamera;
    [SerializeField] Skidmarks _skidmarksController;
    [SerializeField] Animator _anim;

    void Start()
    {
        //TODO: Spawn Player
        SpawnPlayer();

        _anim.SetBool("start", true);
    }

    void SpawnPlayer()
    {
        _roomCamera.SetActive(false);

        GameObject _player = PhotonNetwork.Instantiate(_playerPrefab.name, _playerPrefab.transform.position, _playerPrefab.transform.rotation);
        WheelSkidInitializer wheelSkidInitializer = _player.GetComponent<WheelSkidInitializer>();

        if (wheelSkidInitializer != null)
            wheelSkidInitializer.Initialize(_skidmarksController);

        Debug.Log("Spawned Player", _player);
    }
}