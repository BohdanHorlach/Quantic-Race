using System.Collections;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class ChargeStation : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField, Range(1, 5)] private int _amountAddedCharges = 1;
    [SerializeField, Min(1)] private float _recoveryTime = 1;


    private void Awake()
    {
        _collider.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AbilityControllerSinglePlayer controllerSinglePlayer) == true)
            GiveCharge(controllerSinglePlayer);
        else if (other.TryGetComponent(out AbilityControllerMultiplayer controllerMultiplayer) == true)
            GiveCharge(controllerMultiplayer);
    }


    private IEnumerator Recovery()
    {
        yield return new WaitForSeconds(_recoveryTime);

        _collider.enabled = true;
    }


    private void GiveCharge(AbilityControllerSinglePlayer abilitiesController)
    {
        _collider.enabled = false;

        abilitiesController.AddCharge(_amountAddedCharges);
        StartCoroutine(Recovery());
    }


    //TODO: Add ChargeStationMultyplayer class, and use [PubRPC] for syncronazed state
    private void GiveCharge(AbilityControllerMultiplayer abilitiesController)
    {
        _collider.enabled = false;

        abilitiesController.AddCharge(_amountAddedCharges);
        StartCoroutine(Recovery());
    }
}