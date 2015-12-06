public abstract class AnyMeter : AbstractElement
{
    protected Scale scale;

    protected AnyMeter(double resistance) : base(resistance)
    {
        scale = new Scale(ScaleMinimum, ScaleMaximum, UnitName);
    }

    public abstract string GetValue();
    public abstract string UnitName { get; }
    public abstract double ScaleMaximum { get; }
    public abstract double ScaleMinimum { get; }

}