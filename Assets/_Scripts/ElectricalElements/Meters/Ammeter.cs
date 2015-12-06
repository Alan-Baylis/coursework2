using System;
using UnityEngine;
using System.Collections;

public class Ammeter : AnyMeter {

    public Ammeter(double resistance) : base(resistance)
    {
    }

    #region Graphics

    public override void Draw()
    {
        throw new NotImplementedException();
    }

    public override Rect DragableRect
    {
        get { throw new NotImplementedException(); }
    }

    #endregion

    public override string GetValue()
    {
        return scale.GetValue(Properties.Amperage);
    }

    public override string UnitName
    {
        get { return "Ampere"; }
    }

    public override double ScaleMaximum
    {
        get { return -500; }
    }

    public override double ScaleMinimum
    {
        get { return 500; }
    }
}
