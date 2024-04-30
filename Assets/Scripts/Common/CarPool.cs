using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class CarPool : MonoBehaviour
{
    [SerializeField] private GameObject[] _cars;


    public Transform Spawn(int index)
    {
        GameObject car = _cars[index];

        return PhotonNetwork.Instantiate(car.name, car.transform.position, car.transform.rotation).transform;
    }


    public Transform SpawnRandom()
    {
        int index = UnityEngine.Random.Range(0, _cars.Length - 1);
        return Spawn(index);
    }


    public GameObject GetCarFromIndex(int index)
    {
        if (index >= _cars.Length || index < 0)
            throw new IndexOutOfRangeException();

        return _cars[index];
    }
}