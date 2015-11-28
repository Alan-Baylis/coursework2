using UnityEngine;

[System.Serializable]
public class Vector3EditorProxy
{
    [To2dnd.TDataMember]
    public float X { get; set; }
    [To2dnd.TDataMember]
    public float Y { get; set; }
    [To2dnd.TDataMember]
    public float Z { get; set; }

    protected Vector3EditorProxy() { }

    public Vector3EditorProxy(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3EditorProxy(Vector3 vector3) : this(vector3.x, vector3.y, vector3.z)
    {
    }

    public Vector3 ToVector3()
    {
        return new Vector3(X, Y, Z);
    }
}