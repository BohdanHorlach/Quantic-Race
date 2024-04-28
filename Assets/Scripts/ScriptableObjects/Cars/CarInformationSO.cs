using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCarInformatinSO", menuName = "ScriptableObjects/CarInformationSO")]
public class CarInformationSO : ScriptableObject
{
    public string carName;
    public GameObject singlePlayerUserPrefab;
    public GameObject singlePlayerBotPrefab;
    public GameObject visualPrefab;

    // true -> singlePlayer
    // false -> multiPlayer
    public bool isSinglePlayer;
}
