public class Cable : AbstractElement, ICopyable<Cable>
{
    protected const string DefaultMaterialName = "copper";
    protected const float MinimalAcceptableCableLength = 0.1f;
    protected const float MinimalAcceptableCrossSectionalSquare = 0.1f;
    protected const float MinimalCrossSectionalSquare = 0;
    protected const float BoundCableLength = 0;
    protected float crossSectionalSquare;
    protected float length;
    protected float resistivity;

    public virtual float Length
    {
        get { return length; }
        set
        {
            length = length < BoundCableLength ? MinimalAcceptableCableLength : value;
            RecalculateResistance();
        }
    }

    public virtual float Resistivity
    {
        get { return resistivity; }
        set
        {
            resistivity = value;
            RecalculateResistance();
        }
    }

    public virtual float CrossSectionalSquare
    {
        get { return crossSectionalSquare; }
        set
        {
            crossSectionalSquare = value <= MinimalCrossSectionalSquare ? MinimalAcceptableCrossSectionalSquare : value;
            RecalculateResistance();
        }
    }

    public Cable Copy()
    {
        return new Cable
        {
            AssociatedCircuit = AssociatedCircuit,
            crossSectionalSquare = crossSectionalSquare,
            length = length,
            resistivity = resistivity,
            Properties = Properties.Copy()
        };
    }

    protected void RecalculateResistance()
    {
        Properties.SetUR(Properties.Current, resistivity*length/crossSectionalSquare);
    }

    public static readonly Cable DefaultCable = new Cable
    {
        CrossSectionalSquare = MinimalAcceptableCrossSectionalSquare,
        Length = MinimalAcceptableCableLength,
        Resistivity = HelperClass.GetResistivity(DefaultMaterialName)
    };
}