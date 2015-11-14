using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Works as a collection of elements conencted together.
/// </summary>
public abstract class ElectricalCircuit : MonoBehaviour
{
    protected List<AbstractElement> circuit;

    public AbstractElement this[int i]
    {
        get
        {
            return i > circuit.Count || i < 0 ? null : circuit[i];
        }
        protected set
        {
            if (i <= circuit.Count && i >= 0)
            {
                circuit[i] = value;
            }
        }
    }

    public int IndexOfByName(string elementName)
    {
        return circuit.FindIndex(x => x.Name == elementName);
    }

    public T GetElementByName<T>(string elementName) where T : AbstractElement
    {
        return circuit.FirstOrDefault(x => x.Name == elementName) as T;
    }

    public abstract void AddInSeries(AbstractElement element);

    public abstract void AddInParallel(AbstractElement element);
}