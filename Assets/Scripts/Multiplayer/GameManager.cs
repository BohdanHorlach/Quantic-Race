using UnityEngine;
using Photon.Pun;


//GameManager ����� ���������� �����
//����� ������ �� PlayerSpawner
//����� ���� ����� �����, ����� ������� ���������� ���������, SpawnPointController
//� ����� ���� ���������� ����� ����� ������, [SerializeField] private Transform[] _spawnPoint;
//� � ����� �������� �� ����� ���������, [SerializeField] private SpawnPointController
//��� ��������� ��� ���� ����� GetNextRandomSpawnPoint ���� ���� ��������� �������� �����.
//������� ������� ����� �� �� ����� �� ��������������� ����� �� �����������.
//��� ����� ����� ��� ����� ������������ ������ List<Transform> �� ��������� ���� � �����,
//� ����� ����������� �������� �������� ����� � ����� ������.
//�� ������ �������� ���� ������ �� �������� ����, ��� ���� ����� ������ ��� ���� �������, ��� � ������� ��� ���������.
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