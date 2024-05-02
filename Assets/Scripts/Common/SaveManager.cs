using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance {  get; private set; }
    public Skidmarks Skidmarks { get; set; }
    public PositionCalculator PositionCalculator { get; set; }

    public int CurrCar;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/CarData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/CarData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);

            CurrCar = data.CurrCar;
            file.Close();
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/CarData.dat");
        PlayerData data = new PlayerData();

        data.CurrCar = CurrCar;
        bf.Serialize(file, data);
        file.Close();
    }
}
[Serializable]
 class PlayerData
{
    public int CurrCar;
}