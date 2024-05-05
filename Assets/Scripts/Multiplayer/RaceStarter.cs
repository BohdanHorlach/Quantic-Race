using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RaceStarter : MonoBehaviourPun
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private CounterToStart _counterToStart;
    [SerializeField] private PositionCalculator _positionCalculator;

    private List<CarMovementMultiplayer> _cars;


    private void Awake()
    {
        _cars = new List<CarMovementMultiplayer>();
    }


    private void SetMoveToCar()
    {
        foreach (CarMovementMultiplayer car in _cars)
            car.IsCanMove = true;
    }


    private void InitializePositionCalculator()
    {
        CheckPointHandler[] cars = _cars.Select(car => car.GetComponent<CheckPointHandler>()).ToArray();
        _positionCalculator.Initialize(cars);
    }


    private void EnableUI()
    {
        DisplayUIMultiplayer[] displays = _cars.Select(car => car.GetComponent<DisplayUIMultiplayer>()).Where(display => display != null).ToArray();

        foreach(DisplayUIMultiplayer display in displays)
            display.ActiveRaceUI = true;
    }


    private void GoStart()
    {
        _photonView.RPC("GoStartRPC", RpcTarget.All);
    }


    [PunRPC]
    private void GoStartRPC()
    {
        SetMoveToCar();
        InitializePositionCalculator();
        EnableUI();

        _counterToStart.OnCounterEnd -= GoStart;
    }


    [PunRPC]
    private void StartCounter()
    {
        _counterToStart.StartCounter();
    }


    public void AddToListCars(CarMovementMultiplayer car)
    {
        if (car != null && _cars.Contains(car) != true)
        {
            _cars.Add(car);
            car.IsCanMove = false;
        }
    }


    public void StartRace()
    {
        _counterToStart.OnCounterEnd += GoStart;
        _photonView.RPC("StartCounter", RpcTarget.All);
    }
}