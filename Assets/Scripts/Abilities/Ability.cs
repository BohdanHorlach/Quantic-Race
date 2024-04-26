using UnityEngine;


public abstract class Ability : MonoBehaviour
{
    public abstract TypeAbility Type { get; }

    public abstract void Activate();
    public abstract bool IsActive();
}