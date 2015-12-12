using System;
using System.Collections.Generic;
using UnityEngine;

public class Branch : AbstractElement
{
    public Branch() : base(null)
    {
        Branches = new List<AbstractElement>();
    }

    public List<AbstractElement> Branches { get; protected set; }

    public override ElectricProperties Properties
    {
        get
        {
            var sums = new List<double>();
            foreach (var i in Branches)
            {
                //var el = i;
                var sum = 0.0;
                for (var el = i; !(el is BranchEndElement); el = el.NextElement)
                {
                    if (el == null)
                    {
                        sum = 0;
                        break;
                    }
                    //Debug.Log(string.Format("summing {0}: sum += {1}; next element: {2}", el, el.Properties.Resistance, el.NextElement));
                    sum += el.Properties.Resistance;
                }
                //Debug.Log(string.Format("sum is {0}", sum));
                sums.Add(sum);
                //Debug.Log("---------------------");
            }
            //Debug.Log("----function ended---");
            return ElectricProperties.CreateFromIR(base.Properties.Amperage, HelperClass.GetParallelResistance(sums));
        }

        protected set
        {
            SetAmperage(value.Amperage);
        }
    }

    public override Rect DragableRect
    {
        get { throw new NotImplementedException(); }
    }

    public void CloseBranches()
    {
        foreach (var i in Branches)
        {
            var el = i;
            for (; el.NextElement != null && !(el.NextElement is BranchEndElement); el = el.NextElement)
            {
            }
            if (el.NextElement == null)
                el.Connect(BranchEndElement.BranchEnd);
        }
    }

    public override void Draw()
    {
        throw new NotImplementedException();
    }

    public override void SetAmperage(double newAmperage)
    {
        base.SetAmperage(newAmperage);
        foreach (var i in Branches)
        {
            HelperClass.DoWithChain(i, (element => element.SetAmperage(newAmperage)), x => (x != null && !(x is BranchEndElement)));
        }
    }
}