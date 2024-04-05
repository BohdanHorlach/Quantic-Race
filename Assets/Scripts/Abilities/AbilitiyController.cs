using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class AbilitiyController : Abilitiy
{
    [SerializeField] private InputOfCarMovement _input;
    [SerializeField] private Abilitiy _ability;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _cooldownTime;
    [SerializeField] private int _maxChargeCount;

    private int _currentChargeCount = 0;
    private bool _isActive = true;

    public override TypeAbility Type { get => _ability.Type; }
    public event Action UseAbility;


    private void Update()
    {
        _slider.value = _currentChargeCount;
    }


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
        if (_isActive == false || _currentChargeCount <= 0)
            return;

        _ability.Activate();
        Recharge();

        UseAbility?.Invoke();
    }


    public void AddCharge(int amountCharges)
    {
        _currentChargeCount = Mathf.Clamp(_currentChargeCount + amountCharges, 0, _maxChargeCount);
    }
}