using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUI : MonoBehaviour
{
    [SerializeField] private AbilityControllerSinglePlayer abilityController;


    [SerializeField] private Image chargeAmountImageUI;
    [SerializeField] private TextMeshProUGUI chargesCountTextUI;

    [SerializeField] private TextMeshProUGUI numberOfCarsTextUI;
    [SerializeField] private TextMeshProUGUI playerPositionTextUI;

    [SerializeField] private TextMeshProUGUI currentLapTextUI;
    [SerializeField] private TextMeshProUGUI numberOfLapsTextUI;

    [SerializeField] private TextMeshProUGUI finishPositionTextUI;
    [SerializeField] private Canvas finishingCanvas;
    [SerializeField] private Canvas raceUI;


    private PositionCalculator positionCalculator;
    private CarSpawner carSpawner;
    private SceneNavigator sceneNavigator;

    void Update()
    {
        chargeAmountImageUI.fillAmount = abilityController.GetChargeFillPercentage();
        chargesCountTextUI.text = abilityController.GetCurrentChargeCount().ToString();
        UpdateRacePositionUI();
    }

    private void OnEnable()
    {
        finishingCanvas.gameObject.SetActive(false);
        NumberOfCarsUIInit();
        NumberOfLapsUIInit();
    }

    private void NumberOfCarsUIInit()
    {
        numberOfCarsTextUI.text = UserDataManager.selectedGameOptionsSO.spawnPoints.Length.ToString();
    }

    private void NumberOfLapsUIInit()
    {
        numberOfLapsTextUI.text = UserDataManager.selectedGameOptionsSO.numberOfLaps.ToString();
    }

    public void UpdateRacePositionUI()
    {
        playerPositionTextUI.text = positionCalculator.GetCarPosition(GetComponent<CheckPointHandler>()).ToString();
        currentLapTextUI.text = positionCalculator.GetCurrentLap(GetComponent<CheckPointHandler>()).ToString();
    }

    public void Initialize(PositionCalculator positionCalculator, CarSpawner carSpawner, SceneNavigator sceneNavigator)
    {
        this.positionCalculator = positionCalculator;
        this.carSpawner = carSpawner;
        this.sceneNavigator = sceneNavigator;

        GetComponent<CheckPointHandler>().handleFinishing += Finishing;
    }

    public void Finishing()
    {
        if (TryGetComponent(out CarMovementSinglePlayer component))
        {
            component.SlowDownTo0();
        }
        finishingCanvas.transform.Find("PlayAgain").GetComponent<Button>().onClick.AddListener(onPlayAgain);
        finishingCanvas.transform.Find("MainMenu").GetComponent<Button>().onClick.AddListener(onMainMenu);
        raceUI.gameObject.SetActive(false);
        PlayerInputDetectorSinglePlayer.PauseInput();
        finishingCanvas.gameObject.SetActive(true);
        finishPositionTextUI.text = playerPositionTextUI.text + "th";
        carSpawner.SetBotsMovement(false);
    }

    private void onPlayAgain()
    {
        sceneNavigator.StartRace();
    }

    private void onMainMenu()
    {
        sceneNavigator.CarSelectionMenu();
    }

    private void OnDisable()
    {
        if (TryGetComponent(out CheckPointHandler component))
        {
            component.handleFinishing -= Finishing;
        }
    }
}
