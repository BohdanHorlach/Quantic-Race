using UnityEngine;
using Photon.Pun;


public class CarMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] PhotonView _playerPrefab;
    [SerializeField] CarCameraSwitcher _cameraSwitcher;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private InputOfCarMovement _input;
    [SerializeField] private Axle[] _axles;
    [SerializeField, Min(1)] private float _maxSpeed;
    [SerializeField, Min(1)] private float _motorForce;
    [SerializeField, Min(1)] private float _brakeForce;
    [SerializeField, Min(1)] private float _steeringAngleForce;
    [SerializeField, Min(1)] private float _handbrakeForce;
    [SerializeField, Min(1)] private float _dividerForReverceForce;


    public float CurrentSpeed { get => _rigidbody.velocity.sqrMagnitude; }
    public float MaxSpeed { get => _maxSpeed; }
    public Rigidbody Rigidbody { get => _rigidbody; }

    private void Awake()
    {
        if (_playerPrefab.IsMine == true) {
            _cameraSwitcher.Enable();
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();

        _input.InputHorizontal += Steering;
        _input.InputVertical += InputVerticalProcessing;
        _input.InputBrake += Brake;
    }


    public override void OnDisable()
    {
        base.OnDisable();

        _input.InputHorizontal -= Steering;
        _input.InputVertical -= InputVerticalProcessing;
        _input.InputBrake -= Brake;
    }


    private void Update()
    {
        if (_playerPrefab.IsMine)
        {
            foreach (Axle axle in _axles)
                axle.UpdateAxle();
        }
    }


    private void Gas(float force)
    {
        foreach (Axle axle in _axles)
            axle.SetMotorTorque(force);
    }


    private void Steering(float force)
    {
        foreach (Axle axle in _axles)
            axle.SetSteering(force * _steeringAngleForce);
    }


    private void Brake(float force)
    {
        foreach (Axle axle in _axles)
            axle.SetBrakeTorque(force * _handbrakeForce);
    }


    private void InputVerticalProcessing(float value)
    {
        float force = 0;

        if ((value >= 0 || value < 0 && _axles[0].GetCurentRPM() <= 0))
        {
            float gasForce = Mathf.Clamp(_maxSpeed / CurrentSpeed * 0.1f, 0, 1);
            force = value >= 0 ? _motorForce * gasForce : _motorForce / _dividerForReverceForce;
            force *= value;
        }
        else if (value < 0 && _axles[0].GetCurentRPM() > 0)
        {
            force = value * _brakeForce;
        }
        if (_playerPrefab.IsMine)
        {
            Gas(force);
        }
    }
}
