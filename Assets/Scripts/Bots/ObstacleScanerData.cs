[System.Serializable]
public struct ObstacleScanerData
{
    public static float emptyValue = -1f;

    public float forwardHitDistance;
    public float leftHitDistance;
    public float rightHitDistance;


    public static ObstacleScanerData GetEmpty()
    {
        ObstacleScanerData data;
        data.forwardHitDistance = emptyValue;
        data.leftHitDistance = emptyValue;
        data.rightHitDistance = emptyValue;

        return data;
    }


    public bool IsEmpty()
    {
        return forwardHitDistance == emptyValue && leftHitDistance == emptyValue && rightHitDistance == emptyValue;
    }
}