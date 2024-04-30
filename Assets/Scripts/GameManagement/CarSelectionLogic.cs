using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectionLogic : MonoBehaviour
{
    //[SerializeField] private SelectedGameOptionsSO selectedGameOptionsSO;
    [SerializeField] private TextMeshProUGUI carTitle;
    private List<GameObject> cars;

    private int selectedCarIndex; // Keeps track of the currently selected car

    // Start is called before the first frame update
    void Start()
    {
        // If no car prefabs are assigned, handle the error
        if (UserDataManager.selectedGameOptionsSO.carInformationSOArray.Length == 0)
        {
            Debug.LogError("No car prefabs assigned!");
            return;
        }

        InstantiateAllCars();
        HideAllCars();

        // Set initial car model and name
        TryLoadSelectedCar();
    }

    public void OnNextButtonPressed()
    {
        HideCar(selectedCarIndex);

        selectedCarIndex++;
        if (selectedCarIndex >= UserDataManager.selectedGameOptionsSO.carInformationSOArray.Length)
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
            selectedCarIndex = UserDataManager.selectedGameOptionsSO.carInformationSOArray.Length - 1; // Wrap around if reaching the beginning
        }
        UpdateCarSelection(selectedCarIndex);
    }

    private void InstantiateAllCars()
    {
        cars = new List<GameObject>();
        foreach (CarInformationSO carInformationSO in UserDataManager.selectedGameOptionsSO.carInformationSOArray)
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
        UserDataManager.SelectCarInfoSO(index);

        carTitle.text = UserDataManager.selectedGameOptionsSO.carInformationSOArray[index].carName;
    }

    private void TryLoadSelectedCar()
    {
        if (UserDataManager.selectedGameOptionsSO.selectedCarInformationSO == null)
        {
            selectedCarIndex = 0;
            UpdateCarSelection(selectedCarIndex);
        }
        else
        {
            selectedCarIndex = UserDataManager.GetIndexOfSelectedCar();
            UpdateCarSelection(selectedCarIndex);
        }
    }
}
