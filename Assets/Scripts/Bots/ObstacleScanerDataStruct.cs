using UnityEngine;

[System.Serializable]
public struct ObstacleScanerDataStruct
{
    public static float emptyValue = -1f;
    public static float angleEmptyValue = -1000f;
    // comparing error can be allowed when comparing world distance
    public static float ALLOWED_DISTANCE_ERROR = 3;

    public float forwardHitDistance;
    public float leftHitDistance;
    public float rightHitDistance;
    public float forwardHitAngle;

    public float leftHitInfiniteDistance;
    public float rightHitInfiniteDistance;

    //public bool isForwardWall;


    //public static ObstacleScanerDataStruct GetEmpty()
    //{
    //    ObstacleScanerDataStruct data;
    //    data.forwardHitDistance = emptyValue;
    //    data.leftHitDistance = emptyValue;
    //    data.rightHitDistance = emptyValue;
    //    data.forwardHitAngle = angleEmptyValue;

    //    return data;
    //}


    public bool IsEmpty()
    {
        return forwardHitDistance == emptyValue && leftHitDistance == emptyValue && rightHitDistance == emptyValue;
    }


    public int GetSideWithMoreSpace()
    {
        int left = -1;
        int right = 1;

        if (rightHitDistance == emptyValue && leftHitDistance == emptyValue)
        {
            // no obstacles on the left and right
            if (leftHitInfiniteDistance > rightHitInfiniteDistance)
            {
                return left;
            }
            else
            {
                return right;
            }
        }
        else
        {
            return rightHitDistance == emptyValue ? right : left;
        }
    }
    //public bool IsEmptySides()
    //{
    //    return leftHitDistance == emptyValue && rightHitDistance == emptyValue;
    //}
    // TODO TESTING
    public void CanMoveForward()
    {

        Debug.Log("Angle between ray and hit surface: " + forwardHitAngle);
        Debug.Log("----------------");

    }
}