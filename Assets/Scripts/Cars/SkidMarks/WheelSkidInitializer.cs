using UnityEngine;


public class WheelSkidInitializer : MonoBehaviour
{
    [SerializeField] private WheelSkid[] _wheelSkids;


    private void Awake()
    {
        if(SaveManager.instance.Skidmarks == null)
            SaveManager.instance.Skidmarks = FindObjectOfType<Skidmarks>();

        Initialize(SaveManager.instance.Skidmarks);
    }


    private void Initialize(Skidmarks skidmarks)
    {
        foreach(WheelSkid wheelSkid in _wheelSkids)
        {
            wheelSkid.Initialize(skidmarks);
        }
    }
}