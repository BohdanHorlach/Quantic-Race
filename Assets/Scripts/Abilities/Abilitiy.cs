using UnityEngine;


public abstract class Abilitiy : MonoBehaviour
{
    public abstract TypeAbility Type { get; }

    public abstract void Activate();
}