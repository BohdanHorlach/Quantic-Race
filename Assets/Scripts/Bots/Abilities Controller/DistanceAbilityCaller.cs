using UnityEngine;


public class DistanceAbilityCaller : MonoBehaviour
{
    [SerializeField] private Abilitiy _ability;
    [SerializeField] private CheckPointHandler _checkPointHandler;
    [SerializeField] private PositionCalculator _positionCalculator;
    [SerializeField, Min(0)] private float _distanceToTargetToUse;
    [SerializeField, Range(0f, 1f)] private float _chanceOfUse = 0.5f;
    [SerializeField] private bool _isTeleport;


    private void Start()
    {
        if (_positionCalculator == null)
            _positionCalculator = SaveManager.instance.PositionCalculator;
    }


    private void Update()
    {
        if (_positionCalculator == null)
            return;

        float distance = _positionCalculator.DistanceToNextOpponent(_checkPointHandler);

        bool isTargetDistance = _isTeleport ? distance <= _distanceToTargetToUse : distance >= _distanceToTargetToUse;

        if (isTargetDistance && Random.value <= _chanceOfUse)
            _ability.Activate();
    }
}