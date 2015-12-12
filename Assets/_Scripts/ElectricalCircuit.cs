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
    protected GameObject inspector;
    public GameObject linePrefab;
    public GameObject linesObject;
    public List<Vector3[]> pointsOfConnections = new List<Vector3[]>();
    public List<ElementController> realElements = new List<ElementController>();
    public static ElectricalCircuit Instance { get; protected set; }

    public List<LineRenderer> LineRenderers
    {
        get { return new List<LineRenderer>(linesObject.transform.GetComponentsInChildren<LineRenderer>()); }
    }

    public List<AbstractElement> AllElements
    {
        get { return NodeDrawableBase.allDrawableBases.OfType<AbstractElement>().ToList<AbstractElement>(); }
    }

    public List<Battery> Batteries
    {
        get { return AllElements.OfType<Battery>().ToList(); }
    }

    [UsedImplicitly]
    private void Awake()
    {
        Instance = this;
    }

    [UsedImplicitly]
    private void Update()
    {
        HelperClass.DrawConnections(pointsOfConnections);
    }

    public void UpdatePointsOfConnections()
    {
        var allControllers = realElements;
        var elementsOfControllers = (from controller in allControllers select GetElementByController(controller));
        var pairsOfElements = (from element in elementsOfControllers
            where element.NextElement != null
            select new[] {element, element.NextElement});
        pointsOfConnections = (from twoElements in pairsOfElements
            let x = GetControllerByElement(twoElements[0]).transform.position
            let y = GetControllerByElement(twoElements[1]).transform.position
            select
                new[]
                {
                    new Vector3(x.x, x.y, x.z),
                    new Vector3(y.x, y.y, y.z)
                }).ToList();

        UpdateLinesCount();
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
        var pairElement =
            ResourcesManager.Instance.entries.FirstOrDefault(
                x => String.Equals(x.name, type, StringComparison.CurrentCultureIgnoreCase));
        if (pairElement == null) return;

        var gameTemp = Instantiate(pairElement.prefab);
        var gameTempController = gameTemp.GetComponentInChildren<ElementController>();
        gameTemp.transform.position = new Vector3(HelperClass.myRandom.Next(-1, 1), HelperClass.myRandom.Next(-1, 1), 4);
        gameTempController.ElementName = element.Name;
    }

    public void Connect(string name1, string name2)
    {
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
        ApplyPhysics();
        UpdatePointsOfConnections();
    }

    public void ApplyPhysics()
    {
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
        ContextMenuManager.Instance.ContextMenuActive = false;
    }

    public void AddResistor()
    {
        var temp = new Resistor(2);
        CreatePairForElement(temp);
        ContextMenuManager.Instance.ContextMenuActive = false;
    }

    public void AddKey()
    {
        var temp = new Key(1, true);
        CreatePairForElement(temp);
        ContextMenuManager.Instance.ContextMenuActive = false;
    }

    public void AddCable()
    {
        var temp = HelperClass.GetRandomCable(); // todo replace random cable
        CreatePairForElement(temp);
        ContextMenuManager.Instance.ContextMenuActive = false;
    }

    public void AddAmmeter()
    {
        var temp = new Ammeter(4);
        CreatePairForElement(temp);
        ContextMenuManager.Instance.ContextMenuActive = false;
    }

    public void AddVoltmeter()
    {
        var temp = new Voltmeter(4);
        CreatePairForElement(temp);
        ContextMenuManager.Instance.ContextMenuActive = false;
    }

    public void AddOhmmeter()
    {
        var temp = new Ohmmeter(4);
        CreatePairForElement(temp);
        ContextMenuManager.Instance.ContextMenuActive = false;
    }

    public void UpdateLinesCount()
    {
        foreach (var t in LineRenderers)
        {
            Destroy(t.gameObject);
        }
        for (var i = 0; i < pointsOfConnections.Count; ++i)
        {
            var a = Instantiate(linePrefab);
            a.transform.SetParent(linesObject.transform);
        }
    }
}