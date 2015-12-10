using UnityEngine;

public class ElementController : MonoBehaviour
{
    protected const string InPointName = "inPoint";
    protected const string OutPointName = "outPoint";
    protected const string PointsParentName = "points";
    protected const float ElementsZ = 4;
    public Transform outPoint;
    public Transform inPoint;
    public string Id { get; set; }
    void Awake()
    {
        ElectricalCircuit.Instance.realElements.Add(this);
    }

    void Start()
    {
        if (inPoint == null)
            inPoint = transform.FindChild(PointsParentName).FindChild(InPointName);
        if (outPoint == null)
            outPoint = transform.FindChild(PointsParentName).FindChild(OutPointName);
    }
}