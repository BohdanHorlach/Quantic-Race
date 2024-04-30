using System;
using System.Collections.Generic;
using UnityEngine;


// script used by car
public class CheckPointHandler : MonoBehaviour
{
    private Dictionary<int, List<int>> _nextCheckPointsForCheckPoint;


    private Transform[] _checkPointsArray;
    private List<int> _targetCheckPoints;

    // distance from car to checkpoint to mark it as reached
    private float _minDistanceForScoring = -1f;
    private int _countLaps;
    private int _currentLap;

    public int CurrentLap { get => _currentLap; }

    public string playerName;


    private void Awake()
    {
        _targetCheckPoints = new List<int>(new[] { 0 });
    }


    private void Update()
    {
        GoNextCheckPoint();
    }


    private void GoNextCheckPoint()
    {
        // all laps done
        if (_currentLap > _countLaps)
            return;


        bool isNext = false;
        List<int> newTargetCheckPoints = new List<int>();
        foreach (int index in _targetCheckPoints)
        {
            float distanceToChekPoint = GetDistanceToCheckPoint(index);
            if (distanceToChekPoint < _minDistanceForScoring)
            {
                isNext = true;
                newTargetCheckPoints = _nextCheckPointsForCheckPoint[index];
            }
        }
        if (isNext)
        {
            _targetCheckPoints = newTargetCheckPoints;
        }



        if (_targetCheckPoints.Count == 0 && _currentLap <= _countLaps)
        {
            _targetCheckPoints = new List<int>(new[] { 0 });
            _currentLap++;
        }
    }


    public int GetCurrentTargetCheckPoinIndex()
    {
        return _targetCheckPoints[0];
    }

    // distance from the car to specific target checkpoint
    public float GetDistanceToCheckPoint(int index)
    {
        if (index == -1)
            return 0f;

        return Vector3.Distance(transform.position, _checkPointsArray[index].position);
    }

    public float GetMinDistanceToCheckPoin()
    {
        float minDistance = float.MaxValue;
        foreach (int index in _targetCheckPoints)
        {
            minDistance = Mathf.Min(minDistance, GetDistanceToCheckPoint(index));
        }
        return minDistance;
    }


    public void Initialize(Transform[] checkPoints, Dictionary<int, List<int>> nextCheckPointsForCheckPoint, float minDistanceForScoring, int countLaps, string playerName)
    {
        _checkPointsArray = checkPoints;
        _nextCheckPointsForCheckPoint = nextCheckPointsForCheckPoint;
        _targetCheckPoints = new List<int>(new[] { 0 });
        _countLaps = countLaps;
        _minDistanceForScoring = minDistanceForScoring;
        _currentLap = 0;
        this.playerName = playerName;
    }
}