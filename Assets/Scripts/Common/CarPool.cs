using System;
using System.Collections.Generic;
using UnityEngine;


public class CarPool : MonoBehaviour
{
    [SerializeField] private GameObject[] _cars;

    private Dictionary<GameObject, bool> _spawnedCars;


    private void Awake()
    {
        _spawnedCars = new Dictionary<GameObject, bool>();

        foreach (GameObject car in _cars)
            _spawnedCars.Add(car, false);
    }


    public Transform Spawn(int index)
    {
        GameObject car = _cars[index];
        bool isSpawned = _spawnedCars[car];

        if (isSpawned == true)
            return Instantiate(car).transform;

        car.SetActive(true);
        _spawnedCars[car] = true;
        return car.transform;
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