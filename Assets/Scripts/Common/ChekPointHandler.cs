using System.Collections.Generic;
using UnityEngine;


// script used by car
public class CheckPointHandler : MonoBehaviour
{
    private Transform[] _checkPointPositionsArray;
    private Queue<Transform> _checkPointsQueue;

    // distance from car to checkpoint to mark it as reached
    private float _minDistanceForScoring = -1f;
    private int _countLaps;
    private int _currentLap;

    public int CurrentLap { get => _currentLap; }

    public string playerName;


    private void Awake()
    {
        _checkPointsQueue = new Queue<Transform>();
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

        float distanceToChekPoint = GetDistanceToCheckPoint();

        if (distanceToChekPoint < _minDistanceForScoring)
            _checkPointsQueue.Dequeue();

        if (_checkPointsQueue.Count == 0 && _currentLap <= _countLaps)
            FillCheckPoints(_checkPointPositionsArray);
    }


    // refill checkpoints queue and add lap to total laps count
    private void FillCheckPoints(Transform[] checkPoints)
    {
        if (checkPoints == null)
            return;

        foreach (Transform transform in checkPoints)
            _checkPointsQueue.Enqueue(transform);

        _currentLap++;
    }


    public Transform GetCurrentCheckPoint()
    {
        if (_checkPointsQueue == null || _checkPointsQueue.Count == 0)
            return null;

        if (_checkPointsQueue.TryPeek(out Transform checkPoint))
            return checkPoint;

        return null;
    }

    // distance from the car to current target checkpoint
    public float GetDistanceToCheckPoint()
    {
        if (_checkPointsQueue == null || _checkPointsQueue.Count == 0)
            return 0f;

        if (_checkPointsQueue.TryPeek(out Transform chekPoint))
            return Vector3.Distance(transform.position, chekPoint.position);

        return 0f;
    }


    public void Initialize(Transform[] checkPoints, float minDistanceForScoring, int countLaps, string playerName)
    {
        _checkPointPositionsArray = checkPoints;
        _countLaps = countLaps;
        _minDistanceForScoring = minDistanceForScoring;
        _checkPointsQueue.Clear();
        _currentLap = 0;
        this.playerName = playerName;

        FillCheckPoints(_checkPointPositionsArray);
    }
}