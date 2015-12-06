using System;
using UnityEngine;

public class Voltmeter : PositionalMeter
{
    
    public Voltmeter(double resistance) : base(resistance)
    {
    }

    public override Rect DragableRect
    {
        get { throw new NotImplementedException(); }
    }

    public override void Draw()
    {
        throw new NotImplementedException();
    }

    public override string GetValue()
    {
        return scale.GetValue(Properties.Current);
    }

    public override string UnitName
    {
        get { return "Volt"; }
    }

    public override double ScaleMaximum
    {
        get { return 1000; }
    }

    public override double ScaleMinimum
    {
        get { return -1000; }
    }
}