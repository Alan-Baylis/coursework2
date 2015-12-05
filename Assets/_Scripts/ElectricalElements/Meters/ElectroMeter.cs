using System;
using UnityEngine;
using System.Collections;

public abstract class ElectroMeter : AbstractElement {

    protected readonly double watchedVariable;
    protected Scale scale;

    public ElectroMeter(double resistance, ref double variable) : base(resistance)
    {

    }

    public abstract void SetWatchedVariable()
    {

    }
}
