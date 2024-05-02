using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PowerWave : Ability
{
    [SerializeField] private PhotonPowerWaveCaller _photonPowerWaveCaller;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField, Min(1)] private float _pushForce = 10f;
    [SerializeField] private SphereCollider _collider;

    private HashSet<PhotonView> _enteredTheCoverageArea;
    private TypeAbility _type = TypeAbility.PowerWave;

    public override TypeAbility Type { get => _type; }


    private void Awake()
    {
        _enteredTheCoverageArea = new HashSet<PhotonView>();        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Car")
            _enteredTheCoverageArea.Add(other.GetComponent<PhotonView>());
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Car")
            _enteredTheCoverageArea.Remove(other.GetComponent<PhotonView>());
    }


    public Dictionary<int, SerializableVector3> GetPushCarsNearbyInfo()
    {
        Dictionary<int, SerializableVector3> playerIDsForceToPush = new Dictionary<int, SerializableVector3>();

        foreach (PhotonView player in _enteredTheCoverageArea)
        {
            Vector3 direction = player.transform.position - transform.position;
            float forceByDistance = _collider.radius / Vector3.Distance(transform.position, player.transform.position);

            Vector3 resultForce = direction.normalized * forceByDistance * _pushForce;
            playerIDsForceToPush.Add(player.ViewID, new SerializableVector3(resultForce));
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
}