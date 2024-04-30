using System.Collections;
using UnityEngine;
using Photon.Pun;


public class Accelerator : Abilitiy
{
    [SerializeField] private PhotonView _photonView;
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
        _isActivate = true;
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
        _isActivate = false;
        _particle.Stop();

        while (_car.CurrentSpeed > _car.MaxSpeed)
        {
            float decelerateForce = Mathf.Lerp(_car.CurrentSpeed, _car.MaxSpeed, Time.fixedDeltaTime) * 0.1f;
            Vector3 force = -_car.transform.forward * _constantDecelerationForce;
            _car.Rigidbody.AddForceAtPosition(force * decelerateForce, _decelerationPoint.position);

            yield return null;
        }
    }


    [PunRPC]
    private void CallAcceleration(string stopedCoroutine, string startCoroutine, int viewID)
    {
        if (_photonView.ViewID != viewID)
            return;

        StopCoroutine(stopedCoroutine);
        StartCoroutine(startCoroutine);
    }


    public override void Activate()
    {
        if (_isActivate == false)
            _photonView.RPC("CallAcceleration", RpcTarget.All, "SlowDownToMaxSpeed", "Acceleration", _photonView.ViewID);
        else
            _photonView.RPC("CallAcceleration", RpcTarget.All, "Acceleration", "SlowDownToMaxSpeed", _photonView.ViewID);
    }
}