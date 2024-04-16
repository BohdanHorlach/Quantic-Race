using UnityEngine;
using UnityEngine.UI;

public class CarSelector : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    private int CurrCar;

    private void Awake()
    {
        CurrCar = SaveManager.instance.CurrCar;
        Selectcar(CurrCar);       
    }

    private void Selectcar(int temp)
    {
        prevButton.interactable = (temp !=0);
        nextButton.interactable= (temp != transform.childCount - 1);
        for (int i = 0; i<transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == temp);
        }
    }

    public void ChangCar(int temp) 
    {
        CurrCar += temp;
        SaveManager.instance.CurrCar = CurrCar;
        SaveManager.instance.Save();
        Selectcar(CurrCar);
    }
}
