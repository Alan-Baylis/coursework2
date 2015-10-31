using UnityEngine;
using System.Collections;

public abstract class Cable : AbstractElement
{
    protected float length;
    protected float resistivity;
    protected float crossSectionalSquare;

    protected const float MinimalAcceptableCableLength = 0.1f;
    protected const float MinimalAcceptableCrossSectionalSquare = 0.1f;
    protected const float MinimalCrossSectionalSquare = 0;
    protected const float BoundCableLength = 0;

    public virtual float Length
    {
        get
        {
            return length;
        }
        set
        {
            length = length < BoundCableLength ? MinimalAcceptableCableLength : value;
            RecalculateResistance();
        }
    }

    public virtual float Resistivity
    {
        get
        {
            return resistivity;
        }
        set
        {
            // TODO logic
            resistivity = value;
            RecalculateResistance();
        }
    }

    public virtual float CrossSectionalSquare
    {
        get
        {
            return crossSectionalSquare;
        }
        set
        {
            crossSectionalSquare = value <= MinimalCrossSectionalSquare ? MinimalAcceptableCrossSectionalSquare : value;
            RecalculateResistance();
        }
    }

    public override double GetCurrent()
    {
        return base.GetCurrent();
    }

    public override double GetAmperage()
    {
        return base.GetAmperage();
    }

    protected override ElectricProperties Properties
    {
        get { return base.Properties; }
        set { base.Properties = value; }
    }

    private void RecalculateResistance()
    {
        Properties.SetUR(Properties.Current, resistivity*length/crossSectionalSquare);
    }
}
