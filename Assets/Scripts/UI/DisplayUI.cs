using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUI : MonoBehaviour
{
    [SerializeField] private AbilitiyController abilitiyController;

    [SerializeField] private Image chargeAmountImageUI;
    [SerializeField] private TextMeshProUGUI chargesCountTextUI;

    [SerializeField] private TextMeshProUGUI numberOfCarsTextUI;
    [SerializeField] private TextMeshProUGUI playerPositionTextUI;

    [SerializeField] private TextMeshProUGUI countDownTextUI;

    private PositionCalculator positionCalculator;

    void Update()
    {
        chargeAmountImageUI.fillAmount = abilitiyController.GetChargeFillPercentage();
        chargesCountTextUI.text = abilitiyController.GetCurrentChargeCount().ToString();
        UpdateRacePositionUI();
    }

    private void OnEnable()
    {
        NumberOfCarsUIInit();
    }

    private void NumberOfCarsUIInit()
    {
        numberOfCarsTextUI.text = UserDataManager.selectedGameOptionsSO.spawnPoints.Length.ToString();
    }

    public void UpdateRacePositionUI()
    {
        playerPositionTextUI.text = positionCalculator.GetCarPosition(GetComponent<CheckPointHandler>()).ToString();
    }

    public void GetPositionCalculator(PositionCalculator positionCalculator)
    {
        this.positionCalculator = positionCalculator;
    }
}
