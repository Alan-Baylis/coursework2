using UnityEngine;
using System.Collections;

public class ElectricalCircuitWithEqualCables : ElectricalCircuitWithSpecifiedCables {
    protected Cable cableToUse;

    public ElectricalCircuitWithEqualCables(Cable singleCable = null, bool makeCopy = false)
    {
        cableToUse = makeCopy ?
            (singleCable == null ?
                Cable.DefaultCable :
                singleCable.Copy()) :
            (singleCable ?? Cable.DefaultCable);
    }
}
