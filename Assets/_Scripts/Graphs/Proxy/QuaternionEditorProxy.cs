using UnityEngine;

public class QuaternionEditorProxy
{
    [To2dnd.TDataMember]
    public float X { get; set; }
    [To2dnd.TDataMember]
    public float Y { get; set; }
    [To2dnd.TDataMember]
    public float Z { get; set; }
    [To2dnd.TDataMember]
    public float W { get; set; }

    protected QuaternionEditorProxy() { }

    public QuaternionEditorProxy(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public QuaternionEditorProxy(Quaternion quaternion)
        : this(quaternion.x, quaternion.y, quaternion.z, quaternion.w)
    {
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(X, Y, Z, W);
    }
}