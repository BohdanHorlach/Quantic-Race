using System;
using UnityEngine;



public class BotsCarMovement : InputOfCarMovement
{
    [SerializeField] private Transform[] _wayPoints;
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private Transform _forwardRayPosition;
    [SerializeField] private LayerMask _wallsMask;
    [SerializeField, Min(1)] private float _minSteeringDistance = 1f;


    private const float _normalizationAngle = 90;
    private float _horizontalValue = 0f;
    private float _verticalValue = 0f;
    private float _handBrakeValue = 0f;
    private int _wayPointIndex = 0;
    private Vector3 _targetPoint;
    private bool isBrake = false;


    public override event Action<float> InputHorizontal;
    public override event Action<float> InputVertical;
    public override event Action<float> InputBrake;

    public bool IsMoved = true;


    private void Awake()
    {
        IsMoved = _wayPoints.Length != 0;
        _targetPoint = GetRandomPositionFromPoint(_wayPoints[_wayPointIndex]);
    }


    private void FixedUpdate()
    {
        if(IsMoved == false)
            return;

        Move();
        SteeringToPoint();
        InvokeEvents();
    }


    private void InvokeEvents()
    {
        InputVertical?.Invoke(_verticalValue);
        InputHorizontal?.Invoke(_horizontalValue);
        InputBrake?.Invoke(_handBrakeValue);
    }


    private Vector3 GetRandomPositionFromPoint(Transform point)
    {
        Vector3 offset = UnityEngine.Random.insideUnitSphere * (point.lossyScale.x / 2);
        Vector3 position = point.position + offset;

        return position;
    }


    private void SteeringToPoint()
    {
        Vector3 direction = _targetPoint - transform.position;
        float steeringAngle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        _horizontalValue = steeringAngle / _normalizationAngle;

        if (_minSteeringDistance >= Vector3.Distance(transform.position, _targetPoint)) {
            _wayPointIndex++;
            _targetPoint = GetRandomPositionFromPoint(_wayPoints[_wayPointIndex]);
        }
    }


    private void Move()
    {
        _verticalValue = isBrake == false ? 1 : -1;
    }


    public void SetBrake(bool isBrake)
    {
        this.isBrake = isBrake;
    }
}