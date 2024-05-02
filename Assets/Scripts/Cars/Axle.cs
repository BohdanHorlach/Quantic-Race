using UnityEngine;

[System.Serializable]
public class Axle
{
    [SerializeField] private WheelColliderAndVisual _leftWheel;
    [SerializeField] private WheelColliderAndVisual _rightWheel;
    [SerializeField] private bool isMotor;
    [SerializeField] private bool isSteering;
    [SerializeField] private bool isBreak;


    private void UpdateWheelVisual(WheelColliderAndVisual wheel)
    {
        Vector3 newWheelPositin;
        Quaternion newWheelRotation;

        // get data from collider
        wheel.wheelCollider.GetWorldPose(out newWheelPositin, out newWheelRotation);

        wheel.wheelVisual.position = newWheelPositin;
        wheel.wheelVisual.rotation = newWheelRotation;
    }


    public void SetMotorTorque(float force)
    {
        if (isMotor != true)
            return;

        _leftWheel.wheelCollider.motorTorque = force;
        _rightWheel.wheelCollider.motorTorque = force;
    }


    public void SetSteering(float angle)
    {
        if (isSteering != true)
            return;

        _leftWheel.wheelCollider.steerAngle = angle;
        _rightWheel.wheelCollider.steerAngle = angle;
    }


    public void SetBrakeTorque(float force)
    {
        if (isBreak != true)
            return;

        _leftWheel.wheelCollider.brakeTorque = Mathf.Abs(force);
        _rightWheel.wheelCollider.brakeTorque = Mathf.Abs(force);
    }


    public float GetCurentRPM()
    {
        return _leftWheel.wheelCollider.rpm;
    }


    public void UpdateAxle()
    {
        UpdateWheelVisual(_leftWheel);
        UpdateWheelVisual(_rightWheel);
    }
}