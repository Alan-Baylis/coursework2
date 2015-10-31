using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public class ElectricalCircuit : MonoBehaviour
{
    protected List<AbstractElement> circuit;

    public int IndexOfByName(string name)
    {
        return circuit.FindIndex(x => x.Name == name);
    }

    public T GetElementByName<T>(string name) where T : AbstractElement
    {
        return circuit.FirstOrDefault(x => x.Name == name) as T;
    }

    public AbstractElement this[int i]
    {
        get { return i > circuit.Count || i < 0 ? null : circuit[i]; }
        protected set
        {
            if (i <= circuit.Count && i >= 0)
            {
                circuit[i] = value;
            }
        }
    }
}
