using UnityEngine;


public class CarCameraSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private bool _isActive = true;


    public void Enable()
    {
        _virtualCamera.SetActive(_isActive);
        _playerCamera.SetActive(_isActive);
    }
}