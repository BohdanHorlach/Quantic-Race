using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PowerWave : Abilities
{
    [SerializeField, Min(1)] private float _pushForce = 10f;

    private HashSet<Collider> _enteredTheCoverageArea;


    private void Start()
    {
        _enteredTheCoverageArea = new HashSet<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
            _enteredTheCoverageArea.Add(other);
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Car")
            _enteredTheCoverageArea.Remove(other);
    }


    protected override void Activate()
    {
        foreach(Collider collider in _enteredTheCoverageArea){
            Vector3 direction = collider.transform.position - transform.position;
            float force = _pushForce / Vector3.Distance(transform.position, collider.transform.position);

            CarMovement car = collider.GetComponent<CarMovement>();
            car.Rigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
    }
}