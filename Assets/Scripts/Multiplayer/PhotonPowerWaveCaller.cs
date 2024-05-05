using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Pun;


public class PhotonPowerWaveCaller : MonoBehaviourPun
{
    [SerializeField] private PowerWaveMultiplayer _powerWave;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private PowerWaveHelperMultiplayer _powerWaveHelper;


    private byte[] SerializeDictionary(Dictionary<int, SerializableVector3> dictionary)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (MemoryStream memoryStream = new MemoryStream())
        {
            binaryFormatter.Serialize(memoryStream, dictionary);
            return memoryStream.ToArray();
        }
    }


    [PunRPC]
    public void Activate(int viewID, byte[] serializedDictionary)
    {
        if (_photonView.ViewID != viewID)
            return;

        _powerWave.PlayEffects();
        _powerWaveHelper.PhotonView.RPC("Push", RpcTarget.All, serializedDictionary);
    }


    public void RunRPC()
    {        
        Dictionary<int, SerializableVector3> playerIDsForceToPush = _powerWave.GetPushCarsNearbyInfo();
        byte[] serializedDictionary = SerializeDictionary(playerIDsForceToPush);

        _photonView.RPC("Activate", RpcTarget.All, _photonView.ViewID, serializedDictionary);
    }
}