using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class DisplayUIMultiplayer : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private AbilityControllerMultiplayer abilityController;

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
    private SceneNavigator sceneNavigator;

    public bool ActiveRaceUI 
    { 
        get => raceUI.gameObject.activeSelf;
        set 
        {
            if(photonView.IsMine == true)
                raceUI.gameObject.SetActive(value); 
        } 
    }


    private void Awake()
    {
        raceUI.gameObject.SetActive(false);
        finishingCanvas.gameObject.SetActive(false);
    }


    void Update()
    {
        if (photonView.IsMine == false)
            return;

        chargeAmountImageUI.fillAmount = abilityController.GetChargeFillPercentage();
        chargesCountTextUI.text = abilityController.GetCurrentChargeCount().ToString();
        UpdateRacePositionUI();
    }

    private void OnEnable()
    {
        if (photonView.IsMine == false)
            return;

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

    public void Initialize(PositionCalculator positionCalculator, SceneNavigator sceneNavigator)
    {
        this.positionCalculator = positionCalculator;
        this.sceneNavigator = sceneNavigator;

        GetComponent<CheckPointHandler>().handleFinishing += Finishing;
    }

    public void Finishing()
    {
        PlayerInputDetectorMultiplayer.PauseInput();
        finishPositionTextUI.text = playerPositionTextUI.text + "th";

        if (photonView.IsMine == false)
            return;

        raceUI.gameObject.SetActive(false);
        finishingCanvas.gameObject.SetActive(true);
    }

    public void onPlayAgain()
    {
        PhotonNetwork.LeaveRoom();
        sceneNavigator.GoToLobby();
    }

    public void onMainMenu()
    {
        PhotonNetwork.LeaveRoom();
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
