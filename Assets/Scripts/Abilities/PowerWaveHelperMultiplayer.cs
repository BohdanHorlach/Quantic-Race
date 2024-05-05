using Photon.Pun;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class PowerWaveHelperMultiplayer : MonoBehaviourPun
{
    [SerializeField] private Rigidbody _rigidbody;

    public PhotonView PhotonView;

    private Dictionary<int, SerializableVector3> DeserializeDictionary(byte[] dictionary)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (MemoryStream memoryStream = new MemoryStream(dictionary))
        {
            return (Dictionary<int, SerializableVector3>)binaryFormatter.Deserialize(memoryStream);
        }
    }


    [PunRPC]
    public void Push(byte[] serializedDictionary)
    {
        Dictionary<int, SerializableVector3> pushInfo = DeserializeDictionary(serializedDictionary);

        foreach(KeyValuePair<int, SerializableVector3> pair in pushInfo)
        {
            PhotonView view = PhotonNetwork.GetPhotonView(pair.Key);
            Rigidbody playerBody = view.GetComponent<Rigidbody>();

            if(playerBody != null)
                playerBody.AddForce(pair.Value.ToVector3(), ForceMode.Impulse);
        }
    }
}