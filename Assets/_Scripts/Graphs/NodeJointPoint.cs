using System;
using System.Collections.Generic;
using System.Linq;
using To2dnd;
using UnityEngine;

[Serializable]
public class NodeJointPoint
{
	/// <summary>
	/// All instances of class.
	/// </summary>
	public static List<NodeJointPoint> allPoints = new List<NodeJointPoint> ();
	/// <summary>
	/// The id of Node holding this point.
	/// </summary>
	[TDataMember]
	public string ParentId { get; set; }
	/// <summary>
	/// The Node holding this point.
	/// The result is determined by Id.
	/// </summary>
	public NodeDrawableBase Parent {
		get {
			return NodeDrawableBase.allDrawableBases.FirstOrDefault (node => node.Id == ParentId);
		}
	}

	/// <summary>
	/// The point to which this joint is connected.
	/// The result is not determined by Id.
	/// </summary>
	public NodeJointPoint Target { get; protected set; }

	/// <summary>
	/// The target Node id.
	/// </summary>
	/// <value>The target identifier.</value>
	[TDataMember]
	public string TargetId { get; set; }
    #region Deprecated
    /// <summary>
    /// TODO ?
    /// </summary>
    public Rect ActionRect { get; set; }
    /// <summary>
    /// TODO ?
    /// </summary>
    public Vector2 Offset { get; set; }
    #endregion
    #region Serialization
    [TDataMember]
	public RectEditorProxy
		actionRectProxy;
	[TDataMember]
	public Vector2EditorProxy
		offsetProxy;

	[TBeforeSerialization]
	public void Save ()
	{
		offsetProxy = new Vector2EditorProxy (Offset);
		actionRectProxy = new RectEditorProxy (ActionRect);
	}

	[TAfterDeserialization]
	public void Load ()
	{
		Offset = offsetProxy.ToVector2 ();
		ActionRect = actionRectProxy.ToRect ();
		if (TargetId != NodeDrawableBase.NothingId) {
			var nodeBase = NodeBase.AllNodes.FirstOrDefault (node => node.Id == TargetId);
			if (nodeBase != null)
				Target = nodeBase.FirstJoint;
			else
				Debug.LogError (string.Format ("Load failure! TargetId({0}) referencing not existing node!", TargetId));
		} else {
			Target = null;
		}
	}
	#endregion
    #region Constructors & destructors
    public NodeJointPoint (Rect actionRect, Vector2 offset, NodeDrawableBase parent, NodeBase target=null)
	{
		ActionRect = actionRect;
		Offset = offset;
		Target = target != null ? target.FirstJoint : null;
		TargetId = target != null ? target.Id : NodeDrawableBase.NothingId;
		ParentId = parent.Id;
	}

	public NodeJointPoint (NodeDrawableBase parent, NodeBase target = null)
        : this(new Rect(), new Vector2(), parent, target)
	{
	}

    protected NodeJointPoint()
    {
    }

    ~NodeJointPoint()
    {
        allPoints.Remove(this);
    }
    #endregion
    /// <summary>
	/// Connect point to given Node.
	/// </summary>
	public bool Connect (NodeBase target)
	{
		/*if (target == null) {
			TargetId = NodeDrawableBase.NothingId;
			Target = null;
            return false;
		}
        TargetId = target.Id;
        Target = target.FirstJoint;
        return true;*/

        TargetId = target == null ? NodeDrawableBase.NothingId : target.Id;
        Target = target == null ? null : target.FirstJoint;
        return target == null;
	}

	/// <summary>
	/// Is the point connected to given Node?
	/// </summary>
	public bool Connected (NodeBase target)
	{
		return TargetId == target.Id;
	}
}
