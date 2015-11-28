using UnityEngine;
using System.Collections;

public class NodeImpl : NodeBase
{
    protected int dragableRectHeight = 50;

    public override void Draw()
    {
        GUILayout.BeginArea(new Rect(0, 0, DragableRect.width, DragableRect.height), GUI.skin.box);
        GUILayout.Label("\n" + Id);
        GUILayout.EndArea();
    }

    public override Rect DragableRect
    {
        get
        {
            return new Rect(Position.x, Position.y, Size.x, dragableRectHeight);
        }
    }

}
