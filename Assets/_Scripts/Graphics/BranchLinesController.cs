using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BranchLinesController : MonoBehaviour
{
    public GameObject linesObject;
    public GameObject linePrefab;

    public static BranchLinesController Instance { get; protected set; }

    public List<Vector3[]> pointsPairsToConnect = new List<Vector3[]>();
    private List<LineRenderer> LineRenderers {
        get { return new List<LineRenderer>(linesObject.GetComponentsInChildren<LineRenderer>());}
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        DrawDashedConnections();
    }

    public void UpdatePointsList()
    {
        var allBranches = NodeDrawableBase.allDrawableBases.OfType<Branch>().ToList();
        
        var pairsOfElements = (from element in allBranches
                               let pairs = (from beginning in element.Branches select new[] { element, beginning }).ToList()
                               select pairs).ToList();
        var weighted = new List<AbstractElement[]>();
        pairsOfElements.ForEach(x => { x.ForEach(y => weighted.Add(y)); });
        pointsPairsToConnect = (from twoElements in weighted
                               let x = ElectricalCircuit.Instance.GetControllerByElement(twoElements[0]).transform.position
                               let y = ElectricalCircuit.Instance.GetControllerByElement(twoElements[1]).transform.position
                               select
                                   new[]
                {
                    new Vector3(x.x, x.y, x.z),
                    new Vector3(y.x, y.y, y.z)
                }).ToList();

        UpdateLinesCount();
    }

    private void UpdateLinesCount()
    {
        foreach (var t in LineRenderers)
        {
            Destroy(t.gameObject);
        }
        for (var i = 0; i < pointsPairsToConnect.Count; ++i)
        {
            var a = Instantiate(linePrefab);
            a.transform.SetParent(linesObject.transform);
        }
    }

    public void DrawDashedConnections()
    {
        foreach (var pair in pointsPairsToConnect)
        {
            var lineRenderer = Instance.LineRenderers[pointsPairsToConnect.IndexOf(pair)];
            lineRenderer.SetVertexCount(0);
            lineRenderer.SetVertexCount(2);
            var startPoint = pair[0];
            var endPoint = pair[1];
            /*const float curveThickness = 1.5f;
            var tangent = Mathf.Clamp((-1) * (startPoint.x - endPoint.x), -100, 100);
            var startTangent = new Vector2(startPoint.x + tangent, startPoint.y);
            var endTangent = new Vector2(endPoint.x - tangent, endPoint.y);*/
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
    }
}
