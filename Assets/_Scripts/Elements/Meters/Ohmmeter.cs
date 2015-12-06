using System;
using UnityEngine;

public class Ohmmeter : PositionalMeter
{
    public Ohmmeter(double resistance) : base(resistance)
    {
    }

    public override Rect DragableRect
    {
        get { throw new NotImplementedException(); }
    }

    public override string UnitName
    {
        get { return "Ohm"; }
    }

    public override double ScaleMaximum
    {
        get { return 500; }
    }

    public override double ScaleMinimum
    {
        get { return 0; }
    }

    public override void Draw()
    {
        throw new NotImplementedException();
    }

    public override string GetValue()
    {
        return scale.GetValue(Properties.Resistance);
    }
}