using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewSelectedGameOptionsSO", menuName = "ScriptableObjects/SelectedGameOptionsSO")]
public class SelectedGameOptionsSO : ScriptableObject
{
    public CarInformationSO selectedCarInformationSO;
    public CarInformationSO[] carInformationSOArray;
    public int selectedTrackIndex;

    // true -> singlePlayer
    // false -> multiPlayer
    public bool isSinglePlayer;

    public Transform[] spawnPoints;
}
