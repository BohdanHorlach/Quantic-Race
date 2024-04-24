using UnityEngine;
using Photon.Pun;
using System.Collections;


public class CarMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private CarCameraSwitcher _cameraSwitcher;
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
    private bool isCoroutineRunning = false;
        
    private void Awake()
    {
        if (_photonView.IsMine == true)
        {
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

    private bool isFlipped()
    {
        return Vector3.Dot(transform.up, Vector3.down) > 0;
    }

    private void ResetCoordinates()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float currentYRotation = transform.eulerAngles.y;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


    private IEnumerator ResetCarPosition()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(1);
        if (isFlipped())
        {
            ResetCoordinates();
        }
        isCoroutineRunning = false;
    }


    private void Update()
    {
        if (_photonView.IsMine)
        {
            foreach (Axle axle in _axles)
                axle.UpdateAxle();
        }

        if (isFlipped() && !isCoroutineRunning)
        {
            StartCoroutine(ResetCarPosition());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCoordinates();
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
        if (_photonView.IsMine)
        {
            Gas(force);
        }
    }
}
