using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO
// FIX PROBLEM: BOT IS NEXT TO THE WALL AND DIRECTED TO THE FLOOR WITH FORWARD RAYS
// AND TRIES TO GO BACKWARD BUT THE WALL IS THERE



public class BotsCarMovement : InputOfCarMovement
{

    [SerializeField] private Collider _brakeZone;
    [SerializeField] private BotObstacleDetector _obstacleDetector;
    [SerializeField] private WayPoint _wayPoint;


    private const float CHANCE_SELECT_ALTERNATIVE_POINT = 0.5f;
    private const float MAX_INPUT_VALUE = 1f;
    private const float ANGLE_NORMALZIER = 90f;



    private Vector3 _targetPoint;
    private bool _hasNextPoint = false;
    private ObstacleScanerDataStruct _scanerBuffer;

    //private bool _isBrake = false;
    public bool _isMoved;

    public override event Action<float> InputHorizontal;
    public override event Action<float> InputVertical;
    public override event Action<float> InputBrake;
    public override event Action InputResetCoordinats;




    private void Awake()
    {
        _isMoved = true;
        _brakeZone.isTrigger = true;
        _targetPoint = GetWayPointCenter(_wayPoint.transform);
    }


    // subscribe to getting raycasting data from [BotObstacleDetector]
    private void OnEnable()
    {
        _obstacleDetector.ScanerUpdate += InitializeScanerBuffer;
    }

    // unsubscribe from getting raycasting data from [BotObstacleDetector]
    private void OnDisable()
    {
        _obstacleDetector.ScanerUpdate -= InitializeScanerBuffer;
    }


    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "BotAssistant")
    //        return;

    //    SetBrake(true);
    //}


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "BotAssistant")
    //        return;

    //    SetBrake(false);
    //}


    private void FixedUpdate()
    {
        if (_isMoved == false || _hasNextPoint == false)
        {
            InvokeEvents(0, 0);
        }
        else
        {
            float verticalValue = GetMoveForce();
            float horizontalValue = GetSteeringForce();

            horizontalValue *= verticalValue < 0 ? 0 : 1;

            MoveToNextPoint();
            InvokeEvents(verticalValue, horizontalValue);

        }
    }


    private void LateUpdate()
    {
        _hasNextPoint = _wayPoint.NextPoint != null;
    }

    // get raycasting data from [BotObstacleDetector]
    private void InitializeScanerBuffer(ObstacleScanerDataStruct scanerData)
    {
        _scanerBuffer = scanerData;
    }

    // input simulation
    private void InvokeEvents(float verticalValue, float horizontalValue)
    {
        InputVertical?.Invoke(verticalValue);
        InputHorizontal?.Invoke(horizontalValue);
        InputBrake?.Invoke(0);
    }




    // TODO ADD COMMENT
    private float GetForseFromHitDistance()
    {
        return MAX_INPUT_VALUE;
    }

    // steering force to avoid accident
    private float GetSteeringForseFromHitDistance(float rayDistance, float hitDistance)
    {
        return MAX_INPUT_VALUE - hitDistance / rayDistance;
    }

    // getting moving force
    private float GetMoveForce()
    {
        if (_scanerBuffer.forwardHitDistance == ObstacleScanerDataStruct.emptyValue)
        {
            // can move forward
            return GetForseFromHitDistance();
        }
        else
        {
            // obstacle detected
            float force = GetForseFromHitDistance();
            //return _isBrake == false ? force : -force;
            return force;
        }
    }

    // steering force to avoid forward obstacle
    private float GetSteeringForceFromForwardAvoidance()
    {
        float steeringForce = MAX_INPUT_VALUE;
        int SideSign = _scanerBuffer.GetSideWithMoreSpace();

        steeringForce *= SideSign;


        return steeringForce;
    }


    // steering force to avoid left or right obstacle
    private float GetSteeringForceFromAsideAvoidance()
    {
        float rightDistance = _scanerBuffer.rightHitDistance;
        float leftDistance = _scanerBuffer.leftHitDistance;
        float steeringForce;
        float SideSign;

        if (rightDistance == ObstacleScanerDataStruct.emptyValue)
        {
            // if can turn on the right
            steeringForce = GetSteeringForseFromHitDistance(_obstacleDetector.AsideRayDistance, leftDistance);
            SideSign = 1;
        }
        else if (leftDistance == ObstacleScanerDataStruct.emptyValue)
        {
            // if can turn on the left
            steeringForce = GetSteeringForseFromHitDistance(_obstacleDetector.AsideRayDistance, rightDistance);
            SideSign = -1;
        }
        else
        {
            steeringForce = 0;
            SideSign = 0;
        }
        steeringForce *= SideSign;

        return steeringForce;
    }


    // steering force to avoid any obstacle
    // TODO ANGLE_NORMALZIER IS ?
    private float GetSteeringForce()
    {

        Vector3 direction = _targetPoint - transform.position;
        float targetAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        Debug.Log(targetAngle);
        float force;

        if (_scanerBuffer.IsEmpty())
        {
            // no obstacles
            Debug.Log("no obstacles");
            // TODO UNDERSTAND HOW TO TRANSFORM TARGET ANGLE TO FORCE
            force = targetAngle / ANGLE_NORMALZIER;
        }
        else if (_scanerBuffer.forwardHitDistance != ObstacleScanerDataStruct.emptyValue)
        {
            // obstacle is forward
            Debug.Log("obstacle is forward");
            force = GetSteeringForceFromForwardAvoidance();
        }
        else
        {
            // obstacle is on the left or right
            Debug.Log("obstacle is on the left or right");
            force = GetSteeringForceFromAsideAvoidance();
        }

        return force;
    }




    //public void SetBrake(bool isBrake)
    //{
    //    _isBrake = isBrake;
    //}


    private Vector3 GetWayPointCenter(Transform point)
    {
        return point.position;
    }

    private Vector3 GetRandomPointInsideWayPoint(Transform point)
    {
        Vector3 offset = UnityEngine.Random.insideUnitSphere * (point.lossyScale.x / 2);
        Vector3 position = point.position + offset;

        return position;
    }

    private void SetTargetPoint(WayPoint wayPont)
    {
        Transform point = wayPont.transform;

        _wayPoint = wayPont;

        _targetPoint = GetWayPointCenter(point);
    }


    public void MoveToNextPoint()
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
            _isMoved = false;
        }
    }


}