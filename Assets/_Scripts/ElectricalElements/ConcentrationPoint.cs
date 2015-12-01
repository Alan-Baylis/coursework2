using UnityEngine;
using System.Collections;

public class ConcentrationPoint : AbstractElement {
    public int InBranchesNum { get; set; }
    public int OutBranchesNum { get; set; }

    public override ElectricProperties Properties
    {
        get
        {
            var resultResistance = 0;
            

        }
    }

    public ConcentrationPoint() : base(null)
    {
    }

    public override void Connect(AbstractElement other)
    {
        base.Connect(other);
        OutBranchesNum++;
    }

    public override void Disconnect(AbstractElement other)
    {
        base.Disconnect(other);
        OutBranchesNum--;
    }

    public override void Draw()
    {
        throw new System.NotImplementedException();
    }

    public override Rect DragableRect
    {
        get { throw new System.NotImplementedException(); }
    }
}
