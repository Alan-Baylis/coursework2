using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class AbstractElement : NodeBase, IConnectable<AbstractElement>
{
    public const string DefaultNameOfElement = "Element";

    public virtual AbstractElement NextElement
    {
        get
        {
            if (FirstJoint.Target == null)
            {
                //Debug.Log(string.Format("{0} has target as null", this));
                return null;
            }
            if (FirstJoint.Target.Parent != null) return (AbstractElement) (FirstJoint.Target.Parent);
            //Debug.Log(string.Format("{0} has target.parent as null", this));
            return null;
        }
        protected set { FirstJoint.Connect(value); }
    }

    public bool Powered
    {
        get { return Properties.IsPowered(); }
    }
    
    public virtual bool Conductive
    {
        get { return NextElements.Count != 0; }
    }

    public List<AbstractElement> NextElements
    {
        get
        {
            return (from point in joints
                where point.Target != null && point.Target.Parent != null
                select point.Target.Parent as AbstractElement).ToList();
        }
    }

    /// <summary>
    ///     Name of the element.
    /// </summary>
    public virtual string Name
    {
        get { return string.Format("{0}({1})", GetType(), Id); } // string.Format("{0}({1})", GetType().ToString(), Id)
    }

    public virtual bool Connect(AbstractElement other)
    {
        /*if (NextElement == null)
        {*/
        //Debug.Log(string.Format("connecting {0} with {1}, {2}", this, other, other != null));
            return (NextElement = other) != null;
        //}
        /*else
        {
            joints.Add(new NodeJointPoint(this, other));
        }*/
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
        var batteryU = (from battery in batteries select battery.Properties.Current).ToList<double>().Sum();

        var circuitR = (from element in listOfConnectedElements where !(element is BranchEndElement) select element.Properties.Resistance).ToList<double>().Sum();

        foreach (var connectedElement in listOfConnectedElements)
        {
            connectedElement.Properties.SetIR(batteryU / circuitR, connectedElement.Properties.Resistance);
        }
    }

    public virtual List<AbstractElement> GetListOfConnectedElements(AbstractElement begin,
        List<AbstractElement> resList = null)
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
        foreach (
            var element in
                allPaths.Where(path => path != null)
                    .SelectMany(path => path.Where(element => !totalResList.Contains(element))))
        {
            totalResList.Add(element);
        }
        return totalResList;
    }

    public virtual void Disconnect(AbstractElement other)
    {
        joints.RemoveAll(x => x.Connected(other));
    }

    public virtual bool IsClosed()
    {
        return _IsClosed();
    }

    protected virtual bool _IsClosed(AbstractElement beginningElement = null)
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

    public double GetSeriesResistance(List<AbstractElement> elements)
    {
        //return elements.Sum<AbstractElement>(x => x.Properties.Resistance);
        return (from element in elements select element.Properties.Resistance).ToList<double>().Sum();
    }

    public double GetSeriesResistance(List<double> numbers)
    {
        return numbers.Sum();
    }

    #region Constructors & destructors

    protected AbstractElement(ElectricProperties props)
    {
        if (props == null)
            props = ElectricProperties.CreateFromUR(0, 1);
        properties = props;
        joints.Add(new NodeJointPoint(this));
    }

    protected AbstractElement(double resistance)
    {
        properties = ElectricProperties.CreateFromUR(0, resistance);
        joints.Add(new NodeJointPoint(this));
        /*var message = "created " + this + "!!! Joints are " + joints.Count + " and they are ";
        message = joints.Aggregate(message, (current, point) => current + string.Format("parentId = {0}, targetId = {1}", point.ParentId, point.TargetId));
        Debug.Log(message);*/
    }

    protected AbstractElement() : this(null)
    {
    }

    #endregion

    #region Electric properties

    /// <summary>
    ///     The electric properties of element.
    /// </summary>
    public virtual ElectricProperties Properties
    {
        get
        {
            return properties;
        }
        protected set
        {
            properties = value;
        }
    }
    private ElectricProperties properties;

    public virtual void SetCurrent(double newCurrent)
    {
        properties.SetUR(newCurrent, properties.Resistance);
    }

    #endregion
}