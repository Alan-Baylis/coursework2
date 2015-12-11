using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class NodeDrawableBase
{
	/// <summary>
	/// All drawables.
	/// </summary>
	public static List<NodeDrawableBase> allDrawableBases = new List<NodeDrawableBase> ();
	/// <summary>
	/// The unique identifier of drawable.
	/// </summary>
	[To2dnd.TDataMember]
	public virtual string Id { get; set; }
    #region Deprecated
    /// <summary>
    /// The position of drawable.
    /// </summary>
    public Vector2 Position { get; set; }
    /// <summary>
    /// The size of element.
    /// </summary>
    public Vector2 Size { get; set; }
    #endregion
    /// <summary>
	/// ElementName used to represent that element is not
	/// connected to anything.
	/// </summary>
	public const string NothingId = "-1";
	/// <summary>
	/// The Ids have incremental growth.
	/// This one tells from what.
	/// </summary>
	public const string StartingId = "0";
	/// <summary>
	/// Children need to have ids in form:
	/// &#60;node ElementName&#62;&#60;delimiter&#62;&#60;child ElementName&#62;.
	/// This is the delimiter.
	/// </summary>
	public const string HierarchyDelimiter = "->";
	/// <summary>
	/// The list of connective joints.
	/// </summary>
	[To2dnd.TDataMember]
	public List<NodeJointPoint> joints = new List<NodeJointPoint> ();
	#region Serialization

	[To2dnd.TDataMember]
	protected Vector2EditorProxy
		positionProxy;
	[To2dnd.TDataMember]
	protected Vector2EditorProxy
		sizeProxy;

	public abstract void Draw ();

	[To2dnd.TBeforeSerialization]
	public virtual void Save ()
	{
		sizeProxy = new Vector2EditorProxy (Size);
		positionProxy = new Vector2EditorProxy (Position);
	}

	[To2dnd.TAfterDeserialization]
	public virtual void Load ()
	{
		Size = sizeProxy.ToVector2 ();
		Position = positionProxy.ToVector2 ();
	}

	#endregion
	#region Constructors and destructors
	protected NodeDrawableBase ()
	{
		allDrawableBases.Add (this);
		Id = (allDrawableBases.Count).ToString ();
	}

	~NodeDrawableBase ()
	{
		allDrawableBases.Remove (this);
	}
	#endregion
	/// <summary>
	/// Query to all objects to get objects of some specified type.
	/// </summary>
	public List<T> GetSpecificDrawableList<T> () where T: NodeDrawableBase
	{
		var result = allDrawableBases.OfType<T> ().ToList ();
		return result;
	}
}
