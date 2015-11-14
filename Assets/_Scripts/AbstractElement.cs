using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class AbstractElement : MonoBehaviour
{
    public const string DefaultNameOfElement = "Element";

    protected AbstractElement(ElectricProperties props, string id = null)
    {
        if (props == null)
            props = ElectricProperties.CreateFromUR(0, 0);
        Properties = props;
        id = IdRefinition(id);
        Id = id;
        Ids.Add(id);
    }

    protected AbstractElement() : this(null)
    {
    }

    /// <summary>
    ///     It is the unique identifier of element.
    /// </summary>
    public string Id { get; protected set; }

    /// <summary>
    ///     Name of the element.
    /// </summary>
    public virtual string Name
    {
        get { return DefaultNameOfElement; }
    }

    /// <summary>
    ///     The electric properties of element.
    /// </summary>
    public ElectricProperties Properties { get; set; }

    public ElectricalCircuit AssociatedCircuit { get; set; }
    public static List<string> Ids { get; protected set; }

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

    private static string IdRefinition(string id)
    {
        if (!Ids.Contains(id)) return id;
        var r = new Regex(@"\(\d+\)");
        var m = r.Match(id);
        if (m.Success)
        {
            var oldValue = m.Groups[m.Groups.Count - 1].ToString();
            var numberInParenthesis = Convert.ToInt32(oldValue.Substring(1, oldValue.Length - 1));
            id = id.Replace(oldValue, string.Format("({0})", numberInParenthesis + 1));
        }
        else
        {
            id += " (1)";
        }

        return id;
    }
}