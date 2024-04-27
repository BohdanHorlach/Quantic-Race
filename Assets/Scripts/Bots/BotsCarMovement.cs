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
    // error can be allowed when comparing world distance
    [SerializeField] private float ALLOWED_DISTANCE_ERROR = 3;

    // TODO UNDERSTAND VARIABLES
    private const float CHANCE_SELECT_ALTERNATIVE_POINT = 0.5f;
    private const float MAX_INPUT_VALUE = 1f;
    private const float MAX_STEERING_ANGLE = 90f;
    private Vector3 _targetPoint;
    //private bool _isBrake = false;
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


    // TODO UNDERSTAND
    private void FixedUpdate()
    {
        // if can move forward -> force = 1
        // else 
        if (IsMoved == false || _hasNextPoint == false)
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





    private float GetForseFromHitDistance(float rayDistance, float hitDistance)
    {
        // TODO TEST
        //return rayDistance / hitDistance;
        //return hitDistance / rayDistance;
        return MAX_INPUT_VALUE;
    }
    
    private float GetSteeringForseFromHitDistance(float rayDistance, float hitDistance)
    {
        return MAX_INPUT_VALUE - hitDistance / rayDistance;
    }


    // TODO TRY FIX
    private float GetMoveForce()
    {
        // TODO
        _scanerBuffer.CanMoveForward();
        if (_scanerBuffer.forwardHitDistance == ObstacleScanerDataStruct.emptyValue)
        {
            // can move forward
            //return _isBrake == false ? _maxInputValue : -_maxInputValue;
            return MAX_INPUT_VALUE;
        }
        else
        {
            // obstacle detected
            // TODO
            // CASE 1 GO BACK
            // CASE 2 GO FORWARD AND TURN LEFT/RIGHT
            float rayDistance = _obstacleDetector.ForwardRayDistance;
            float hitDistance = _scanerBuffer.forwardHitDistance;
            float force = GetForseFromHitDistance(rayDistance, hitDistance);
            //return _isBrake == false ? force : -force;
            return force;
        }
    }


    // TODO TRY FIX
    private int WhereToTurnIfObstacleForward(/*float rightDistance, float leftDistance*/)
    {
        //int forward = 0;
        int left = -1;
        int right = 1;
        float rightDistance = _scanerBuffer.rightHitDistance;
        float leftDistance = _scanerBuffer.leftHitDistance;


        if (Approximately(rightDistance, leftDistance, ALLOWED_DISTANCE_ERROR))
        {
            // if left and right distances are approximately equal

            //if (rightDistance != ObstacleScanerDataStruct.emptyValue)
            //{
            //    return forward;
            //}
            //else
            //{
            if (_obstacleDetector.LeftHitInfiniteDistance > _obstacleDetector.RightHitInfiniteDistance)
            {
                return left;
            }
            else
            {
                return right;
            }
            //}
        }
        else
        {
            return rightDistance == ObstacleScanerDataStruct.emptyValue ? left : right;
        }
    }


    // TODO TRY FIX
    private float GetSteeringForceFromForwardAvoidance(/*float rightDistance, float leftDistance*/)
    {
        float steeringForce = GetForseFromHitDistance(_obstacleDetector.ForwardRayDistance, _scanerBuffer.forwardHitDistance);
        int SideSign = WhereToTurnIfObstacleForward(/*rightDistance, leftDistance*/);

        steeringForce *= SideSign;

        return steeringForce;
    }


    // TODO TRY FIX
    private float GetSteeringForceFromAsideAvoidance(/*float rightDistance, float leftDistance*/)
    {
        float rightDistance = _scanerBuffer.rightHitDistance;
        float leftDistance = _scanerBuffer.leftHitDistance;
        float steeringForce;
        float SideSign;

        //if (Approximately(rightDistance, leftDistance, ALLOWED_DISTANCE_ERROR) && rightDistance == ObstacleScanerDataStruct.emptyValue)
        //{
        //    steeringForce = 0;
        //}
        //else
        //{
        float nearestSideDistance;// = rightDistance == ObstacleScanerDataStruct.emptyValue ? leftDistance : rightDistance;

        if(rightDistance == ObstacleScanerDataStruct.emptyValue)
        {
            nearestSideDistance = leftDistance;
            SideSign = 1;
        }
        else if(leftDistance == ObstacleScanerDataStruct.emptyValue)
        {
            nearestSideDistance = rightDistance;
            SideSign = -1;
        }
        else
        {
            nearestSideDistance = 0;
            SideSign = 0;
        }
        steeringForce = GetSteeringForseFromHitDistance(_obstacleDetector.AsideRayDistance, nearestSideDistance);
        steeringForce *= SideSign;
        //}

        return steeringForce;
    }


    // TODO TRY FIX
    private float GetSteeringForceObstacleAvoidance()
    {
        float steeringForce;


        if (_scanerBuffer.forwardHitDistance != ObstacleScanerDataStruct.emptyValue)
            steeringForce = GetSteeringForceFromForwardAvoidance(/*rightDistance, leftDistance*/);
        else
            steeringForce = GetSteeringForceFromAsideAvoidance(/*rightDistance, leftDistance*/);// * 0.1f;

        return steeringForce;
    }


    // TODO TRY FIX
    private float GetSteeringForce()
    {

        Vector3 direction = _targetPoint - transform.position;
        float steeringAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        //Debug.Log(steeringAngle);
        float force;

        if (_scanerBuffer.IsEmpty())
            force = steeringAngle / MAX_STEERING_ANGLE;
        else
            // TODO
            force = GetSteeringForceObstacleAvoidance();

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

    // TODO UNDERSTAND
    private void SetTargetPoint(WayPoint wayPont)
    {
        Transform point;

        point = wayPont.transform;
        _wayPoint = wayPont;

        _targetPoint = GetWayPointCenter(point);
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
            IsMoved = false;
        }
    }



    // helper functions

    public static bool Approximately(float a, float b, float allowedError)
    {
        return Mathf.Abs(a - b) <= allowedError;
    }

}