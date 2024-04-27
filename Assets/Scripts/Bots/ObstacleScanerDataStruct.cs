using UnityEngine;

[System.Serializable]
public struct ObstacleScanerDataStruct
{
    public static float emptyValue = -1f;
    public static float angleEmptyValue = -1000f;

    public float forwardHitDistance;
    public float leftHitDistance;
    public float rightHitDistance;
    public float forwardHitAngle;


    public static ObstacleScanerDataStruct GetEmpty()
    {
        ObstacleScanerDataStruct data;
        data.forwardHitDistance = emptyValue;
        data.leftHitDistance = emptyValue;
        data.rightHitDistance = emptyValue;
        data.forwardHitAngle = angleEmptyValue;

        return data;
    }


    public bool IsEmpty()
    {
        return forwardHitDistance == emptyValue && leftHitDistance == emptyValue && rightHitDistance == emptyValue;
    }

    // TODO TESTING
    public void CanMoveForward()
    {

        Debug.Log("Angle between ray and hit surface: " + forwardHitAngle);
        Debug.Log("----------------");

    }
}