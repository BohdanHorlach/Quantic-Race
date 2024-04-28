using UnityEngine;
using UnityEngine.UI;

public class CarSelector : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    private int CurrentCarIndex;

    private void Awake()
    {
        CurrentCarIndex = SaveManager.instance.CurrCar;
        Selectcar(CurrentCarIndex);
    }

    private void Selectcar(int temp)
    {
        prevButton.interactable = (temp != 0);
        nextButton.interactable = (temp != transform.childCount - 1);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == temp);
        }
    }

    public void ChangCar(int temp)
    {
        CurrentCarIndex += temp;
        SaveManager.instance.CurrCar = CurrentCarIndex;
        SaveManager.instance.Save();
        Selectcar(CurrentCarIndex);
    }
}
