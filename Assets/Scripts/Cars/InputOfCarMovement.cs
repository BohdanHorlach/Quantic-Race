using System;
using UnityEngine;


public abstract class InputOfCarMovement : MonoBehaviour
{
    public abstract event Action<float> InputHorizontal;
    public abstract event Action<float> InputVertical;
    public abstract event Action<float> InputBrake;

    public abstract bool IsCanMove { get; set; }
}