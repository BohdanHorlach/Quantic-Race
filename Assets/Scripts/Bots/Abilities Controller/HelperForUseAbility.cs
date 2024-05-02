using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(SphereCollider))]
public class HelperForUseAbility : MonoBehaviour
{
    [SerializeField] private AbilityController _abilityController;
    [SerializeField] private TypeAbility _targetType;
    [SerializeField, Range(0.1f, 1f)] private float _chanceOfRetaliation = 0.5f;

    private Dictionary<Collider, HelperForUseAbility> _enteredTheCoverageArea;

    public event Action<TypeAbility> UseAbility;


    private void Awake()
    {
        _enteredTheCoverageArea = new Dictionary<Collider, HelperForUseAbility>();
    }


    public void OnEnable()
    {
        _abilityController.UseAbility += InformTheNearestCars;
    }


    private void OnDisable()
    {
        _abilityController.UseAbility -= InformTheNearestCars;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<HelperForUseAbility>(out HelperForUseAbility helper)) 
        {
            helper.UseAbility += CounterUseOfAbility;
            _enteredTheCoverageArea.Add(other, helper);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(_enteredTheCoverageArea.ContainsKey(other) == true)
        {
            _enteredTheCoverageArea[other].UseAbility -= CounterUseOfAbility;
            _enteredTheCoverageArea.Remove(other);
        }
    }


    private void CounterUseOfAbility(TypeAbility targetType)
    {
        if (_targetType != targetType)
            return;

        float randomValue = UnityEngine.Random.value;

        if (randomValue <= _chanceOfRetaliation)
            _abilityController.Activate();
    }


    private void InformTheNearestCars()
    {
        UseAbility?.Invoke(_abilityController.Type);
    }
}