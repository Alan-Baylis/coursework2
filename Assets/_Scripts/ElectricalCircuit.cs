using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngineInternal;

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
    public List<AbstractElement> AllElements { get { return AbstractElement.allDrawableBases.OfType<AbstractElement>().ToList<AbstractElement>(); } }
    public List<Battery> Batteries { get { return AllElements.OfType<Battery>().ToList(); } }
    public List<ElementController> realElements = new List<ElementController>();

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        /*var timer = 0f;
        while (!Input.GetMouseButton(0))
        {
            Debug.LogFormat("timer = {0}", timer);
            timer += Time.deltaTime;
        }
        if (timer < SecondsToDrag)
        {
            Debug.Log("In Connect Mode");
            //currentMode = ElementController.ElementMode.Connect;
        }
        else
        {
            Debug.Log("In Drag Mode");
            //transform.parent.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ElementsZ));
        }*/
    }

    public ElementController GetControllerByElement(AbstractElement element)
    {
        return (from element2 in realElements where element2.Id == element.Id select element2).ToList().FirstOrDefault<ElementController>();
    }

    public AbstractElement GetElementByController(ElementController controller)
    {
        return (from element in AllElements where element.Id == controller.Id select element).ToList().FirstOrDefault<AbstractElement>();
    }

    public void CreatePairForElement(AbstractElement element)
    {
        var type = element.GetType().ToString();
        Debug.LogFormat("Type is {0}", type);
        var gameTemp = Instantiate(ResourcesManager.Instance.entries.FirstOrDefault<ResourcesManager.StringGameObject>(x => x.name.ToLower() == type.ToLower()).prefab);
        var gameTempController = gameTemp.GetComponentInChildren<ElementController>();
        gameTemp.transform.position = new Vector3(0, 0, 4);
        gameTempController.Id = element.Id;
    }

    public void Connect(string id1, string id2)
    {
        var firstOrDefault = AllElements.FirstOrDefault(x => x.Id == id1);
        if (firstOrDefault != null)
            firstOrDefault.Connect(AllElements.FirstOrDefault(x => x.Id == id2));

        Batteries.FirstOrDefault<Battery>().GiveProperties();
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
        var temp = HelperClass.GetRandomCable();
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