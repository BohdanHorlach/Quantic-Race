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
        if(other.TryGetComponent(out AbilitiyController abilitiesController) == true)
            GiveCharge(abilitiesController);
    }


    private IEnumerator Recovery()
    {
        yield return new WaitForSeconds(_recoveryTime);

        _collider.enabled = true;
    }


    private void GiveCharge(AbilitiyController abilitiesController)
    {
        _collider.enabled = false;

        abilitiesController.AddCharge(_amountAddedCharges);
        StartCoroutine(Recovery());
    }
}