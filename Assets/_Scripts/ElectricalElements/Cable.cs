public class Cable : AbstractElement, ICopyable<Cable>
{
	#region Constants
	protected const string DefaultMaterialName = "copper";
    protected const double MinimalAcceptableCableLength = 1e-3;
    protected const double MinimalAcceptableCrossSectionalSquare = 1e-5;
    protected const double MinimalCrossSectionalSquare = 1e-3;
    protected const double BoundCableLength = 0;
	#endregion

    protected double crossSectionalSquare;
    protected double length;
	protected double resistivity;

    public virtual double Length
    {
		get { return length; }
		set {
			length = length < BoundCableLength ? MinimalAcceptableCableLength : value;
			RecalculateResistance ();
		}
	}

    public virtual double Resistivity
    {
		get { return resistivity; }
		set {
			resistivity = value;
			RecalculateResistance ();
		}
	}

    public virtual double CrossSectionalSquare
    {
		get { return crossSectionalSquare; }
		set {
			crossSectionalSquare = value <= 
				MinimalCrossSectionalSquare ? MinimalAcceptableCrossSectionalSquare : value;
			RecalculateResistance ();
		}
	}

	public Cable Copy ()
	{
		return new Cable
        {
            crossSectionalSquare = crossSectionalSquare,
            length = length,
            resistivity = resistivity,
            Properties = Properties.Copy()
        };
	}

	protected void RecalculateResistance ()
	{
		Properties.SetIR (Properties.Amperage, resistivity * length / crossSectionalSquare);
	}

	public static readonly Cable DefaultCable = new Cable
    {
        CrossSectionalSquare = MinimalAcceptableCrossSectionalSquare,
        Length = MinimalAcceptableCableLength,
        Resistivity = HelperClass.GetResistivity(DefaultMaterialName)
    };

    public Cable(ElectricProperties props=null) : base(props)
    {
        var resistance = props.Resistance;
        Resistivity = HelperClass.GetResistivity(DefaultMaterialName);
        CrossSectionalSquare = 1;
        Length = resistance * CrossSectionalSquare / Resistivity;
    }

    /*public override void GiveProperties(ElectricProperties properties, AbstractElement beginning)
    {
        if (beginning == null)
            beginning = this;
        else if (beginning == this)
            return;
        Properties = properties;
        NextElement.GiveProperties(properties, beginning);
    }*/

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