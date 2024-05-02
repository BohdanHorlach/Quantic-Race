using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class CarPool : MonoBehaviour
{
    [SerializeField] private GameObject[] _cars;


    public GameObject Spawn(int index)
    {
        GameObject car = _cars[index];

        return PhotonNetwork.Instantiate(car.name, car.transform.position, car.transform.rotation);
    }


    public GameObject SpawnRandom()
    {
        int index = UnityEngine.Random.Range(0, _cars.Length - 1);
        return Spawn(index);
    }
}