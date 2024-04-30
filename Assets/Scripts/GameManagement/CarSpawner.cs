using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSpawner : MonoBehaviour
{
    private List<int> freeSpawnPoints;
    [SerializeField] private WayPoint firstWayPoint;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private int coutndownTimeSeconds = 3;

    private int _currentSpawnPoint;
    private List<GameObject> bots;

    private void Awake()
    {
        bots = new List<GameObject>();
        InitFreeSpawnPoints();
    }

    private void OnEnable()
    {
        SpawnPlayer();
        SpawnBots();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the RaceScene is loaded
        if (scene.name == SceneNavigator.TRACK1_SINGLE_PLAYER)
        {
            StartCoroutine(StartCountdown());
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator StartCountdown()
    {
        string goText = "Go!!!";

        PlayerInputDetector.PauseInput();
        countdownText.enabled = true;
        while (coutndownTimeSeconds > 0)
        {
            Debug.Log("Countdown: " + coutndownTimeSeconds);
            countdownText.text = coutndownTimeSeconds.ToString();
            yield return new WaitForSeconds(1);
            coutndownTimeSeconds--;
        }
        countdownText.text = goText;
        yield return new WaitForSeconds(1);
        Debug.Log("Go!");
        countdownText.enabled = false;
        PlayerInputDetector.ResumeInput();
        AllowBotsMove();

    }

    private void InitFreeSpawnPoints()
    {
        freeSpawnPoints = new List<int>();
        _currentSpawnPoint = 0;
        for (int i = 0; i < UserDataManager.selectedGameOptionsSO.spawnPoints.Length; ++i)
        {
            freeSpawnPoints.Add(i);
        }
        for (int i = freeSpawnPoints.Count - 1; i > 0; i--)
        {
            var k = Random.Range(0, i + 1);
            var value = freeSpawnPoints[k];
            freeSpawnPoints[k] = freeSpawnPoints[i];
            freeSpawnPoints[i] = value;
        }
    }

    private int GetFreeRandomSpawnPoint()
    {
        int position = freeSpawnPoints[_currentSpawnPoint];
        _currentSpawnPoint += 1;
        return position;
    }


    private GameObject Spawn(GameObject car)
    {
        int spawnPositionIndex = GetFreeRandomSpawnPoint();
        Transform spawnPositionTransform = UserDataManager.selectedGameOptionsSO.spawnPoints[spawnPositionIndex];
        Vector3 position = spawnPositionTransform.position;
        Quaternion rotation = spawnPositionTransform.rotation;

        GameObject carInstance = Instantiate(car, position, rotation);

        Debug.Log("Spawning car at: " + position);

        return carInstance;
    }

    private void SpawnPlayer()
    {

        GameObject car = UserDataManager.selectedGameOptionsSO.selectedCarInformationSO.singlePlayerUserPrefab;
        Spawn(car);
    }

    private void SpawnBots()
    {
        CarInformationSO[] botsInfoSO = UserDataManager.selectedGameOptionsSO.carInformationSOArray;
        while (_currentSpawnPoint < freeSpawnPoints.Count)
        {
            GameObject car = botsInfoSO[Random.Range(0, botsInfoSO.Length)].singlePlayerBotPrefab;

            BotInit(car);
            GameObject carInstance = Spawn(car);

            bots.Add(carInstance);


            //Debug.Log(freeSpawnPoints.Count);
        }

    }

    private void BotInit(GameObject car)
    {
        // set first way point
        car.transform.Find("Controller").GetComponent<BotsCarMovement>().SetFirstWayPoint(firstWayPoint);

        // stop movement
        car.transform.Find("Controller").GetComponent<BotsCarMovement>()._isMoved = false;

    }

    private void AllowBotsMove()
    {
        foreach (GameObject car in bots)
        {
            car.transform.Find("Controller").GetComponent<BotsCarMovement>()._isMoved = true;
        }
    }
}