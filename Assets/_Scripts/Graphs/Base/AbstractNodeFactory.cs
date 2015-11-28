using UnityEngine;
using System.Collections;

public abstract class AbstractNodeFactory<TNode, TChild> where TNode : NodeBase where TChild : NodeChildBase
{
    public abstract TNode CreateNewNode(float x, float y);
    #region Deprecated
    public abstract TChild CreateNewChild(TNode parent);
    #endregion
    public virtual NodeJointPoint CreateJointPoint(NodeDrawableBase parent, NodeBase target=null)
    {
        return parent != null ? new NodeJointPoint(parent, target) : null;
    }

    public virtual NodeJointPoint CreateJointPoint(Rect actionRect, Vector2 offset, NodeDrawableBase parent, NodeBase target = null)
    {
        return parent != null ? new NodeJointPoint(actionRect, offset, parent, target) : null;
    }
}
