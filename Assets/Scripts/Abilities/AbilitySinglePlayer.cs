using UnityEngine;


public abstract class AbilitySinglePlayer : MonoBehaviour
{
    public abstract TypeAbility Type { get; }

    public abstract void Activate();
    public abstract bool IsActive();
}