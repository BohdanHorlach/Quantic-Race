using System.Collections;
using UnityEngine;


public class DistanceAbilityCallerMultiplayer : MonoBehaviour
{
    [SerializeField] private AbilityMultiplayer _ability;
    [SerializeField] private CheckPointHandler _checkPointHandler;
    [SerializeField] private PositionCalculator _positionCalculator;
    [SerializeField, Min(0)] private float _distanceToTargetToUse;
    [SerializeField, Range(0f, 1f)] private float _chanceOfUse = 0.5f;
    [SerializeField] private bool _isTeleport;

    private float ABILITY_SECONDS_DALAY = 10;
    private bool _isActive;



    private void Awake()
    {
        _isActive = false;
    }

    private void Update()
    {
        //if (_positionCalculator == null)
        //    return;

        //float distance = _positionCalculator.DistanceToNextOpponent(_checkPointHandler);
        bool canAbilityImprovePosition = true;

        //if (_isTeleport && distance <= _distanceToTargetToUse)
        //{
        //    // if teleport and oponent is not too far from the car
        //    canAbilityImprovePosition = true;
        //}
        //if (_isTeleport == false && distance >= _distanceToTargetToUse)
        //{
        //    // if accelerator and oponent is too far from the car
        //    canAbilityImprovePosition = true;
        //}

        if (canAbilityImprovePosition && Random.value <= _chanceOfUse)
        {
            if (_isActive) return;
            _ability.Activate();
            StartCoroutine(AcceleratorDelay());
        }



    }
    private IEnumerator AcceleratorDelay()
    {
        _isActive = true;
        yield return new WaitForSeconds(ABILITY_SECONDS_DALAY);
        _isActive = false;
    }
}