public class Cable : AbstractElement, ICopyable<Cable>
{
	#region Constants
	protected const string DefaultMaterialName = "copper";
	protected const float MinimalAcceptableCableLength = 1e-3f;
	protected const float MinimalAcceptableCrossSectionalSquare = 1e-5f;
	protected const float MinimalCrossSectionalSquare = 1e-3f;
	protected const float BoundCableLength = 0;
	#endregion

	protected float crossSectionalSquare;
	protected float length;
	protected float resistivity;

	public virtual float Length {
		get { return length; }
		set {
			length = length < BoundCableLength ? MinimalAcceptableCableLength : value;
			RecalculateResistance ();
		}
	}

	public virtual float Resistivity {
		get { return resistivity; }
		set {
			resistivity = value;
			RecalculateResistance ();
		}
	}

	public virtual float CrossSectionalSquare {
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
		Properties.SetUR (Properties.Current, resistivity * length / crossSectionalSquare);
	}

	public static readonly Cable DefaultCable = new Cable
    {
        CrossSectionalSquare = MinimalAcceptableCrossSectionalSquare,
        Length = MinimalAcceptableCableLength,
        Resistivity = HelperClass.GetResistivity(DefaultMaterialName)
    };

    public Cable(ElectricProperties props=null) : base(props)
    {

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