using System;
using System.Linq;
using UnityEngine;


public class PositionCalculator : MonoBehaviour 
{
    [SerializeField] private Transform[] _checkPoints;

    // checkpoint handler for each car in a race
    [SerializeField] private CheckPointHandler[] _carsRaceInfoArray;
    [SerializeField, Min(1)] private int _numberOfLaps;
    [SerializeField] private float _minDistanceForScoring;


    private Transform[] _racePositionsArray;
    private string[] _playerNamesArray;

    private void Start()
    {
        _racePositionsArray = new Transform[_carsRaceInfoArray.Length];
        _playerNamesArray = new string[_carsRaceInfoArray.Length];

        var combinedArray = _carsRaceInfoArray.Zip(_playerNamesArray, (carRaceInfo, playerName) => new { carRaceInfo = carRaceInfo, playerName = playerName }).ToArray();
        foreach (var pair in combinedArray)
        {
            pair.carRaceInfo.Initialize(_checkPoints, _minDistanceForScoring, _numberOfLaps, pair.playerName);
        }

        UpdateRacePositions();
    }


    private void Update()
    {
        UpdateRacePositions();
    }


    private void UpdateRacePositions()
    {
        // order first by lap number
        // then by checkpoint number
        // then by distance to checkpoint
        _racePositionsArray = _carsRaceInfoArray
            .OrderByDescending(carRaceInfo => carRaceInfo.CurrentLap)
            .ThenByDescending(carRaceInfo => Array.IndexOf(_checkPoints, carRaceInfo.GetCurrentCheckPoint()))
            .ThenBy(carRaceInfo => carRaceInfo.GetDistanceToCheckPoint())
            .Select(carRaceInfo => carRaceInfo.transform)
            .ToArray();
    }


    public int GetCarPosition(CheckPointHandler carRaceInfo)
    {
        return Array.IndexOf(_racePositionsArray, carRaceInfo.transform);
    }


    public float DistanceToNextOpponent(CheckPointHandler carRaceInfo)
    {
        if (_carsRaceInfoArray.Contains(carRaceInfo) == false)
            return -1f;
        if (_racePositionsArray.Length == 1)
            return 0f;

        int position = GetCarPosition(carRaceInfo);
        Transform opponent = position == 0 ? _racePositionsArray[1] : _racePositionsArray[position - 1];
        float distance = Vector3.Distance(carRaceInfo.transform.position, opponent.position);

        return position == 0 ? -distance : distance;
    }
}