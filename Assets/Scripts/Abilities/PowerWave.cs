using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PowerWave : Abilities
{
    [SerializeField] private ParticleSystem _particle;
    [SerializeField, Min(1)] private float _pushForce = 10f;
    [SerializeField] private SphereCollider _collider;

    private HashSet<CarMovement> _enteredTheCoverageArea;


    private void Awake()
    {
        _enteredTheCoverageArea = new HashSet<CarMovement>();        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
            _enteredTheCoverageArea.Add(other.GetComponent<CarMovement>());
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Car")
            _enteredTheCoverageArea.Remove(other.GetComponent<CarMovement>());
    }


    private void PushCarsNearby()
    {
        foreach (CarMovement car in _enteredTheCoverageArea)
        {
            Vector3 direction = car.transform.position - transform.position;
            float force = _collider.radius / Vector3.Distance(transform.position, car.transform.position);

            car.Rigidbody.AddForce(direction.normalized * force * _pushForce, ForceMode.Impulse);
        }
    }


    private void PlayEffects()
    {
        _particle.Play();

    }


    public override void Activate()
    {
        PushCarsNearby();
        PlayEffects();
    }
}