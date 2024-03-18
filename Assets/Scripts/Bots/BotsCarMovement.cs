using System;
using UnityEngine;



public class BotsCarMovement : InputOfCarMovement
{
    [SerializeField] private BotObstacleDetector _obstacleDetector;
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField, Min(1)] private float _minSteeringDistance = 1f;

    private const float _maxInputValue = 1f;
    private const float _normalizationAngle = 90f;
    private int _wayPointIndex = 0;
    private Vector3 _targetPoint;
    private bool _isBrake = false;
    private ObstacleScanerData _scanerBuffer;


    public override event Action<float> InputHorizontal;
    public override event Action<float> InputVertical;
    public override event Action<float> InputBrake;

    public bool IsMoved = true;


    private void Awake()
    {
        //IsMoved = _wayPoints.Length != 0;
        _targetPoint = GetRandomPositionFromPoint(_wayPoints[_wayPointIndex]);
    }


    private void OnEnable()
    {
        _obstacleDetector.ScanerUpdate += InitializeScanerBuffer;
    }


    private void OnDisable()
    {
        _obstacleDetector.ScanerUpdate -= InitializeScanerBuffer;
    }


    private void FixedUpdate()
    {
        if (IsMoved == false)
        {
            UpdateEvents(0, 0);
        }
        else
        {
            float verticalValue = GetMoveForce();
            float horizontalValue = GetSteeringForceToThePoint();
            horizontalValue *= verticalValue < 0 ? -1 : 1;

            MoveToNextPoint();
            UpdateEvents(verticalValue, horizontalValue);
        }
    }


    private void InitializeScanerBuffer(ObstacleScanerData scanerData)
    {
        _scanerBuffer = scanerData;
    }


    private void UpdateEvents(float verticalValue, float horizontalValue)
    {
        InputVertical?.Invoke(verticalValue);
        InputHorizontal?.Invoke(horizontalValue);
        InputBrake?.Invoke(0);
    }


    private Vector3 GetRandomPositionFromPoint(Transform point)
    {
        Vector3 offset = UnityEngine.Random.insideUnitSphere * (point.lossyScale.x / 2);
        Vector3 position = point.position + offset;

        return position;
    }


    private float GetForseFromHitDistance(float rayDistance, float hitDistance)
    {
        return (rayDistance * 0.1f) / hitDistance;
    }


    private int WhereToTurnIfObstacleForward(float rightDistance, float leftDistance)
    {
        int dontTurn = 0;
        int turnToLeft = -1;
        int turnToRight = 1;

        if (rightDistance == leftDistance)
        {
            if(rightDistance != ObstacleScanerData.emptyValue)
            {
                return dontTurn;
            }
            else
            {
                if (_obstacleDetector.LeftHitInfiniteDistance > _obstacleDetector.RightHitInfiniteDistance)
                {
                    return turnToLeft;
                }
                else
                {
                    return turnToRight;
                }
            }
        }
        else
        {
            return rightDistance == ObstacleScanerData.emptyValue ? turnToLeft : turnToRight;
        }
    }


    private float GetForceFromForwardAvoidance(float rightDistance, float leftDistance)
    {
        float force = GetForseFromHitDistance(_obstacleDetector.ForwardRayDistance, _scanerBuffer.forwardHitDistance);
        int sign = WhereToTurnIfObstacleForward(rightDistance, leftDistance);

        force *= sign;

        return force;
    }


    private float GetForceFromAsideAvoidance(float rightDistance, float leftDistance)
    {
        float force;

        if (rightDistance == leftDistance && rightDistance == ObstacleScanerData.emptyValue)
        {
            force = 0;
        }
        else
        {
            float currentSide = rightDistance == ObstacleScanerData.emptyValue ? leftDistance : rightDistance;
            force = GetForseFromHitDistance(_obstacleDetector.AsideRayDistance, currentSide);
            force *= currentSide == leftDistance ? 1 : -1;
        }

        return force;
    }


    private float ObstacleAvoidance()
    {
        float force;
        float rightDistance = _scanerBuffer.rightHitDistance;
        float leftDistance = _scanerBuffer.leftHitDistance;

        if (_scanerBuffer.forwardHitDistance != ObstacleScanerData.emptyValue) 
            force = GetForceFromForwardAvoidance(rightDistance, leftDistance);
        else
            force = GetForceFromAsideAvoidance(rightDistance, leftDistance);

        return force;
    }


    private void MoveToNextPoint()
    {
        if (_minSteeringDistance < Vector3.Distance(transform.position, _targetPoint))
            return;

        _wayPointIndex++;

        if (_wayPointIndex != _wayPoints.Length)
            _targetPoint = GetRandomPositionFromPoint(_wayPoints[_wayPointIndex]);
        else
            IsMoved = false;
    }


    private float GetSteeringForceToThePoint()
    {
        if (IsMoved == false)
            return 0;

        Vector3 direction = _targetPoint - transform.position;
        float steeringAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        float force;

        if (_scanerBuffer.IsEmpty())
            force = steeringAngle / _normalizationAngle;
        else
            force = ObstacleAvoidance();

        return force;
    }


    private float GetMoveForce()
    {
        if (_scanerBuffer.forwardHitDistance == ObstacleScanerData.emptyValue)
        {
            return _isBrake == false ? _maxInputValue : -_maxInputValue;
        }
        else
        {
            float rayDistance = _obstacleDetector.ForwardRayDistance;
            float hitDistance = _scanerBuffer.forwardHitDistance;
            return -rayDistance / hitDistance * 0.1f;
        }
    }


    public void SetBrake(bool isBrake)
    {
        _isBrake = isBrake;
    }
}