using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class AbstractElement : MonoBehaviour
{
    public const string DefaultNameOfElement = "Element";

    protected AbstractElement(ElectricProperties props, string name = DefaultNameOfElement)
    {
        if (props == null)
            props = ElectricProperties.CreateFromUR(0, 0);
        Properties = props;
        name = NameRefinition(name);
        Names.Add(name);
        Name = name;
    }

    protected AbstractElement() : this(null)
    {
    }

    public string Name { get; set; }
    protected virtual ElectricProperties Properties { get; set; }
    public ElectricalCircuit AssociatedCircuit { get; set; }
    public static List<string> Names { get; protected set; }

    public virtual double GetAmperage()
    {
        return Properties.Amperage;
    }

    public virtual double GetCurrent()
    {
        return Properties.Current;
    }

    public virtual double GetResistance()
    {
        return Properties.Resistance;
    }

    public T GetPreviousElement<T>() where T : AbstractElement
    {
        return AssociatedCircuit[AssociatedCircuit.IndexOfByName(Name) - 1] as T;
    }

    public T GetNextElement<T>() where T : AbstractElement
    {
        return AssociatedCircuit[AssociatedCircuit.IndexOfByName(Name) + 1] as T;
    }

    private static string NameRefinition(string name)
    {
        if (!Names.Contains(name)) return name;
        var r = new Regex(@"\(\d+\)");
        var m = r.Match(name);
        if (m.Success)
        {
            var succ = m.Groups[m.Groups.Count - 1].ToString();
            var num = Convert.ToInt32(succ.Substring(1, succ.Length - 1));
            name = name.Replace(succ, string.Format("({0})", num));
        }
        else
        {
            name += " (1)";
        }

        return name;
    }
}