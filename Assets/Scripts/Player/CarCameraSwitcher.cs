using UnityEngine;


public class CarCameraSwitcher : MonoBehaviour
{
    [SerializeField] GameObject _playerCamera;
    [SerializeField] GameObject _virtualCamera;


    public void Enable()
    {
        _virtualCamera.SetActive(true);
        _playerCamera.SetActive(true);
    }
}