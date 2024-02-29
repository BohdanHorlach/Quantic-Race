using System;
using UnityEngine;


public class PlayerInputDetector : InputOfCarMovement
{
    public override event Action<float> InputHorizontal;
    public override event Action<float> InputVertical;
    public override event Action<float> InputBrake;


    private void FixedUpdate()
    {
        ReadMoveInput();
        ReadBrakeInput();
    }


    private void ReadMoveInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        InputHorizontal?.Invoke(horizontalInput);
        InputVertical?.Invoke(verticalInput);
    }


    private void ReadBrakeInput()
    {
        float brakeInput = Input.GetAxis("Jump");

        InputBrake?.Invoke(brakeInput);
    }
}