using JetBrains.Annotations;
using UnityEngine;

public class ElementController : MonoBehaviour
{
    protected const float ElementsZ = 4;
    public string ElementName { get; set; }

    [UsedImplicitly]
    void Awake()
    {
        ElectricalCircuit.Instance.realElements.Add(this);
    }

    [UsedImplicitly]
    void Start()
    {
    }
}