using UnityEngine;
using Photon.Pun;

<<<<<<< Updated upstream
public class CarMovement : MonoBehaviourPunCallbacks
{
<<<<<<< Updated upstream
=======
    //[SerializeField] PhotonView _photonView;
   // [Space]

>>>>>>> Stashed changes
=======

    public PhotonView playerPrefab;

    public class CarMovement : MonoBehaviourPunCallbacks
    {
    public PhotonView _playerPrefab;
>>>>>>> Stashed changes
    [SerializeField] private InputOfCarMovement input;
    [SerializeField] private Axle[] _axles;
    [SerializeField, Min(1)] private float _motorForce;
    [SerializeField, Min(1)] private float _brakeForce;
    [SerializeField, Min(1)] private float _steeringAngleForce;
    [SerializeField, Min(1)] private float _handbrakeForce;
    [SerializeField, Min(1)] private float _dividerForReverceForce;


    private void OnEnable()
    {
        
        input.InputHorizontal += Steering;
        input.InputVertical += InputVerticalProcessing;
        input.InputBrake += Brake;
    }
    
    private void OnDisable()
    {
        input.InputHorizontal -= Steering;
        input.InputVertical -= InputVerticalProcessing;
        input.InputBrake -= Brake;
    }
    


    private void Update()
    {
<<<<<<< Updated upstream
        if (photonView.IsMine)
        {

=======
        if (_playerPrefab.IsMine)
        {
>>>>>>> Stashed changes
            foreach (Axle axle in _axles)
                axle.UpdateAxle();
        }
    }


    private void Gas(float force)
    {
        foreach(Axle axle in _axles) 
            axle.SetMotorTorque(force);
    }


    private void Steering(float force)
    {
        foreach (Axle axle in _axles)
            axle.SetSteering(force * _steeringAngleForce);
    }


    private void Brake(float force)
    {
        foreach (Axle axle in _axles)
            axle.SetBrakeTorque(force * _handbrakeForce);
    }


    private void InputVerticalProcessing(float value)
    {
        float force = 0;

        if (value >= 0 || value < 0 && _axles[0].GetCurentRPM() <= 0)
        {
            force = value >= 0 ? _motorForce : _motorForce / _dividerForReverceForce;
            force *= value;
        }
        else if (value < 0 && _axles[0].GetCurentRPM() > 0)
        {
            force = value * _brakeForce;
        }
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        if (photonView.IsMine)
=======
        if (_playerPrefab.IsMine)
>>>>>>> Stashed changes
        {
            Gas(force);
        }
    }
}