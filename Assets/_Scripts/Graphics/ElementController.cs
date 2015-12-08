using UnityEngine;

public class ElementController : MonoBehaviour
{
    protected const string InPointName = "inPoint";
    protected const string OutPointName = "outPoint";
    protected const string PointsParentName = "points";
    protected const float ElementsZ = 4;
    public Transform OutPoint { get; protected set; }
    public Transform InPoint { get; protected set; }
    public string Id { get; set; }
    // Use this for initialization

    void Start()
    {
        if (InPoint == null)
            InPoint = transform.FindChild(PointsParentName).FindChild(InPointName);
        if (OutPoint == null)
            OutPoint = transform.FindChild(PointsParentName).FindChild(OutPointName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}