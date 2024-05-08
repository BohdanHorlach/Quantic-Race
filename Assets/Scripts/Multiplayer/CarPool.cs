using UnityEngine;
using Photon.Pun;


public class CarPool : MonoBehaviour
{
    [SerializeField] private GameObject[] _cars;


    public GameObject GetCarFromIndex(int index)
    {
        return _cars[index];
    }


    public GameObject GetRandomCar()
    {
        int index = UnityEngine.Random.Range(0, _cars.Length - 1);
        return GetCarFromIndex(index);
    }
}