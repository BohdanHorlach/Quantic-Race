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
        bool isAdded = abilitiesController.AddCharge(_amountAddedCharges);

        if (isAdded == false)
            return;

        _collider.enabled = false;
        StartCoroutine("Recovery");
    }
}