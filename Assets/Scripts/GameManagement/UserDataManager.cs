using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// TODO
//
// rename later:
//                  PlayGame -> PlaySinglePlayer
//                  




//
// menu -> Single player TrackSelect -> 
//      -> Multiplayer TrackSelect -> 
//

public static class UserDataManager
{

    static public SelectedGameOptionsSO selectedGameOptionsSO;

    public static void SelectCarInfoSO(int index)
    {
        selectedGameOptionsSO.selectedCarInformationSO = selectedGameOptionsSO.carInformationSOArray[index];
    }

    public static int GetIndexOfSelectedCar()
    {
        return Array.IndexOf(selectedGameOptionsSO.carInformationSOArray, selectedGameOptionsSO.selectedCarInformationSO);
    }

}
