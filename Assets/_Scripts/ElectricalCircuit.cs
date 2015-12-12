using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
///     Works as a collection of elements conencted together.
/// </summary>
public class ElectricalCircuit : MonoBehaviour
{
    public static ElectricalCircuit Instance
    {
        get; protected set; 
    }

    protected GameObject inspector;
    public List<AbstractElement> AllElements { get { return NodeDrawableBase.allDrawableBases.OfType<AbstractElement>().ToList<AbstractElement>(); } }
    public List<Battery> Batteries { get { return AllElements.OfType<Battery>().ToList(); } }
    public List<ElementController> realElements = new List<ElementController>();

    [UsedImplicitly]
    void Awake()
    {
        Instance = this;
    }

    [UsedImplicitly]
    void Update()
    {
        
    }

    public ElementController GetControllerByElement(AbstractElement element)
    {
        var r =
            (from element2 in realElements where element2.ElementName == element.Name select element2).ToList()
                .FirstOrDefault();
        if (r != null) return r;
        Debug.LogError("No such object");
        return null;
    }

    public AbstractElement GetElementByController(ElementController controller)
    {
        var allElements = AllElements;
        if (allElements == null)
        {
            print("All elements == null");
        }
        var r =
            (from element in AllElements where element.Name == controller.ElementName select element).ToList()
                .FirstOrDefault();
        if (r != null) return r;
        Debug.LogError("No such object");
        return null;
    }

    public void CreatePairForElement(AbstractElement element)
    {
        var type = element.GetType().ToString();
        Debug.LogFormat("Type is {0}", type);
        var pairElement = ResourcesManager.Instance.entries.FirstOrDefault(x => String.Equals(x.name, type, StringComparison.CurrentCultureIgnoreCase));
        if (pairElement == null) return;

        var gameTemp = Instantiate(pairElement.prefab);
        var gameTempController = gameTemp.GetComponentInChildren<ElementController>();
        gameTemp.transform.position = new Vector3(0, 0, 4);
        gameTempController.ElementName = element.Name;
    }

    public void Connect(string name1, string name2)
    {
        Debug.LogWarning("We are Here");
        var first = AllElements.FirstOrDefault(x => x.Name == name1);
        var second = AllElements.FirstOrDefault(x => x.Name == name2);
        if (first == null)
        {
            Debug.LogError("First element's core not found.");
            return;
        }
        if (second == null)
        {
            Debug.LogError("Second element's core not found.");
            return;
        }
        first.Connect(second);

        var battery = Batteries.FirstOrDefault();
        if (battery != null)
        {
            battery.GiveProperties();
        }
        else
        {
            Debug.LogError("No battery found!");
        }
    }

    public void AddBattery()
    {
        var temp = HelperClass.GetRandomBattery();
        CreatePairForElement(temp);
        AllElements.Add(temp);
    }

    public void AddResistor()
    {
        var temp = new Resistor(2);
        CreatePairForElement(temp);
        AllElements.Add(temp);
    }

    public void AddKey()
    {
        var temp = new Key(1, true);
        CreatePairForElement(temp);
        AllElements.Add(temp);
    }

    public void AddCable()
    {
        var temp = HelperClass.GetRandomCable(); // todo replace random cable
        CreatePairForElement(temp);
        AllElements.Add(temp);
    }

    public void AddAmmeter()
    {
        var temp = new Ammeter(4);
        CreatePairForElement(temp);
        AllElements.Add(temp);
    }

    public void AddVoltmeter()
    {
        var temp = new Voltmeter(4);
        CreatePairForElement(temp);
        AllElements.Add(temp);
    }

    public void AddOhmmeter()
    {
        var temp = new Ohmmeter(4);
        CreatePairForElement(temp);
        AllElements.Add(temp);
    }
}