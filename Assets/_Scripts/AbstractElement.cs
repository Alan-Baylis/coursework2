using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class AbstractElement : NodeBase, IConnectable<AbstractElement>
{
	public const string DefaultNameOfElement = "Element";

	public virtual AbstractElement NextElement {
		get {
            if (FirstJoint.Target == null || FirstJoint.Target.Parent == null)
                return null;
			return FirstJoint.Target.Parent as AbstractElement;
		}
		protected set {
            FirstJoint.Connect(value);// ? new NodeJointPoint (value)
		}
	}
    #region Constructors & destructors
    protected AbstractElement (ElectricProperties props)
	{
		if (props == null)
			props = ElectricProperties.CreateFromUR (0, 1);
		Properties = props;
		//joints.Add (new NodeJointPoint (this));
		FirstJoint = new NodeJointPoint (this);
	}

    protected AbstractElement () : this(null)
	{
	}
    #endregion
    public bool Powered { get { return Properties.IsConsideredPowered (); } }

    public virtual void GiveProperties(AbstractElement beginning = null, Battery battery = null)
    {
        if (this is Battery && battery == null)
        {
            battery = (beginning = this) as Battery;
        }
        if (battery != null)
        {
            GiveProperties(battery.Properties, beginning);
        }
        else
        {
            if(NextElement != null)
                NextElement.GiveProperties(beginning, battery);
        }
    }

    protected virtual void GiveProperties(ElectricProperties properties, AbstractElement beginning=null)
	{
        if (beginning == null)
            beginning = this;
        else if (beginning == this)
            return;
        Properties = properties;
        NextElement.GiveProperties(properties, beginning);
	}

	/// <summary>
	///     Name of the element.
	/// </summary>
	public virtual string Name {
		get { return DefaultNameOfElement; }
	}

	#region Electric properties

	/// <summary>
	///     The electric properties of element.
	/// </summary>
	public ElectricProperties Properties { get; set; }

	public virtual double GetAmperage ()
	{
		return Properties.Amperage;
	}

	public virtual double GetCurrent ()
	{
		return Properties.Current;
	}

	public virtual double GetResistance ()
	{
		return Properties.Resistance;
	}

	#endregion

    public void Connect(AbstractElement other)
    {
        NextElement = other;
    }
}

/*private static string IdRefinition (string id)
{
	if (!Ids.Contains (id))
		return id;
	var r = new Regex (@"\(\d+\)");
	var m = r.Match (id);
	if (m.Success) {
		var oldValue = m.Groups [m.Groups.Count - 1].ToString ();
		var numberInParenthesis = Convert.ToInt32 (oldValue.Substring (1, oldValue.Length - 1));
		id = id.Replace (oldValue, string.Format ("({0})", numberInParenthesis + 1));
	} else {
		id += " (1)";
	}

	return id;
}*/