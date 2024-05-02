using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class RaceStarter : MonoBehaviourPun
{
    [SerializeField] private PhotonView _view;
    [SerializeField] private PositionCalculator _positionCalculator;

    private List<CarMovement> _cars;

    private void Awake()
    {
        _cars = new List<CarMovement>();
    }


    private void SetMoveToCar()
    {
        foreach (CarMovement car in _cars)
            car.IsCanMove = true;
    }


    private void InitializePositionCalculator()
    {
        Transform[] carTransforms = _cars.Select(car => car.transform).ToArray();
        _positionCalculator.Initialize(carTransforms);
    }


    [PunRPC]
    private void GoStart()
    {
        SetMoveToCar();
        InitializePositionCalculator();
    }


    public void AddToListCars(CarMovement car)
    {
        if (car != null && _cars.Contains(car) != true)
        {
            _cars.Add(car);
            car.IsCanMove = false;
        }
    }


    public void StartRace()
    {
        _view.RPC("GoStart", RpcTarget.All);
    }
}