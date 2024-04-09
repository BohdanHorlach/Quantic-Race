using UnityEngine;


[System.Serializable]
public class WayPont : MonoBehaviour
{
    public WayPont NextPoint;
    public WayPont[] AlternativePoint;
    public float DistanceToGetNext;


    public WayPont GetRandomAlternativePoint()
    {
        if (AlternativePoint == null || AlternativePoint.Length == 0)
            return NextPoint;

        int index = Random.Range(0, AlternativePoint.Length - 1);
        return AlternativePoint[index];
    }
}