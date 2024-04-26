using UnityEngine;


[System.Serializable]
public class WayPoint : MonoBehaviour
{
    public WayPoint NextPoint;
    public WayPoint[] AlternativePoint;
    public float DistanceToGetNext;


    public WayPoint GetRandomAlternativePoint()
    {
        if (AlternativePoint == null || AlternativePoint.Length == 0)
            return NextPoint;

        int index = Random.Range(0, AlternativePoint.Length - 1);
        return AlternativePoint[index];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        
        Gizmos.DrawWireSphere(transform.position, 10f);

    }
}