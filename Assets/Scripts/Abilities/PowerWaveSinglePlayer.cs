using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PowerWaveSinglePlayer : AbilitySinglePlayer
{
    private const string CAR_TAG = "Car";
    private const string BREACABLE_WALL_TAG = "BreakableWall";


    [SerializeField] private ParticleSystem _particle;
    [SerializeField, Min(1)] private float _pushForce = 10f;
    [SerializeField] private SphereCollider _collider;

    private HashSet<Transform> _enteredTheCoverageArea;
    private TypeAbility _type = TypeAbility.PowerWave;

    public override TypeAbility Type { get => _type; }


    private void Awake()
    {       
        _enteredTheCoverageArea = new HashSet<Transform>();
    }

    private bool CanInteract(Collider other)
    {
        if (other.tag == CAR_TAG) return true;
        if (other.tag == BREACABLE_WALL_TAG) return true;
        return false;
    }

    private void Interract(Transform interactableObject)
    {
        if (interactableObject.TryGetComponent(out CarMovementSinglePlayer car))
        {
            Vector3 direction = car.transform.position - transform.position;
            float force = _collider.radius / Vector3.Distance(transform.position, car.transform.position);
            car.Rigidbody.AddForce(direction.normalized * force * _pushForce, ForceMode.Impulse);
        }
        if (interactableObject.TryGetComponent(out FastWayPannel pannel))
        {
            pannel.Break();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (CanInteract(other))
            _enteredTheCoverageArea.Add(other.GetComponent<Transform>());
    }


    private void OnTriggerExit(Collider other)
    {
        if (CanInteract(other))
            _enteredTheCoverageArea.Remove(other.GetComponent<Transform>());
    }


    // TODO direct push vector higher
    // if power wave is from behind it's accelerate opponent
    private void PushCarsNearby()
    {
        foreach (Transform interactableObject in _enteredTheCoverageArea)
        {
            Interract(interactableObject);
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