using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PowerWaveCaller : MonoBehaviour
{
    [SerializeField] private Ability _ability;
    [SerializeField, Range(0f, 1f)] private float _chanceOfUse = 0.5f;
    [SerializeField, Min(1)] private int _amountTargetToUse = 1;

    private int _countEnteredToArea = 0;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
            _countEnteredToArea++;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Car")
            _countEnteredToArea--;
    }


    private void Update()
    {
        if (_countEnteredToArea >= _amountTargetToUse && _chanceOfUse >= Random.value)
            _ability.Activate();
    }
}