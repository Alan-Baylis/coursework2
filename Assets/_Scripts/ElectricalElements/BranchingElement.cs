using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchingElement : AbstractElement {

    public List<AbstractElement> Branches { get; protected set; }

    public BranchingElement() : base(null)
    {
        Branches = new List<AbstractElement>();
    }

    private double current;

    public override ElectricProperties Properties
    {
        get
        {
            var sums = new List<double>();
            foreach (var i in Branches)
            {
                //var el = i;
                var sum = 0.0;
                for (var el = i; !(el is BranchEndElement); el = el.NextElement )
                {
                    if (el == null)
                    {
                        sum = 0;
                        break;
                    }
                    Debug.Log(string.Format("summing {0}: sum += {1}; next element: {2}", el, el.Properties.Resistance, el.NextElement));
                    sum += el.Properties.Resistance;
                }
                Debug.Log(string.Format("sum is {0}", sum));
                sums.Add(sum);
                Debug.Log("---------------------");
            }
            Debug.Log("----function ended---");
            return ElectricProperties.CreateFromUR(current, HelperClass.GetParallelResistance(sums));
        }

        protected set
        {
            SetCurrent(value.Current);
        }
    }

    public void CloseBranches()
    {
        foreach (var i in Branches)
        {
            var el = i;
            for (; el != null && !(el is BranchEndElement); el = el.NextElement);
            if (!(el is BranchEndElement))
                el = BranchEndElement.BranchEnd;
        }
    }

    public override void Draw()
    {
        throw new System.NotImplementedException();
    }

    public override Rect DragableRect
    {
        get { throw new System.NotImplementedException(); }
    }

    public override void SetCurrent(double current)
    {
        this.current = current;
    }
}
