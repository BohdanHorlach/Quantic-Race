using UnityEngine;

[System.Serializable]
public class Axle
{
    [SerializeField] private WheelData _leftWheel;
    [SerializeField] private WheelData _rightWheel;
    [SerializeField] private bool isMotor;
    [SerializeField] private bool isSteering;
    [SerializeField] private bool isBreak;


    private void UpdateTransformDataFromWheel(WheelData wheel)
    {
        Vector3 positionWheel;
        Quaternion rotationWheel;

        wheel.wheelCollider.GetWorldPose(out positionWheel, out rotationWheel);

        wheel.wheelTransform.position = positionWheel;
        wheel.wheelTransform.rotation = rotationWheel;
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
        UpdateTransformDataFromWheel(_leftWheel);
        UpdateTransformDataFromWheel(_rightWheel);
    }
}