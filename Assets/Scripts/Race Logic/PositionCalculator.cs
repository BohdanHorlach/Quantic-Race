using System;
using System.Linq;
using UnityEngine;


public class PositionCalculator : MonoBehaviour 
{
    [SerializeField] private Transform[] _checkPoints;
    [SerializeField] private CheckPointHandler[] _cars;
    [SerializeField, Min(1)] private int _numberOfLaps;
    [SerializeField] private float _minDistanceForScoring;

    private Transform[] _racePositions;
    private string[] _names;

    private void Start()
    {
        _racePositions = new Transform[_cars.Length];
        _names = new string[_cars.Length];

        foreach (CheckPointHandler car in _cars)
        {
            car.Initialize(_checkPoints, _minDistanceForScoring, _numberOfLaps);
        }
    }


    private void Update()
    {
        UpdateRacePositions();

        for(int i = 0; i < _racePositions.Length; i++)
        {
            _names[i] = _racePositions[i].name;
        }
    }


    private void UpdateRacePositions()
    {
        _racePositions = _cars
            .OrderByDescending(car => car.CurrentLaps)
            .ThenByDescending(car => Array.IndexOf(_checkPoints, car.GetCurrentCheckPoint()))
            .ThenBy(car => car.GetDistanceToCheckPoint())
            .Select(car => car.transform)
            .ToArray();
    }


    public int GetPositionCar(CheckPointHandler car)
    {
        return Array.IndexOf(_racePositions, car.transform);
    }


    public float DistanceToNextOpponent(CheckPointHandler car)
    {
        if (_cars.Contains(car) == false)
            return -1f;
        if (_racePositions.Length == 1)
            return 0f;

        int position = GetPositionCar(car);
        Transform opponent = position == 0 ? _racePositions[1] : _racePositions[position - 1];
        float distance = Vector3.Distance(car.transform.position, opponent.position);

        return position == 0 ? -distance : distance;
    }
}