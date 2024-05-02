using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitManager : MonoBehaviour
{

    [SerializeField] private SelectedGameOptionsSO selectedGameOptionsSO;
    [SerializeField] private Canvas canvas;
    private void Awake()
    {
        UserDataManager.selectedGameOptionsSO = selectedGameOptionsSO;
        canvas.enabled = true;

    }

}
