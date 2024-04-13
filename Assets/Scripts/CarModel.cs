using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarModel : MonoBehaviour
{
    [SerializeField] private GameObject[] CarModels;

    private void Awake()
    {
        ChoosenCar(SaveManager.instance.CurrCar);
    }

    private void ChoosenCar(int temp)
    {
        Instantiate(CarModels[temp], transform.position, Quaternion.identity, transform);
    }

}
