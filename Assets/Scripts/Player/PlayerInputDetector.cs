using System;
using UnityEngine;


public class PlayerInputDetector : InputOfCarMovement
{
    [SerializeField] private Abilitiy _abilty;
    [SerializeField] private KeyCode _abilitiesKeyKode;

    public override event Action<float> InputHorizontal;
    public override event Action<float> InputVertical;
    public override event Action<float> InputBrake;

    public override bool IsCanMove { get; set; }

    private void FixedUpdate()
    {
        ReadMoveInput();
        ReadBrakeInput();
        DetectInputOfAbilityUse();
    }


    private void Update()
    {
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
            _abilty.Activate();
    }
}