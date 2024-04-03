using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class AbilitiesController : Abilities
{
    [SerializeField] private InputOfCarMovement _input;
    [SerializeField] private Abilities _abilities;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _cooldown;
    [SerializeField] private int _maxChargeCount;

    private float _timeOfLastUsed;
    private int _currentChargeCount = 0;

    private bool _isActive;


    private void Start()
    {
        _timeOfLastUsed = _cooldown;
    }


    private void OnEnable()
    {
        _input.UseAbility += Activate;
    }


    private void OnDisable()
    {
        _input.UseAbility -= Activate;
    }


    private void Update()
    {
        _slider.value = _currentChargeCount;
    }


    private IEnumerator StartTimer()
    {
        _timeOfLastUsed = 0f;

        while (_timeOfLastUsed < _cooldown)
        {
            _timeOfLastUsed += Time.deltaTime;
            yield return null;
        }
    }


    public override void Activate()
    {
        if (_isActive == false && _timeOfLastUsed < _cooldown)
            return;

        _abilities.Activate();

        _currentChargeCount--;
        StartCoroutine("StartTimer");
    }


    public void AddCharge(int amountCharges)
    {
        _currentChargeCount = Mathf.Clamp(_currentChargeCount + amountCharges, 0, _maxChargeCount);
    }
}