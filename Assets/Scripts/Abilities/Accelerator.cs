using System.Collections;
using UnityEngine;


public class Accelerator : Abilities
{
    [SerializeField] private CarMovement _carMovement;
    [SerializeField] private Transform _acceleratorPoint;
    [SerializeField, Min(10)] private float _accelerationforce = 20f;
    [SerializeField, Min(1)] private float _accelerationTime = 5f;
    [SerializeField, Min(1)] private float _maxSpeedOfAcceleration = 400f;
    [SerializeField, Min(10)] private float _constantDecelerationForce = 30f;


    private bool _isActivate = false;


    private IEnumerator Acceleration()
    {
        float timer = 0;

        while (timer < _accelerationTime)
        {
            Vector3 force = _carMovement.transform.forward * _accelerationforce;

            if(_carMovement.CurrentSpeed < _maxSpeedOfAcceleration)
                _carMovement.Rigidbody.AddForceAtPosition(force, _acceleratorPoint.position);

            timer += Time.fixedDeltaTime;
            yield return null;
        }
    }


    private IEnumerator SlowDownToMaxSpeed()
    {
        while(_carMovement.CurrentSpeed > _carMovement.MaxSpeed)
        {
            float force = Mathf.Lerp(_carMovement.CurrentSpeed, _carMovement.MaxSpeed, Time.fixedDeltaTime);
            _carMovement.Rigidbody.velocity -= _carMovement.transform.forward * (force + _constantDecelerationForce);

            yield return null;
        }
    }


    private void CallAcceleration(bool isActive, string stopedCoroutine, string startCoroutine)
    {
        _isActivate = isActive;
        StopCoroutine(stopedCoroutine);
        StartCoroutine(startCoroutine);
    }


    protected override void Activate()
    {
        if (_isActivate == false)
            CallAcceleration(true, "SlowDownToMaxSpeed", "Acceleration");
        else
            CallAcceleration(false, "Acceleration", "SlowDownToMaxSpeed");

    }
}