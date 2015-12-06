using UnityEngine;

public class Resistor : AbstractElement
{
    public Resistor(double resistance) : base(resistance) { }

    public override void Draw()
    {
        throw new System.NotImplementedException();
    }

    public override Rect DragableRect
    {
        get { throw new System.NotImplementedException(); }
    }
}

