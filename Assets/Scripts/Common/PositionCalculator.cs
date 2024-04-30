using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class PositionCalculator : MonoBehaviour
{
    private Dictionary<int, List<int>> _nextCheckPointsForCheckPoint;
    [SerializeField] private Transform[] _checkPoints;

    // checkpoint handler for each car in a race
    private CheckPointHandler[] _carsRaceInfoArray = { };
    //private int _numberOfLaps;
    [SerializeField] private float _minDistanceForScoring;


    private Transform[] _racePositionsArray;
    private string[] _playerNamesArray;

    private void Start()
    {
        //_numberOfLaps = UserDataManager.selectedGameOptionsSO.numberOfLaps;
        UpdateRacePositions();
    }


    private void Update()
    {
        UpdateRacePositions();
    }


    private void CheckPointsDictionaryInit()
    {
        _nextCheckPointsForCheckPoint = new Dictionary<int, List<int>>();
        for (int i = 0; i < _checkPoints.Length - 1; i++)
        {
            _nextCheckPointsForCheckPoint[i] = new List<int>(new[] { i + 1 });
        }
        _nextCheckPointsForCheckPoint[_checkPoints.Length - 1] = new List<int>();
        // to correctly calculate race position on the fast way branch
        _nextCheckPointsForCheckPoint[15].Add(23);
    }
    public void Initialize(CheckPointHandler[] carsRaceInfoArray)
    {
        CheckPointsDictionaryInit();
        _carsRaceInfoArray = carsRaceInfoArray;
        _racePositionsArray = new Transform[_carsRaceInfoArray.Length];
        _playerNamesArray = new string[_carsRaceInfoArray.Length];
        var combinedArray = _carsRaceInfoArray.Zip(_playerNamesArray, (carRaceInfo, playerName) => new { carRaceInfo = carRaceInfo, playerName = playerName }).ToArray();
        foreach (var pair in combinedArray)
        {
            pair.carRaceInfo.Initialize(_checkPoints, _nextCheckPointsForCheckPoint, _minDistanceForScoring, pair.playerName);
        }
    }


    private void UpdateRacePositions()
    {
        // order first by lap number
        // then by checkpoint number
        // then by distance to checkpoint
        _racePositionsArray = _carsRaceInfoArray
            .OrderByDescending(carRaceInfo => carRaceInfo.CurrentLap)
            .ThenByDescending(carRaceInfo => carRaceInfo.GetCurrentTargetCheckPoinIndex())
            .ThenBy(carRaceInfo => carRaceInfo.GetMinDistanceToCheckPoin())
            .Select(carRaceInfo => carRaceInfo.transform)
            .ToArray();
    }


    public int GetCarPosition(CheckPointHandler carRaceInfo)
    {
        return Array.IndexOf(_racePositionsArray, carRaceInfo.transform) + 1;
    }

    public int GetCurrentLap(CheckPointHandler carRaceInfo)
    {
        return carRaceInfo.CurrentLap;
    }

    public bool isFinished(CheckPointHandler carRaceInfo)
    {
        return carRaceInfo.IsFinished();
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