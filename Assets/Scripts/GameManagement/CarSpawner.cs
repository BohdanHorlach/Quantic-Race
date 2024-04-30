using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSpawner : MonoBehaviour
{
    private List<int> freeSpawnPoints;
    [SerializeField] private WayPoint firstWayPoint;
    [SerializeField] private int coutndownTimeSeconds = 3;
    [SerializeField] private PositionCalculator positionCalculator;
    [SerializeField] private SceneNavigator sceneNavigator;

    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Canvas countdownCanvas;

    private int _currentSpawnPoint;
    private List<GameObject> bots;
    public GameObject playerCar;

    private void Awake()
    {
        bots = new List<GameObject>();
        InitFreeSpawnPoints();
    }

    private void OnEnable()
    {
        SpawnPlayer();
        SpawnBots();
        PositionCalculatorInit();
        InitUI();
        //NumberOfCarsUIInit();
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
        countdownCanvas.gameObject.SetActive(true);
        //countdownText.enabled = true;
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
        countdownCanvas.gameObject.SetActive(false);
        PlayerInputDetector.ResumeInput();
        SetBotsMovement(true);

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
        playerCar = Spawn(car);
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

    public void SetBotsMovement(bool canMove)
    {
        foreach (GameObject car in bots)
        {
            car.transform.Find("Controller").GetComponent<BotsCarMovement>()._isMoved = canMove;
        }
    }




    private void InitUI()
    {
        if (playerCar.TryGetComponent(out DisplayUI displayUI))
        {
            displayUI.Initialize(positionCalculator, this, sceneNavigator);
        }
        else
        {
            Debug.Log("Cannot get Countdown TextMeshProUGUI from user car");
        }

    }

    private void PositionCalculatorInit()
    {
        CheckPointHandler[] checkPointHandlers = bots.Select(bot => bot.GetComponent<CheckPointHandler>())
                                                     .Concat(new[] { playerCar.GetComponent<CheckPointHandler>() })
                                                     .ToArray();

        positionCalculator.Initialize(checkPointHandlers);
    }


}
