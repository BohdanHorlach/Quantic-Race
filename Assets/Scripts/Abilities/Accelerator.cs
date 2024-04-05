using System.Collections;
using UnityEngine;


public class Accelerator : Abilitiy
{
    [SerializeField] private CarMovement _car;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private Transform _acceleratorPoint;
    [SerializeField] private Transform _decelerationPoint;
    [SerializeField, Min(10)] private float _accelerationforce = 20f;
    [SerializeField, Min(1)] private float _accelerationTime = 5f;
    [SerializeField, Min(1)] private float _maxSpeedOfAcceleration = 400f;
    [SerializeField, Min(10)] private float _constantDecelerationForce = 30f;

    private bool _isActivate = false;
    private TypeAbility _type = TypeAbility.Accelerator;

    public override TypeAbility Type { get => _type; }


    private IEnumerator Acceleration()
    {
        float timer = 0;
        _particle.Play();

        while (timer < _accelerationTime)
        {
            Vector3 force = _car.transform.forward * _accelerationforce;

            if(_car.CurrentSpeed < _maxSpeedOfAcceleration)
                _car.Rigidbody.AddForceAtPosition(force, _acceleratorPoint.position);

            timer += Time.fixedDeltaTime;
            yield return null;
        }

	    StartCoroutine("SlowDownToMaxSpeed");
    }


    private IEnumerator SlowDownToMaxSpeed()
    {
        _particle.Stop();

        while(_car.CurrentSpeed > _car.MaxSpeed)
        {
            float decelerateForce = Mathf.Lerp(_car.CurrentSpeed, _car.MaxSpeed, Time.fixedDeltaTime) * 0.1f;
            Vector3 force = -_car.transform.forward * _constantDecelerationForce;
            _car.Rigidbody.AddForceAtPosition(force * decelerateForce, _decelerationPoint.position);

            yield return null;
        }
    }


    private void CallAcceleration(bool isActive, string stopedCoroutine, string startCoroutine)
    {
        _isActivate = isActive;
        StopCoroutine(stopedCoroutine);
        StartCoroutine(startCoroutine);
    }


    public override void Activate()
    {
        if (_isActivate == false)
            CallAcceleration(true, "SlowDownToMaxSpeed", "Acceleration");
        else
            CallAcceleration(false, "Acceleration", "SlowDownToMaxSpeed");
    }
}