using System;

public class ElectricProperties
{
    protected ElectricProperties()
    {
    }

    public virtual double Amperage { get; protected set; }
    public virtual double Current { get; protected set; }
    public virtual double Resistance { get; protected set; }
    // ReSharper disable once InconsistentNaming
    public static ElectricProperties CreateFromIU(double amperage, double current)
    {
        var r = new ElectricProperties {Amperage = amperage, Current = current};
        r.Resistance = r.Current/r.Amperage;
        return r;
    }

    // ReSharper disable once InconsistentNaming
    public static ElectricProperties CreateFromIR(double amperage, double resistance)
    {
        var r = new ElectricProperties {Amperage = amperage, Resistance = resistance};
        r.Current = r.Amperage*r.Resistance;
        return r;
    }

    // ReSharper disable once InconsistentNaming
    public static ElectricProperties CreateFromUR(double current, double resistance)
    {
        var r = new ElectricProperties {Current = current, Resistance = resistance};
        r.Amperage = r.Current/r.Resistance;
        return r;
    }


    // ReSharper disable once InconsistentNaming
    public void SetIU(double amperage, double current)
    {
        Amperage = amperage;
        Current = current;
        Resistance = Current/Amperage;
    }

    // ReSharper disable once InconsistentNaming
    public void SetIR(double amperage, double resistance)
    {
        Amperage = amperage;
        Resistance = resistance;
        Current = Amperage*Resistance;
    }

    public void SetUR(double current, double resistance)
    {
        Current = current;
        Resistance = resistance;
        Amperage = Current/Resistance;
    }

    public static bool ArePropertiesValid(ElectricProperties ep, double tolerance = 1e-13)
    {
        return Math.Abs(ep.Amperage - ep.Current/ep.Resistance) < tolerance;
    }

    /// <summary>
    ///     Series circuiting.
    /// </summary>
    /// <param name="a">first properties</param>
    /// <param name="b">second properties</param>
    /// <returns>resulting properties after connecting elements in series</returns>
    public static ElectricProperties operator &(ElectricProperties a, ElectricProperties b)
    {
        a.SetIR(a.Amperage, a.Resistance + b.Resistance);
        return a;
    }

    /// <summary>
    ///     Parallel circuiting.
    /// </summary>
    /// <param name="a">first properties</param>
    /// <param name="b">second properties</param>
    /// <returns>resulting properties after connecting elements in parrallel</returns>
    public static ElectricProperties operator |(ElectricProperties a, ElectricProperties b)
    {
        a.SetIR(a.Amperage, 1/a.Resistance + 1/b.Resistance);
        return a;
    }
}