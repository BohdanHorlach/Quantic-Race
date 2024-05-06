using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PowerWaveMultiplayer : AbilityMultiplayer
{
    [SerializeField] private PhotonPowerWaveCaller _photonPowerWaveCaller;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField, Min(1)] private float _pushForce = 10f;
    [SerializeField] private SphereCollider _collider;

    private const string CAR_TAG = "Car";
    private const string BREACABLE_WALL_TAG = "BreakableWall";

    private HashSet<PhotonView> _enteredTheCoverageArea;
    private TypeAbility _type = TypeAbility.PowerWave;

    public override TypeAbility Type { get => _type; }


    private void Awake()
    {
        _enteredTheCoverageArea = new HashSet<PhotonView>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (CanInteract(other))
            _enteredTheCoverageArea.Add(other.GetComponent<PhotonView>());
    }


    private void OnTriggerExit(Collider other)
    {
        if (CanInteract(other))
            _enteredTheCoverageArea.Remove(other.GetComponent<PhotonView>());
    }


    private bool CanInteract(Collider other)
    {
        if (other.tag == CAR_TAG) return true;
        if (other.tag == BREACABLE_WALL_TAG) return true;
        return false;
    }


    public Dictionary<int, SerializableVector3> GetPushCarsNearbyInfo()
    {
        Dictionary<int, SerializableVector3> playerIDsForceToPush = new Dictionary<int, SerializableVector3>();

        foreach (PhotonView player in _enteredTheCoverageArea)
        {
            if (player.TryGetComponent(out FastWayPannelMultiplayer pannel))
            {
                pannel.Break();
            }
            else
            {
                Vector3 direction = player.transform.position - transform.position;
                float forceByDistance = _collider.radius / Vector3.Distance(transform.position, player.transform.position);

                Vector3 resultForce = direction.normalized * forceByDistance * _pushForce;
                playerIDsForceToPush.Add(player.ViewID, new SerializableVector3(resultForce));
            }
        }

        return playerIDsForceToPush;
    }


    public void PlayEffects()
    {
        _particle.Play();
    }


    public override void Activate()
    {
        _photonPowerWaveCaller.RunRPC();
    }


    public override bool IsActive()
    {
        return false;
    }
}