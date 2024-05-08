using UnityEngine;


public class ColliderDisabler : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private FinishHandler _finishHandler;


    public void CallDisableByFinishHandler()
    {
        _finishHandler.DisableCollider();
    }


    public void Disable()
    {
        _collider.enabled = false;
    }
}