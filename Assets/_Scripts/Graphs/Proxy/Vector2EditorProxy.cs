using UnityEngine;

[System.Serializable]
public class Vector2EditorProxy
{
    [To2dnd.TDataMember]
    public float X { get; set; }
    [To2dnd.TDataMember]
    public float Y { get; set; }

    protected Vector2EditorProxy() { }

    public Vector2EditorProxy(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Vector2EditorProxy(Vector2 vector2) : this(vector2.x, vector2.y)
    {
    }

    public Vector3 ToVector2()
    {
        return new Vector2(X, Y);
    }
}