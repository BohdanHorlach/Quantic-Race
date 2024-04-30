using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class PowerWave : Abilitiy
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private PowerWaveHelper _powerWaveHelper;
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


    private byte[] SerializeDictionary(Dictionary<int, SerializableVector3> dictionary)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (MemoryStream memoryStream = new MemoryStream())
        {
            binaryFormatter.Serialize(memoryStream, dictionary);
            return memoryStream.ToArray();
        }
    }


    private void PushCarsNearby()
    {
        Dictionary<int, SerializableVector3> playerIDsForceToPush = new Dictionary<int, SerializableVector3>();
 
        foreach (PhotonView player in _enteredTheCoverageArea)
        {
            Vector3 direction = player.transform.position - transform.position;
            float forceByDistance = _collider.radius / Vector3.Distance(transform.position, player.transform.position);
            
            Vector3 resultForce = direction.normalized * forceByDistance * _pushForce;
            playerIDsForceToPush.Add(player.ViewID, new SerializableVector3(resultForce));
        }

        byte[] serializedDictionary = SerializeDictionary(playerIDsForceToPush);
        _powerWaveHelper.PhotonView.RPC("Push", RpcTarget.All, serializedDictionary);
    }


    [PunRPC]
    private void PlayEffects(int viewID)
    {
        if (_photonView.ViewID == viewID)
            _particle.Play();
    }

    public override void Activate()
    {
        PushCarsNearby();
        _photonView.RPC("PlayEffects", RpcTarget.All, _photonView.ViewID);
    }
}