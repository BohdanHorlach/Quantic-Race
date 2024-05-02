using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FastWayPannel : MonoBehaviour
{
    [SerializeField] private GameObject _solidPart;
    [SerializeField] private GameObject _brokenPart;

    // top parent object collider to detect when to break
    [SerializeField] private BoxCollider _collider;

    void Start()
    {
        _collider.isTrigger = true;
        _solidPart.SetActive(true);
        _brokenPart.SetActive(false);
    }

    public void Break()
    {
        _solidPart.SetActive(false);
        _brokenPart.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AbilitiyController controller))
        {
            if (controller.IsActive())
            {
                Break();
            }
        }
    }


}
