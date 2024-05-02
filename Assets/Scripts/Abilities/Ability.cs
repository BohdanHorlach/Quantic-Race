using Photon.Pun;


public abstract class Ability : MonoBehaviourPun
{
    public abstract TypeAbility Type { get; }

    public abstract void Activate();
}