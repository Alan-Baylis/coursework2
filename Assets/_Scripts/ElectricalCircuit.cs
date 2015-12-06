using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Works as a collection of elements conencted together.
/// </summary>
public class ElectricalCircuit : MonoBehaviour
{
    protected GameObject inspector;
    protected List<AbstractElement> elements;
    protected List<Battery> Batteries { get { return elements.OfType<Battery>().ToList(); } }

    public void Connect(string id1, string id2)
    {
        var firstOrDefault = elements.FirstOrDefault(x => x.Id == id1);
        if (firstOrDefault != null)
            firstOrDefault.Connect(elements.FirstOrDefault(x => x.Id == id2));

        Batteries.ForEach(x=>x.GiveProperties());
    }

    public void AddBattery()
    {
        var temp = new Battery(30, 2);
        var gameTemp = Instantiate(FindObjectOfType<ResourcesManager>().batteryPrefab);
        var gameTempController = gameTemp.GetComponent<ElementController>();
        gameTempController.Id = temp.Id;
        elements.Add(temp);
    }

    public void AddResistor()
    {

    }

    public void AddKey()
    {

    }

    public void AddCable()
    {

    }

    public void AddAmmeter()
    {

    }

    public void AddVoltmeter()
    {

    }

    public void AddOhmmeter()
    {

    }
}