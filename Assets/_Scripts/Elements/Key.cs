using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Key : AbstractElement {
    public bool On { get; set; }

    public virtual void Switch()
    {
        On = !On;
    }

    public Key(bool state=false, ElectricProperties electricProperties=null) : base(electricProperties)
    {
        On = state;
    }

    public Key(double resistance, bool state = false)
        : this(state, ElectricProperties.CreateFromUR(0, resistance))
    {
    }

    public override bool Conductive
    {
        get
        {
            return base.Conductive && On;
        }
    }

    #region Deprecated
    public override void Draw()
    {
        throw new System.NotImplementedException();
    }

    public override Rect DragableRect
    {
        get { throw new System.NotImplementedException(); }
    }
    #endregion
}
