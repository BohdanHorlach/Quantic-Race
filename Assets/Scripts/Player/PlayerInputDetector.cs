using System;
using UnityEngine;


public class PlayerInputDetector : InputOfCarMovement
{
    [SerializeField] private KeyCode _abilitiesKeyKode;

    public override event Action<float> InputHorizontal;
    public override event Action<float> InputVertical;
    public override event Action<float> InputBrake;
    public override event Action UseAbility;


    private void FixedUpdate()
    {
        ReadMoveInput();
        ReadBrakeInput();
        DetectInputOfAbilityUse();
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


    private void DetectInputOfAbilityUse()
    {
        if (Input.GetKeyDown(_abilitiesKeyKode) == true)
            UseAbility?.Invoke();
    }
}