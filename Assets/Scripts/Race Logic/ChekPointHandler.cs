using System.Collections.Generic;
using UnityEngine;


public class CheckPointHandler : MonoBehaviour
{
    private Transform[] _chekPointsPosition;
    private Queue<Transform> _chekPoints;
    private float _minDistanceForScoring;
    private int _countLaps;
    private int _currentLaps;

    public int CurrentLaps { get => _currentLaps; }


    private void Awake()
    {
        _chekPoints = new Queue<Transform>();
    }


    private void Update()
    {
        GoNextCheckPoint();
    }


    private void GoNextCheckPoint()
    {
        if (_currentLaps > _countLaps)
            return;

        float distanceToChekPoint = GetDistanceToCheckPoint();

        if (distanceToChekPoint < _minDistanceForScoring)
            _chekPoints.Dequeue();

        if (_chekPoints.Count == 0 && _currentLaps <= _countLaps)
            FillCheckPoints(_chekPointsPosition);
    }


    private void FillCheckPoints(Transform[] checkPoints)
    {
        foreach (Transform transform in checkPoints)
            _chekPoints.Enqueue(transform);

        _currentLaps++;
    }


    public Transform GetCurrentCheckPoint()
    {
        if (_chekPoints.TryPeek(out Transform checkPoint))
            return checkPoint;

        return null;
    }


    public float GetDistanceToCheckPoint()
    {
        if (_chekPoints.TryPeek(out Transform chekPoint))
            return Vector3.Distance(transform.position, chekPoint.position);

        return 0f;
    }


    public void Initialize(Transform[] checkPoints, float minDistanceForScoring, int countLaps)
    {
        _chekPointsPosition = checkPoints;
        _countLaps = countLaps;
        _minDistanceForScoring = minDistanceForScoring;
        _chekPoints.Clear();
        _currentLaps = 0;

        FillCheckPoints(_chekPointsPosition);
    }
}