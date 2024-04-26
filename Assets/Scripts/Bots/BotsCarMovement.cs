using System;
using UnityEngine;


// TODO
// FIX PROBLEM: BOT IS NEXT TO THE WALL AND DIRECTED TO THE FLOOR WITH FORWARD RAYS
// AND TRIES TO GO BACKWARD BUT THE WALL IS THERE


public class BotsCarMovement : InputOfCarMovement
{
    [SerializeField] private Collider _brakeZone;
    [SerializeField] private BotObstacleDetector _obstacleDetector;
    [SerializeField] private WayPoint _wayPoint;

    // TODO UNDERSTAND VARIABLES
    private const float CHANCE_SELECT_ALTERNATIVE_POINT = 0.5f;
    private const float _maxInputValue = 1f;
    private const float _normalizationAngle = 90f;
    private Vector3 _targetPoint;
    private bool _isBrake = false;
    private bool _hasNextPoint = false;
    private ObstacleScanerDataStruct _scanerBuffer;

    public override event Action<float> InputHorizontal;
    public override event Action<float> InputVertical;
    public override event Action<float> InputBrake;
    public override event Action InputResetCoordinats;

    public bool IsMoved;


    private void Awake()
    {
        IsMoved = true;
        _brakeZone.isTrigger = true;
        _targetPoint = GetRandomPositionFromPoint(_wayPoint.transform);
    }


    private void OnEnable()
    {
        _obstacleDetector.ScanerUpdate += InitializeScanerBuffer;
    }


    private void OnDisable()
    {
        _obstacleDetector.ScanerUpdate -= InitializeScanerBuffer;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "BotAssistant")
            return;

        SetBrake(true);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BotAssistant")
            return;

        SetBrake(false);
    }


    // TODO UNDERSTAND
    private void FixedUpdate()
    {
        if (IsMoved == false || _hasNextPoint == false)
        {
            UpdateEvents(0, 0);
        }
        else
        {
            float verticalValue = GetMoveForce();
            float horizontalValue = GetSteeringForceToThePoint();
            horizontalValue *= verticalValue < 0 ? 0 : 1;

            MoveToNextPoint();
            UpdateEvents(verticalValue, horizontalValue);

        }
    }


    private void LateUpdate()
    {
        _hasNextPoint = _wayPoint.NextPoint != null;
    }


    private void InitializeScanerBuffer(ObstacleScanerDataStruct scanerData)
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
        //Vector3 offset = UnityEngine.Random.insideUnitSphere * (point.lossyScale.x / 2);
        Vector3 position = point.position;// + offset;

        return position;
    }


    // TODO UNDERSTAND
    private float GetForseFromHitDistance(float rayDistance, float hitDistance)
    {
        return rayDistance / hitDistance;
    }


    // TODO UNDERSTAND
    private int WhereToTurnIfObstacleForward(float rightDistance, float leftDistance)
    {
        int dontTurn = 0;
        int turnToLeft = -1;
        int turnToRight = 1;

        if (rightDistance == leftDistance)
        {
            if (rightDistance != ObstacleScanerDataStruct.emptyValue)
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
            return rightDistance == ObstacleScanerDataStruct.emptyValue ? turnToLeft : turnToRight;
        }
    }


    // TODO UNDERSTAND
    private float GetForceFromForwardAvoidance(float rightDistance, float leftDistance)
    {
        float force = GetForseFromHitDistance(_obstacleDetector.ForwardRayDistance, _scanerBuffer.forwardHitDistance);
        int sign = WhereToTurnIfObstacleForward(rightDistance, leftDistance);

        force *= sign;

        return force;
    }


    // TODO UNDERSTAND
    private float GetForceFromAsideAvoidance(float rightDistance, float leftDistance)
    {
        float force;

        if (rightDistance == leftDistance && rightDistance == ObstacleScanerDataStruct.emptyValue)
        {
            force = 0;
        }
        else
        {
            float currentSide = rightDistance == ObstacleScanerDataStruct.emptyValue ? leftDistance : rightDistance;
            force = GetForseFromHitDistance(_obstacleDetector.AsideRayDistance, currentSide);
            force *= currentSide == leftDistance ? 1 : -1;
        }

        return force;
    }


    // TODO UNDERSTAND
    private float ObstacleAvoidance()
    {
        float force;
        float rightDistance = _scanerBuffer.rightHitDistance;
        float leftDistance = _scanerBuffer.leftHitDistance;

        if (_scanerBuffer.forwardHitDistance != ObstacleScanerDataStruct.emptyValue)
            force = GetForceFromForwardAvoidance(rightDistance, leftDistance);
        else
            force = GetForceFromAsideAvoidance(rightDistance, leftDistance) * 0.1f;

        return force;
    }


    // TODO UNDERSTAND
    private void SetTargetPoint(WayPoint wayPont)
    {
        Transform point;

        point = wayPont.transform;
        _wayPoint = wayPont;

        _targetPoint = GetRandomPositionFromPoint(point);
    }


    // TODO UNDERSTAND
    private void MoveToNextPoint()
    {
        if (_wayPoint.DistanceToGetNext < Vector3.Distance(transform.position, _targetPoint))
            return;

        if (_wayPoint.NextPoint != null)
        {
            if (UnityEngine.Random.value <= CHANCE_SELECT_ALTERNATIVE_POINT)
                SetTargetPoint(_wayPoint.GetRandomAlternativePoint());
            else
                SetTargetPoint(_wayPoint.NextPoint);
        }
        else
        {
            //Debug.Log("here");
            IsMoved = false;
        }
    }


    // TODO UNDERSTAND
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


    // TODO UNDERSTAND
    private float GetMoveForce()
    {
        if (_scanerBuffer.forwardHitDistance == ObstacleScanerDataStruct.emptyValue)
        {
            return _isBrake == false ? _maxInputValue : -_maxInputValue;
        }
        else
        {
            float rayDistance = _obstacleDetector.ForwardRayDistance;
            float hitDistance = _scanerBuffer.forwardHitDistance;
            float force = GetForseFromHitDistance(rayDistance, hitDistance);
            return _isBrake == false ? force : -force;
        }

        // testing remove
        // return 0;
    }


    public void SetBrake(bool isBrake)
    {
        _isBrake = isBrake;
    }
}