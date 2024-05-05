using UnityEngine;
using System.Collections;


public class CarMovementSinglePlayer : MonoBehaviour
{
    [SerializeField] private CarCameraSwitcher _cameraSwitcher;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private InputOfCarMovement _input;
    [SerializeField] private Axle[] _axles;
    [SerializeField, Min(1)] private float _maxSpeed = 300;
    [SerializeField, Min(1)] private float _motorForce = 1000;
    [SerializeField, Min(1)] private float _brakeForce = 800;
    [SerializeField, Min(1)] private float _steeringAngle = 60;
    [SerializeField, Min(1)] private float _handbrakeForce = 300;
    [SerializeField, Min(1)] private float _dividerForReverceForce = 3;


    public float CurrentSpeed { get => _rigidbody.velocity.sqrMagnitude; }
    public float MaxSpeed { get => _maxSpeed; }
    public Rigidbody Rigidbody { get => _rigidbody; }
    private float secondsWaitToResetPosition = 3;


    private void Awake()
    {
        _cameraSwitcher.Enable();
    }

    private void Start()
    {
        StartCoroutine(FlippingProcessing());
    }

    public void OnEnable()
    {
        _input.InputHorizontal += Steering;
        _input.InputVertical += InputVerticalProcessing;
        _input.InputBrake += Brake;
        _input.InputResetCoordinats += ResetCoordinates;
    }


    public void OnDisable()
    {
        _input.InputHorizontal -= Steering;
        _input.InputVertical -= InputVerticalProcessing;
        _input.InputBrake -= Brake;
        _input.InputResetCoordinats -= ResetCoordinates;
    }

    private bool isFlipped()
    {
        return Vector3.Dot(transform.up, Vector3.down) > 0;
    }

    private void ResetCoordinates()
    {
        float currentYRotation = transform.eulerAngles.y;

        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private IEnumerator FlippingProcessing()
    {
        if (isFlipped())
        {
            ResetCoordinates();
        }
        yield return new WaitForSeconds(secondsWaitToResetPosition);
        StartCoroutine(FlippingProcessing());
    }


    private void Update()
    {
        UpdateAxles();

    }

    private void UpdateAxles()
    {
        foreach (Axle axle in _axles)
            axle.UpdateAxle();
    }

    private void Gas(float force)
    {
        foreach (Axle axle in _axles)
            axle.SetMotorTorque(force);
    }


    private void Steering(float force)
    {
        foreach (Axle axle in _axles)
            axle.SetSteering(force * _steeringAngle);
    }


    private void Brake(float force)
    {
        foreach (Axle axle in _axles)
            axle.SetBrakeTorque(force * _handbrakeForce);
    }


    private void InputVerticalProcessing(float value)
    {
        float force = 0;

        if (value >= 0 || value < 0 && _axles[0].GetCurentRPM() <= 0)
        {
            float gasForce = Mathf.Clamp(_maxSpeed / CurrentSpeed * 0.1f, 0, 1);
            force = value >= 0 ? _motorForce * gasForce : _motorForce / _dividerForReverceForce;
            force *= value;
        }
        else if (value < 0 && _axles[0].GetCurentRPM() > 0)
        {
            force = value * _brakeForce;
        }

        Gas(force);
    }


    public void SlowDownTo0()
    {
        GetComponent<Rigidbody>().drag = 100f;
    }
}
