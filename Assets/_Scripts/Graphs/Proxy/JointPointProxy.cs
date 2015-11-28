using UnityEngine;
using System.Collections;

[System.Serializable]
public class JointPointProxy {
    [To2dnd.TDataMember]
    public RectEditorProxy actionRectProxy;
    [To2dnd.TDataMember]
    public Vector2EditorProxy offsetProxy;
    [To2dnd.TDataMember] 
    public string targetId;

    protected JointPointProxy() { }

    public JointPointProxy(RectEditorProxy actionRectProxy, Vector2EditorProxy offsetProxy, string targetId)
    {
        this.actionRectProxy = actionRectProxy;
        this.offsetProxy = offsetProxy;
        this.targetId = targetId;
    }

    public JointPointProxy(Rect rect, Vector2 offset, string targetId)
        : this(new RectEditorProxy(rect), new Vector2EditorProxy(offset), targetId)
    {
    }

    public NodeJointPoint ToJointPoint(NodeDrawableBase parent)
    {
        return new NodeJointPoint(actionRectProxy.ToRect(), offsetProxy.ToVector2(), parent);
    }
}
