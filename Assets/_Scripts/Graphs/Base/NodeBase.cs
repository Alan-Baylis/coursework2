using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class NodeBase: NodeDrawableBase
{
	/// <summary>
	/// Returns all instances of NodeBase ever created and not deleted.
	/// </summary>
	public static List<NodeBase> AllNodes {
		get {
			var result = allDrawableBases.OfType<NodeBase> ();
			return new List<NodeBase> (result);
		}
	}
	
    /// <summary>
    /// First joint.
    /// </summary>
    public NodeJointPoint FirstJoint
    {
        get
        {
            //Debug.Log("Joints count in " + this + " = " + joints.Count);
            return joints[0];
        }
        protected set
        {
            joints[0] = value;
        }
    }

    public NodeJointPoint this[int i]
    {
        get { return joints[i]; }
        protected set { joints[i] = value; }
    }

	#region Constructors and destructors

	protected NodeBase ()
	{
		Children = new List<NodeChildBase> ();
	}

	~NodeBase ()
	{
		AllNodes.Remove (this);
	}

	#endregion
	#region Serialization

	[To2dnd.TBeforeSerialization]
	public override void Save ()
	{
		base.Save ();
	}

	[To2dnd.TAfterDeserialization]
	public override void Load ()
	{
		base.Load ();
		foreach (var child in Children) {
			child.Parent = this;
		}
	}

	#endregion
    #region Deprecated
    /// <summary>
    /// Determines, whether the button has been pressed on the particular child
    /// and sets the CurrentHitChild variable to child that is pressed.
    /// </summary>
    /// <param name="position">position to check</param>
    /// <returns>whether at least some child is pressed</returns>
    public virtual bool PositionMatchesNodeChild(Vector2 position)
    {
        var result = false;
        CurrentHitChild = null;
        CurrentHitJointIndex = -1;

        foreach (var child in Children)
        {
            for (var i = 0; i < child.joints.Count; ++i)
            {
                var actionRect = child.joints[i].ActionRect;
                actionRect.x += Position.x + child.Position.x;
                actionRect.y += Position.y + child.Position.y + Size.y;
                if (!actionRect.Contains(position))
                    continue;
                result = true;
                BezierStartPosition = actionRect.center;
                CurrentHitChild = child;
                CurrentHitJointIndex = i;
                break;
            }
        }

        return result;
    }
    // TODO remove (no sense)
    public NodeJointPoint CurrentJointPoint { get { return CurrentHitJointIndex == -1 ? null : CurrentHitChild.joints[CurrentHitJointIndex]; } }
    public virtual Rect GetOutsRect ()
	{
		var result = new Rect ();
		foreach (var child in Children) {
			if (child.Position.x < result.x) {
				result.x = child.Position.x;
			}
			if (child.Position.y < result.y) {
				result.y = child.Position.y;
			}
			if (child.Size.x > result.width) {
				result.width = child.Size.x;
			}
			result.height += child.Size.y;
		}

		return result;
	}
	public virtual void DrawConnections ()
	{
		foreach (var child in Children) {
			//Debug.Log(string.Format("Length of {0}'s joints:{1}", child.Id, child.joints.Count));
			foreach (var point in child.joints) {
				if (point.TargetId == NothingId)
					continue;
				var startPoint = Position + child.Position + point.Offset + new Vector2 (0, Size.y);
				var destinationNode = point.Target.Parent;
				if (destinationNode == null)
					continue;
				var endPoint = point.Target.Parent.Position + destinationNode.joints [0].Offset;
				CustomEditorHelper.DrawConnection (startPoint, endPoint);
			}
		}
	}
    /// <summary>
    /// The full boundary box in which the whole object is aligned.
    /// </summary>
    public virtual Rect FullRect
    {
        get
        {
            var contentRect = GetOutsRect();
            var mainRect = new Rect(Position, Size);
            mainRect.width = Mathf.Max(contentRect.width, mainRect.width);
            mainRect.height = Mathf.Max(contentRect.height, mainRect.height);
            return mainRect;
        }
    }
    /// <summary>
    /// The rectangle which is responsible for handling dragging.
    /// </summary>
    public abstract Rect DragableRect { get; }
    /// <summary>
    /// Children that each node has.
    /// </summary>
    [To2dnd.TDataMember]
    public List<NodeChildBase> Children { get; protected set; }
    /// <summary>
    /// Gets or sets the current hit child.
    /// </summary>
    public virtual NodeChildBase CurrentHitChild { get; protected set; }
    /// <summary>
    /// TODO eject the field from class (no sense of it to be in class)
    /// </summary>
    public virtual int CurrentHitJointIndex { get; protected set; }
    /// <summary>
    /// Point from which you start to draw curve which identifies connections.
    /// </summary>
    public Vector2 BezierStartPosition { get; protected set; }
    /// <summary>
    /// The the first child's action rect.
    /// </summary>
    public Rect FirstJointRect { get { return joints[0].ActionRect; } set { joints[0].ActionRect = value; } }
    /// <summary>
    /// The offset of first child's joint.
    /// </summary>
    public Vector2 FirstJointOffset { get { return joints[0].Offset; } set { joints[0].Offset = value; } }
    #region Children CRUD

    public virtual void AddChild(NodeChildBase node)
    {
        if (node == null)
            return;
        node.Parent = this;
        Children.Add(node);
    }

    public virtual void RemoveChild(NodeChildBase node)
    {
        if (Children.Remove(node))
        {
            node.Parent = null;
        }
    }

    #endregion
    #endregion
	public override string ToString ()
	{
		return string.Format ("ID: {0}, Position:{1}, Size:{2}, FirstJointRect: {3}, FirstJointOffset: {4}.",
		                     Id, Position, Size, FirstJointRect, FirstJointOffset);
	}
	public virtual void Connect<T> (T nb) where T: NodeBase
	{
        if (nb != null)
        {
            var baseInstance = (NodeBase)nb;
            var newNodeJointPoint = new NodeJointPoint(this);
            newNodeJointPoint.Connect(nb);
            baseInstance.joints.Add(newNodeJointPoint);
        }
        else
        {
            Debug.LogError("error trying to connect NodeBase to null");
        }
	}
}
