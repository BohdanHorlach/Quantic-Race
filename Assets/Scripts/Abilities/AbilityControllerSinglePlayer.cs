using UnityEngine;
using System.Collections;
using System;



public class AbilityControllerSinglePlayer : AbilitySinglePlayer
{
    [SerializeField] private AbilitySinglePlayer _ability;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private int _maxChargeCount;

    private int _currentChargeCount = 0;
    private bool _canActivate = true;

    public override TypeAbility Type { get => _ability.Type; }
    public event Action UseAbility;

    public override bool IsActive()
    {
        return _ability.IsActive();
    }


    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);

        _canActivate = true;
    }


    private void Recharge()
    {
        _currentChargeCount--;
        _canActivate = false;
        StartCoroutine(Cooldown());
    }


    public override void Activate()
    {
        if (_canActivate == false || _currentChargeCount <= 0)
            return;

        _ability.Activate();
        Recharge();

        UseAbility?.Invoke();
    }


    public void AddCharge(int amountCharges)
    {
        _currentChargeCount = Mathf.Clamp(_currentChargeCount + amountCharges, 0, _maxChargeCount);
    }

    //public int GetMaxChargeCount()
    //{
    //    return _maxChargeCount;
    //}

    public int GetCurrentChargeCount()
    {
        return _currentChargeCount;
    }

    public float GetChargeFillPercentage()
    {
        return (float)_currentChargeCount / _maxChargeCount;
    }
}