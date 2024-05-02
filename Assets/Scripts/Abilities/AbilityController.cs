using UnityEngine;
using System.Collections;
using System;
using Photon.Pun;

public class AbilityController : Ability
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Ability _ability;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private int _maxChargeCount;

    private int _currentChargeCount = 0;
    private bool _isActive = true;

    public override TypeAbility Type { get => _ability.Type; }
    public event Action UseAbility;


    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldownTime);

        _isActive = true;
    }


    private void Recharge()
    {
        _currentChargeCount--;
        _isActive = false;
        StartCoroutine("Cooldown");
    }


    public override void Activate()
    {
        if (_isActive == false || _currentChargeCount <= 0 || _photonView.IsMine == false)
            return;

        _ability.Activate();
        Recharge();

        UseAbility?.Invoke();
    }


    public bool AddCharge(int amountCharges)
    {
        if(_currentChargeCount >= _maxChargeCount)
            return false;

        _currentChargeCount = Mathf.Clamp(_currentChargeCount + amountCharges, 0, _maxChargeCount);
        return true;
    }
}