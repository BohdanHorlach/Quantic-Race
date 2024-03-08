using UnityEngine;


public class WheelSkidInitializer : MonoBehaviour
{
    [SerializeField] private WheelSkid[] _wheelSkids;


    public void Initialize(Skidmarks skidmarks)
    {
        foreach(WheelSkid wheelSkid in _wheelSkids)
        {
            wheelSkid.Initialize(skidmarks);
        }
    }
}