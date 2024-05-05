[System.Serializable]
public struct SerializableVector3
{
    public float x; 
    public float y; 
    public float z;


    public SerializableVector3(UnityEngine.Vector3 vector)
    {
        x = vector.x; 
        y = vector.y; 
        z = vector.z;
    }


    public UnityEngine.Vector3 ToVector3()
    {
        return new UnityEngine.Vector3(x, y, z);
    }
}