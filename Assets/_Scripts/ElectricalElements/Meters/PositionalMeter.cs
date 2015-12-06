using System.Collections.Generic;

public abstract class PositionalMeter : AnyMeter
{
    protected AbstractElement measuredSubcircuit;

    protected PositionalMeter(double resistance) : base(resistance)
    {

    }

    public override ElectricProperties Properties
    {
        get
        {
            var tempProperties = base.Properties.Copy();
            tempProperties.SetIR(tempProperties.Amperage, (double) HelperClass.GetPropertiesOfChain(measuredSubcircuit, "resistance") + base.Properties.Resistance);
            return tempProperties;
        }
        protected set
        {
            SetAmperage(value.Amperage);
        }
    }

    public override void SetAmperage(double newAmperage)
    {
        base.SetAmperage(newAmperage);
        HelperClass.DoWithChain(measuredSubcircuit, (element => element.SetAmperage(newAmperage)));
    }

    public void SetMeasuredSubcircuit(AbstractElement abstractElement)
    {
        measuredSubcircuit = abstractElement;
    }

    public override string ToString()
    {
        var temp = new List<AbstractElement>();
        for (var i = measuredSubcircuit; i != null; i = i.NextElement)
        {
            temp.Add(i);
        }
        return string.Format("{0}{{{1}}}", base.ToString(), temp.GetReadableList());
    }
}