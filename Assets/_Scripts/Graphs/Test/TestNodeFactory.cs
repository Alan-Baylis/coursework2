using UnityEngine;
using System.Collections;

public class TestAbstractNodeFactory : AbstractNodeFactory<NodeImpl, NodeChildImpl>
{
    protected readonly Rect NodeActionRect = new Rect(5, 5, 10, 10);
    public override NodeImpl CreateNewNode(float x, float y)
    {
        NodeImpl result = new NodeImpl();
        result.Position = new Vector2(x, y);
        result.Size = new Vector2(150, 200);
        result.joints.Add(new NodeJointPoint(NodeActionRect, NodeActionRect.center, result));
        return result;
    }

    public override NodeChildImpl CreateNewChild(NodeImpl parent)
    {
        NodeChildImpl result = new NodeChildImpl();
        result.Position = new Vector2(0, 100 * parent.Children.Count);
        result.Size = new Vector2(150, 100);
        result.joints.Add(CreateJointPoint(new Rect(125, 5, 25, 25), new Rect(125, 5, 25, 25).center, parent));
        return result;
    }
}
