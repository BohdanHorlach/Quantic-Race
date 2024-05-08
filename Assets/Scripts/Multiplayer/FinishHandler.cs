using UnityEngine;
using Photon.Pun;


public class FinishHandler : MonoBehaviourPun
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _miniMapIcon;
    [SerializeField] private CarCameraSwitcher _cameraSwitcher;
    [SerializeField] private ColliderDisabler _colliderDisabler;
    [SerializeField] private CheckPointHandler _checkPointHandler;


    private void Awake()
    {
        _checkPointHandler.handleFinishing += SwitchMode;
    }



    [PunRPC]
    private void DisableColliderRPC(int viewID)
    {
        if(_photonView.ViewID != viewID)
            return;

        _colliderDisabler.Disable();
        _miniMapIcon.SetActive(false);
    }


    public void DisableCollider()
    {
        _photonView.RPC("DisableColliderRPC", RpcTarget.All, _photonView.ViewID);
    }



    [PunRPC]
    private void DisableCar(int viewID)
    {
        if(_photonView.ViewID != viewID)
            return;

        _animator.SetTrigger("Finished");
        _cameraSwitcher.FreeCamera();
    }


    private void SwitchMode()
    {
        _photonView.RPC("DisableCar", RpcTarget.All, _photonView.ViewID);
    }
}