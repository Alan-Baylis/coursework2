using UnityEngine;
using System.Collections;
using System;

public class Scale
{
    private const int DefaultDivisions = 100;
    private const int BigSmallDivisionsDifference = 10;
    protected readonly double Minimum;
    protected readonly double Maximum;
    protected readonly double OneUnit;

    public Scale(double newMinimum, double newMaximum)
    {
        Minimum = newMinimum;
        Maximum = newMaximum;
        OneUnit = Math.Abs(Minimum - Maximum) / DefaultDivisions;
    }

    public double GetPercentage(double number)
    {
        var wholeScale = Math.Abs(Minimum - Maximum);
        return number / wholeScale;
    }
}
