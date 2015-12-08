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
    public List<AbstractElement> elements;
    public List<Battery> Batteries { get { return elements.OfType<Battery>().ToList(); } }
    public List<ElementController> RealElements { 
        get
        {// elements.Exists(y => x.Id == y.Id) = true
            return FindObjectsOfType<ElementController>().Where(x => true).ToList();
        }
    }
    protected const float SecondsToDrag = 0.8f;

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