using System;
using UnityEngine;
using System.Collections;

public class ElectricProperties {
	public virtual double Amperage {get; protected set;}
	public virtual double Current {get; protected set;}
	public virtual double Resistance {get; protected set;}

	protected ElectricProperties() {}

	public static ElectricProperties CreateFromIU(double amperage, double current) {
	    var r = new ElectricProperties {Amperage = amperage, Current = current};

	    r.Resistance = r.Current / r.Amperage;

		return r;
	}

	public static ElectricProperties CreateFromIR(double amperage, double resistance) {
	    var r = new ElectricProperties {Amperage = amperage, Resistance = resistance};

	    r.Current = r.Amperage * r.Resistance;
		
		return r;
	}

	public static ElectricProperties CreateFromUR(double current, double resistance) {
	    var r = new ElectricProperties {Current = current, Resistance = resistance};

	    r.Amperage = r.Current / r.Resistance;
		
		return r;
	}

	public void SetIU(double amperage, double current) {
		Amperage = amperage;
		Current = current;
		Resistance = Current / Amperage;
	}
	
	public void SetIR(double amperage, double resistance) {
		Amperage = amperage;
		Resistance = resistance;
		Current = Amperage * Resistance;
	}

	public void SetUR(double current, double resistance) {
		Current = current;
		Resistance = resistance;
		Amperage = Current / Resistance;
	}

	public static bool ArePropertiesValid(ElectricProperties ep, double tolerance=1e-13) {
		return Math.Abs(ep.Amperage - ep.Current / ep.Resistance) < tolerance;
	}
}