using UnityEngine;


[System.Serializable]
public class WayPoint : MonoBehaviour
{
    public WayPoint NextPoint;
    public WayPoint[] AlternativePoint;
    public float DistanceToGetNext = 25;

    public WayPoint GetRandomAlternativePoint()
    {
        if (AlternativePoint == null || AlternativePoint.Length == 0)
            return NextPoint;

        int index = Random.Range(0, AlternativePoint.Length - 1);
        return AlternativePoint[index];
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    other.TryGetComponent(out BotsCarMovement otherCarMovement);
    //    otherCarMovement.MoveToNextPoint();
    //}


    // testing visualization
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        
        Gizmos.DrawWireSphere(transform.position, 10f);

    }
}