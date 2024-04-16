using UnityEngine;


public class CarCameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private bool _isNeedActivate = true;


    public void Enable()
    {
        _virtualCamera.SetActive(_isNeedActivate);
        _playerCamera.SetActive(_isNeedActivate);
    }
}