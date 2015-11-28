using UnityEngine;
using System.Collections;

public class RectEditorProxy
{

    [To2dnd.TDataMember]
    public float X { get; set; }
    [To2dnd.TDataMember]
    public float Y { get; set; }
    [To2dnd.TDataMember]
    public float Width { get; set; }
    [To2dnd.TDataMember]
    public float Height { get; set; }

    protected RectEditorProxy() { }

    public RectEditorProxy(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public RectEditorProxy(Rect rect)
        : this(rect.x, rect.y, rect.width, rect.height)
    {
    }

    public Rect ToRect()
    {
        return new Rect(X, Y, Width, Height);
    }
}
