using UnityEngine;

public class ElementController : MonoBehaviour
{
    public enum ElementMode
    {
        Drag, Connect, Idle
    }
    protected const string InPointName = "inPoint";
    protected const string OutPointName = "outPoint";
    protected const float ElementsZ = 4;
    protected const float SecondsToDrag = 0.8f;
    public Transform OutPoint { get; protected set; }
    public Transform InPoint { get; protected set; }
    public string Id { get; set; }
    public ElementMode currentMode;
    // Use this for initialization

    private void Start()
    {
        if (InPoint == null)
            InPoint = transform.FindChild(InPointName);
        if (OutPoint == null)
            OutPoint = transform.FindChild(OutPointName);
    }

    // Update is called once per frame
    private void OnMouseDrag()
    {
        var timer = 0f;
        while (timer <= SecondsToDrag)
        {
            timer += Time.deltaTime;
        }
        transform.parent.position =
            Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ElementsZ));
    }

    private void Update()
    {
    }
}