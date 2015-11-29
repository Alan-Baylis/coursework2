using UnityEngine;
using System.Collections;

public class Key : AbstractElement {
    public Key(ElectricProperties electricProperties=null) : base(electricProperties)
    {
        
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
