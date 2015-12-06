using System;
using UnityEngine;

public class Cable : AbstractElement, ICopyable<Cable>
{
    protected double crossSectionalSquare;
    protected double length;
    protected double resistivity;

    public Cable(double resistivity, double length, double crossSectionalSquare)
        : base(resistivity*length/crossSectionalSquare)
    {
    }

    public Cable(string materialName, double length, double crossSectionalSquare)
        : base(HelperClass.GetResistivity(materialName)*length/crossSectionalSquare)
    {
    }

    public Cable() : base(null)
    {
    }

    public virtual double Length
    {
        get { return length; }
        set
        {
            length = length < BoundCableLength ? MinimalAcceptableCableLength : value;
            RecalculateResistance();
        }
    }

    public virtual double Resistivity
    {
        get { return resistivity; }
        set
        {
            resistivity = value;
            RecalculateResistance();
        }
    }

    public virtual double CrossSectionalSquare
    {
        get { return crossSectionalSquare; }
        set
        {
            crossSectionalSquare = value <=
                                   MinimalCrossSectionalSquare
                ? MinimalAcceptableCrossSectionalSquare
                : value;
            RecalculateResistance();
        }
    }

    #region implemented abstract members of NodeBase

    public override Rect DragableRect
    {
        get { throw new NotImplementedException(); }
    }

    #endregion

    public Cable Copy()
    {
        return new Cable
        {
            crossSectionalSquare = crossSectionalSquare,
            length = length,
            resistivity = resistivity,
            Properties = Properties.Copy()
        };
    }

    protected void RecalculateResistance()
    {
        Properties.SetIR(Properties.Amperage, resistivity*length/crossSectionalSquare);
    }

    #region implemented abstract members of NodeDrawableBase

    public override void Draw()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Constants

    protected const string DefaultMaterialName = "copper";
    protected const double MinimalAcceptableCableLength = 1e-3;
    protected const double MinimalAcceptableCrossSectionalSquare = 1e-5;
    protected const double MinimalCrossSectionalSquare = 1e-3;
    protected const double BoundCableLength = 0;

    #endregion

    public static readonly Cable DefaultCable = new Cable
    {
        CrossSectionalSquare = MinimalAcceptableCrossSectionalSquare,
        Length = MinimalAcceptableCableLength,
        Resistivity = HelperClass.GetResistivity(DefaultMaterialName)
    };
}