public class Battery : AbstractElement
{
    public Battery(ElectricProperties props) : base(props)
    {
    }

    #region implemented abstract members of NodeDrawableBase

    public Battery() : base (null)
    {
    }

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