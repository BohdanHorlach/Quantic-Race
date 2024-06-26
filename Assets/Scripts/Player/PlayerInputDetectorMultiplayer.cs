using System;
using UnityEngine;


public class PlayerInputDetectorMultiplayer : InputOfCarMovement
{
    [SerializeField] private AbilityMultiplayer _abilty;
    [SerializeField] private KeyCode _abilitiesKeyKode;

    public override event Action<float> InputHorizontal;
    public override event Action<float> InputVertical;
    public override event Action<float> InputBrake;
    public override event Action InputResetCoordinats;

    private static bool _isInputPaused = false;

    public override bool IsCanMove { get; set; }

    private void FixedUpdate()
    {
        if (_isInputPaused == false)
        {
            ReadMoveInput();
            ReadBrakeInput();
            ReadAbilityUseInput();
            ReadResetPositionInput();
        }
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


    private void ReadAbilityUseInput()
    {
        if (Input.GetKeyDown(_abilitiesKeyKode) == true)
            _abilty.Activate();
    }

    private void ReadResetPositionInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            InputResetCoordinats?.Invoke();
        }
    }

    public static void PauseInput()
    {
        _isInputPaused = true;
    }

    public static void ResumeInput()
    {
        _isInputPaused = false;
    }
}