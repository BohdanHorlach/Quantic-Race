using System.Collections;
using UnityEngine;


public class Accelerator : Ability
{
    [SerializeField] private CarMovement _car;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private Transform _acceleratorPoint;
    [SerializeField] private Transform _decelerationPoint;
    [SerializeField, Min(10)] private float _accelerationforce = 20f;
    [SerializeField, Min(1)] private float _accelerationTime = 5f;
    [SerializeField, Min(1)] private float _maxSpeedOfAcceleration = 400f;
    [SerializeField, Min(10)] private float _constantDecelerationForce = 30f;

    private bool _isAccelerationActive = false;
    private bool _isDecelerationActive = false;
    private TypeAbility _type = TypeAbility.Accelerator;

    public override TypeAbility Type { get => _type; }


    private IEnumerator Acceleration()
    {
        _isAccelerationActive = true;
        float timer = 0;
        _particle.Play();

        while (timer < _accelerationTime)
        {
            Vector3 force = _car.transform.forward * _accelerationforce;

            if (_car.CurrentSpeed < _maxSpeedOfAcceleration)
                _car.Rigidbody.AddForceAtPosition(force, _acceleratorPoint.position);

            timer += Time.fixedDeltaTime;
            yield return null;
        }

        _isAccelerationActive = false;

        StartCoroutine(SlowDownToMaxSpeed());
    }


    private IEnumerator SlowDownToMaxSpeed()
    {
        _isDecelerationActive = true;
        _particle.Stop();

        while (_car.CurrentSpeed > _car.MaxSpeed)
        {
            // TODO maybe change formula
            float decelerateCoefficient = 0.1f;
            float decelerateForce = Mathf.Lerp(_car.CurrentSpeed, _car.MaxSpeed, Time.fixedDeltaTime) * decelerateCoefficient;
            Vector3 force = -_car.transform.forward * _constantDecelerationForce;
            _car.Rigidbody.AddForceAtPosition(force * decelerateForce, _decelerationPoint.position);

            yield return null;
        }
        _isDecelerationActive = false;
    }


    private void ToggleAcceleration()
    {
        // Check the current state
        if (_isAccelerationActive)
        {
            // If currently accelerating, stop acceleration and start deceleration
            StopCoroutine(Acceleration());
            StartCoroutine(SlowDownToMaxSpeed());
        }
        else if (_isDecelerationActive)
        {
            // If currently decelerating, stop deceleration and start acceleration
            StopCoroutine(SlowDownToMaxSpeed());
            StartCoroutine(Acceleration());
        }
        else
        {
            // If not accelerating or decelerating, start acceleration
            StartCoroutine(Acceleration());
        }
    }



    public override void Activate()
    {
        ToggleAcceleration();
    }


    public override bool IsActive()
    {
        return _isAccelerationActive;
    }
}