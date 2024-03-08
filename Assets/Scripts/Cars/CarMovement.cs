using UnityEngine;

public class CarMovement : MonoBehaviour
{
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


    private void OnEnable()
    {
        _input.InputHorizontal += Steering;
        _input.InputVertical += InputVerticalProcessing;
        _input.InputBrake += Brake;
    }


    private void OnDisable()
    {
        _input.InputHorizontal -= Steering;
        _input.InputVertical -= InputVerticalProcessing;
        _input.InputBrake -= Brake;
    }


    private void Update()
    {
        foreach (Axle axle in _axles)
            axle.UpdateAxle();
    }


    private void Gas(float force)
    {
        foreach(Axle axle in _axles) 
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

        if ((value >= 0 || value < 0 && _axles[0].GetCurentRPM() <= 0) && CurrentSpeed <= _maxSpeed)
        {
            force = value >= 0 ? _motorForce : _motorForce / _dividerForReverceForce;
            force *= value;
        }
        else if (value < 0 && _axles[0].GetCurentRPM() > 0)
        {
            force = value * _brakeForce;
        }

        Gas(force);
    }

}
