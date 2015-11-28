using System;
using System.Collections.Generic;

public class ElectricProperties : ICopyable<ElectricProperties>
{
	public const double LeastAmperage = 1e-6;
	public const double LeastCurrent = 1e-6;

	public bool IsConsideredPowered ()
	{
		return Math.Abs (Amperage - LeastAmperage) < ToleranceOfEquality && Math.Abs (Current - LeastCurrent) < ToleranceOfEquality;
	}

	protected bool Equals (ElectricProperties other)
	{
		return Amperage.Equals (other.Amperage) && Current.Equals (other.Current) && Resistance.Equals (other.Resistance);
	}

	public override bool Equals (object obj)
	{
		if (ReferenceEquals (null, obj))
			return false;
		if (ReferenceEquals (this, obj))
			return true;
		return obj.GetType () == this.GetType () && Equals ((ElectricProperties)obj);
	}

	public override int GetHashCode ()
	{
		unchecked {
			var hashCode = Amperage.GetHashCode ();
			hashCode = (hashCode * 397) ^ Current.GetHashCode ();
			hashCode = (hashCode * 397) ^ Resistance.GetHashCode ();
			return hashCode;
		}
	}

	public const double ToleranceOfEquality = 1e-5;

	protected ElectricProperties ()
	{
	}

	// I = U / R
	public virtual double Amperage { get; protected set; }
	public virtual double Current { get; protected set; }
	public virtual double Resistance { get; protected set; }
	// ReSharper disable once InconsistentNaming
	public static ElectricProperties CreateFromIU (double amperage, double current)
	{
		var r = new ElectricProperties {Amperage = amperage, Current = current};
		r.Resistance = r.Current / r.Amperage;
		return r;
	}

	// ReSharper disable once InconsistentNaming
	public static ElectricProperties CreateFromIR (double amperage, double resistance)
	{
		var r = new ElectricProperties {Amperage = amperage, Resistance = resistance};
		r.Current = r.Amperage * r.Resistance;
		return r;
	}

	// ReSharper disable once InconsistentNaming
	public static ElectricProperties CreateFromUR (double current, double resistance)
	{
		var r = new ElectricProperties {Current = current, Resistance = resistance};
		r.Amperage = r.Current / r.Resistance;
		return r;
	}


	// ReSharper disable once InconsistentNaming
	public void SetIU (double amperage, double current)
	{
		Amperage = amperage;
		Current = current;
		Resistance = Current / Amperage;
	}

	// ReSharper disable once InconsistentNaming
	public void SetIR (double amperage, double resistance)
	{
		Amperage = amperage;
		Resistance = resistance;
		Current = Amperage * Resistance;
	}

	public void SetUR (double current, double resistance)
	{
		Current = current;
		Resistance = resistance;
		Amperage = Current / Resistance;
	}

	public static bool ArePropertiesValid (ElectricProperties ep, double tolerance = 1e-13)
	{
		return Math.Abs (ep.Amperage - ep.Current / ep.Resistance) < tolerance;
	}

	/// <summary>
	///     Series circuiting.
	/// </summary>
	/// <param name="a">first properties</param>
	/// <param name="b">second properties</param>
	/// <returns>resulting properties after connecting elements in series</returns>
	public static ElectricProperties operator & (ElectricProperties a, ElectricProperties b)
	{
		a.SetUR (a.Current, a.Resistance + b.Resistance);
		return a;
	}

	/// <summary>
	///     Parallel circuiting.
	/// </summary>
	/// <param name="a">first properties</param>
	/// <param name="b">second properties</param>
	/// <returns>resulting properties after connecting elements in parrallel</returns>
	public static ElectricProperties operator | (ElectricProperties a, ElectricProperties b)
	{
		a.SetUR (a.Current, 1 / a.Resistance + 1 / b.Resistance);
		return a;
	}

	public ElectricProperties Copy ()
	{
		return new ElectricProperties
        {
            Amperage = Amperage,
            Current = Current,
            Resistance = Resistance
        };
	}
	/*List<double> ap = new List<double> (new double[]{a.Amperage, a.Current, a.Resistance});
		List<double> bp = new List<double> (new double[]{b.Amperage, b.Current, b.Resistance});

		if (ap.Contains (null) || bp.Contains (null))
			return false;*/
	/*public static bool operator == (ElectricProperties a, ElectricProperties b)
	{

		return Math.Abs (a.Amperage - b.Amperage) < ToleranceOfEquality &&
			Math.Abs (a.Current - b.Current) < ToleranceOfEquality &&
			Math.Abs (a.Resistance - b.Resistance) < ToleranceOfEquality;
	}

	public static bool operator != (ElectricProperties a, ElectricProperties b)
	{
		return !(a == b);
	}*/
}