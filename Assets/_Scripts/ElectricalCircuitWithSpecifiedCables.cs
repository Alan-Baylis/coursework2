using UnityEngine;
using System.Collections;
using System.Linq;

public class ElectricalCircuitWithSpecifiedCables : ElectricalCircuit
{
    public override void AddInSeries(AbstractElement element)
    {
        element.Properties = circuit.Last().Properties; 
    }

    public override void AddInParallel(AbstractElement element)
    {
        element.Properties = circuit.Last().Properties | element.Properties;
    }

    protected virtual void AddCable(Cable cable = null)
    {
        if (cable == null)
            cable = Cable.DefaultCable.Copy();
//        cable.Properties = 

    }
}
