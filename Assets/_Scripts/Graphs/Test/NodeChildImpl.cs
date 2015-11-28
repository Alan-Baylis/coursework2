using UnityEngine;
using System.Collections;

public class NodeChildImpl : NodeChildBase
{
    public override void Draw()
    {
        GUILayout.Label(Id);
    }

    public Rect ActionRect
    {
        get
        {
            return joints[0].ActionRect;
        }
        set
        {
            joints[0].ActionRect = value;
        }
    }

    public Vector2 Offset
    {
        get
        {
            return joints[0].Offset;
        }
        set
        {
            joints[0].Offset = value;
        }
    }
}
