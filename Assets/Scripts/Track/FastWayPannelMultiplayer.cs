using UnityEngine;
using Photon.Pun;


public class FastWayPannelMultiplayer : MonoBehaviourPun
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private GameObject _solidPart;
    [SerializeField] private GameObject _brokenPart;
    [SerializeField] private BoxCollider _collider;

    void Start()
    {
        _collider.isTrigger = true;
        _solidPart.SetActive(true);
        _brokenPart.SetActive(false);
    }


    [PunRPC]
    private void BreakRPC(int viewID)
    {
        if (_photonView.ViewID != viewID)
            return;

        _solidPart.SetActive(false);
        _brokenPart.SetActive(true);
    }


    public void Break()
    {
        _photonView.RPC("BreakRPC", RpcTarget.All, _photonView.ViewID);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AbilityControllerMultiplayer controller))
        {
            if (controller.IsActive())
            {
                Break();
            }
        }
    }


}
