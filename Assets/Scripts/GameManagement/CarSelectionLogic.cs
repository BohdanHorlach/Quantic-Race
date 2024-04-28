using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectionLogic : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI carTitle;
    [SerializeField] private CarInformationSO[] carInformationSOArray;
    private List<GameObject> cars;

    private int selectedCarIndex = 0; // Keeps track of the currently selected car

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.childCount);

        // If no car prefabs are assigned, handle the error
        if (carInformationSOArray.Length == 0)
        {
            Debug.LogError("No car prefabs assigned!");
            return;
        }

        InstantiateAllCars();
        HideAllCars();

        // Set initial car model and name
        UpdateCarSelection(selectedCarIndex);
    }

    public void OnNextButtonPressed()
    {
        HideCar(selectedCarIndex);

        selectedCarIndex++;
        if (selectedCarIndex >= carInformationSOArray.Length)
        {
            selectedCarIndex = 0; // Wrap around if reaching the end
        }
        UpdateCarSelection(selectedCarIndex);
    }

    public void OnPrevButtonPressed()
    {
        HideCar(selectedCarIndex);

        selectedCarIndex--;
        if (selectedCarIndex < 0)
        {
            selectedCarIndex = carInformationSOArray.Length - 1; // Wrap around if reaching the beginning
        }
        UpdateCarSelection(selectedCarIndex);
    }

    private void InstantiateAllCars()
    {
        cars = new List<GameObject>();
        foreach (CarInformationSO carInformationSO in carInformationSOArray)
        {
            cars.Add(Instantiate(carInformationSO.visualPrefab, transform));
        }
    }

    private void HideCar(int index)
    {
        cars[index].gameObject.SetActive(false);
    }

    private void ShowCar(int index)
    {
        cars[index].gameObject.SetActive(true);
    }

    private void HideAllCars()
    {
        foreach (GameObject car in cars)
        {
            car.SetActive(false);
        }
    }

    private void UpdateCarSelection(int index)
    {
        ShowCar(index);

        carTitle.text = carInformationSOArray[index].carName;
    }
}
