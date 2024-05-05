using UnityEngine;
using Photon.Pun;


public class FinishHandler : MonoBehaviourPun
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Animator _animator;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private CheckPointHandler _checkPointHandler;


    private void Awake()
    {
        _checkPointHandler.handleFinishing += SwitchMode;
    }


    [PunRPC]
    private void DisableColliderRPC(int viewID)
    {
        if(_photonView.ViewID == viewID)
            _collider.enabled = false;
    }


    private void DisableCollider()
    {
        _photonView.RPC("DisableColliderRPC", RpcTarget.All, _photonView.ViewID);
    }


    [PunRPC]
    private void DisableCar(int viewID)
    {
        if(_photonView.ViewID == viewID)
            _animator.SetTrigger("Finished");
    }


    private void SwitchMode()
    {
        _photonView.RPC("DisableCar", RpcTarget.All, _photonView.ViewID);
    }
}