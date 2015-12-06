using System;

public class Scale
{
    private const int DefaultDivisions = 100;
    private const int BigSmallDivisionsDifference = 10;
    protected readonly double maximum;
    protected readonly double minimum;
    protected readonly double oneUnit;
    protected string suffix;

    public Scale(double newMinimum, double newMaximum, string newSuffix)
    {
        minimum = newMinimum;
        maximum = newMaximum;
        oneUnit = Math.Abs(minimum - maximum)/DefaultDivisions;
        suffix = newSuffix;
    }

    public double GetPercentage(double number)
    {
        var wholeScale = Math.Abs(minimum - maximum);
        return number/wholeScale;
    }

    public string GetValue(double cur)
    {
        return string.Format("{0} {1}", Math.Round(cur, 2), suffix);
    }
}