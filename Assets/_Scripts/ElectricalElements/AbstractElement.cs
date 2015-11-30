using System.Collections.Generic;
using System.Linq;

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
            FirstJoint.Connect(value);
		}
	}
    #region Constructors & destructors
    protected AbstractElement (ElectricProperties props)
	{
		if (props == null)
			props = ElectricProperties.CreateFromUR (0, 1);
		Properties = props;
		joints.Add (new NodeJointPoint (this));
//		FirstJoint = new NodeJointPoint (this);
	}

    protected AbstractElement () : this(null)
	{
	}
    #endregion
    public bool Powered { get { return Properties.IsConsideredPowered (); } }

    public virtual bool Conductive { get { return NextElements.Count != 0; } }

    public List<AbstractElement> NextElements
    {
        get
        {
            return (from point in joints where point.Target != null && point.Target.Parent != null select point.Target.Parent as AbstractElement).ToList();
        }
    }

    public virtual void GiveProperties()
    {
        var listOfConnectedElements = NextElement == null ? null : NextElement.GetListOfConnectedElements(this);
        if (listOfConnectedElements == null)
        {
            //Debug.Log("No connected elements.");
            return;
        }
        //Debug.Log(string.Format("Connected elements: {0}", listOfConnectedElements.GetReadableList()));
        var batteries = listOfConnectedElements.OfType<Battery>();
        var batteryU = 0.0;
        var batteryR = 0.0;

        foreach (var battery in batteries)
        {
            batteryR += battery.Properties.Resistance;
            batteryU += battery.Properties.Current;
        }

        var circuitR = listOfConnectedElements.Where(connectedElement => !(connectedElement is Battery)).Sum(connectedElement => connectedElement.Properties.Resistance);

        foreach (var connectedElement in listOfConnectedElements)
        {
            connectedElement.Properties.SetIR(batteryU/(batteryR + circuitR), connectedElement.Properties.Resistance);
        }
    }

    public virtual List<AbstractElement> GetListOfConnectedElements(AbstractElement begin, List<AbstractElement> resList = null)
    {
        if (resList == null) // if list is no assigned, assign it
        {
            //Debug.Log("Assigning resList");
            resList = new List<AbstractElement>();
        }
        //Debug.Log(string.Format("Adding our element, {0}", Name));
        resList.Add(this);
        if (this == begin) // if we reached the element from which we started, the list is complete
        {
            //Debug.Log(string.Format("END, returning resList with length {0} and elements {1}", resList.Count, resList.GetReadableList()));
            return resList;
        }
        if (!Conductive)
        {
            // [reslist assigned, this is not beginning element, have no joints]
            //Debug.Log(string.Format("!!! Returning null, no joints in {0} object", Name));
            resList.Remove(this);
            return null;
        }
        // [reslist assigned, this is not beginning element, have joints] add this to reslist
        
        var allPaths = NextElements.Select(element => element.GetListOfConnectedElements(begin, resList)).ToList();
        var totalResList = new List<AbstractElement>();
        foreach (var element in allPaths.Where(path => path != null).SelectMany(path => path.Where(element => !totalResList.Contains(element))))
        {
            totalResList.Add(element);
        }
        return totalResList;
    }

    /*public virtual void GiveProperties(AbstractElement beginning = null)
    {
        if (this is Battery)
        {
            //Debug.Log("This is battery " + Id);
            beginning = this;
            if (NextElement != null)
            {
                NextElement.GiveProperties(Properties, beginning);
            }
        }
        else if (NextElement != null)
        {
            NextElement.GiveProperties(beginning);
        }
    }

    public virtual void GiveProperties(ElectricProperties properties, AbstractElement beginning)
	{
        if (beginning == this)
            return;
        Properties = properties;
        NextElement.GiveProperties(properties, beginning);
	}*/

	/// <summary>
	///     Name of the element.
	/// </summary>
	public virtual string Name {
        get { return string.Format("{0}({1})", GetType(), Id); } // string.Format("{0}({1})", GetType().ToString(), Id)
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
        if (NextElement == null)
        {
            NextElement = other;
        }
        else
        {
            joints.Add(new NodeJointPoint(this, other));
        }
    }

    public void Disconnect(AbstractElement other)
    {
        joints.RemoveAll(x => x.Connected(other));
    }

    protected double GetFullCircuitResistance(AbstractElement beginning, double sum=0)
    {
        if (beginning == this) return 0;
        if (NextElement != null)
        {
            return NextElement.GetFullCircuitResistance(beginning, sum + Properties.Resistance);
        }
        return sum + Properties.Resistance;
    }

    public virtual bool IsClosed()
    {
        return _IsClosed();
    }

    protected virtual bool _IsClosed(AbstractElement beginningElement=null)
    {
        var goToNext = NextElement != null;
        var reachedClosure = beginningElement == this;

        if (reachedClosure)
        {
            return true;
        }
        return goToNext && NextElement._IsClosed(beginningElement ?? this);
    }

    public override string ToString()
    {
        return string.Format("{0}({1})", GetType(), Id);
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