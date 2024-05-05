using Photon.Pun;


public abstract class AbilityMultiplayer : MonoBehaviourPun
{
    public abstract TypeAbility Type { get; }

    public abstract void Activate();
    public abstract bool IsActive();
}