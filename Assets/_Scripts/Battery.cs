public class Battery : AbstractElement
{
	#region implemented abstract members of NodeDrawableBase
	public override void Draw ()
	{
		throw new System.NotImplementedException ();
	}
	#endregion
	#region implemented abstract members of NodeBase
	public override UnityEngine.Rect DragableRect {
		get {
			throw new System.NotImplementedException ();
		}
	}
	#endregion
}