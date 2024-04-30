using Photon.Pun;


public abstract class Abilitiy : MonoBehaviourPun
{
    public abstract TypeAbility Type { get; }

    public abstract void Activate();
}