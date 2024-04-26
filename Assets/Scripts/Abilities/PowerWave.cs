using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PowerWave : Ability
{
    [SerializeField] private ParticleSystem _particle;
    [SerializeField, Min(1)] private float _pushForce = 10f;
    [SerializeField] private SphereCollider _collider;

    //private HashSet<CarMovement> _enteredTheCoverageArea;
    private HashSet<Transform> _enteredTheCoverageArea;
    private TypeAbility _type = TypeAbility.PowerWave;

    public override TypeAbility Type { get => _type; }


    private void Awake()
    {
        //_enteredTheCoverageArea = new HashSet<CarMovement>();        
        _enteredTheCoverageArea = new HashSet<Transform>();        
    }


    // TODO change tag to something like Pushable
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
            _enteredTheCoverageArea.Add(other.GetComponent<Transform>());
            //_enteredTheCoverageArea.Add(other.GetComponent<CarMovement>());
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Car")
            _enteredTheCoverageArea.Remove(other.GetComponent<Transform>());
            //_enteredTheCoverageArea.Remove(other.GetComponent<CarMovement>());
    }


    // TODO direct push vector higher
    // if power wave is from behind it's accelerate opponent
    private void PushCarsNearby()
    {
        foreach (Transform car in _enteredTheCoverageArea)
        //foreach (CarMovement car in _enteredTheCoverageArea)
        {
            Vector3 direction = car.transform.position - transform.position;
            float force = _collider.radius / Vector3.Distance(transform.position, car.transform.position);

            car.GetComponent<Rigidbody>().AddForce(direction.normalized * force * _pushForce, ForceMode.Impulse);
            //car.Rigidbody.AddForce(direction.normalized * force * _pushForce, ForceMode.Impulse);
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

    public override bool IsActive()
    {
        return false;
    }
}