using NUnit.Framework;

[TestFixture]
public class ElectricPropertiesUnitTest
{
    public double DefaultAmperage
    {
        get { return 5; }
    }

    public double DefaultCurrent
    {
        get { return 2; }
    }

    public double DefaultResistance
    {
        get { return DefaultCurrent/DefaultAmperage; }
    }

    [Test]
    public void CorrectCreation()
    {
        var ourObject1 = ElectricProperties.CreateFromIR(DefaultAmperage, DefaultResistance);
        var ourObject2 = ElectricProperties.CreateFromUR(DefaultCurrent, DefaultResistance);
        var ourObject3 = ElectricProperties.CreateFromIU(DefaultAmperage, DefaultCurrent);

        Assert.AreEqual(ourObject1.Amperage, ourObject2.Amperage, ElectricProperties.ToleranceOfEquality);
        Assert.AreEqual(ourObject1.Current, ourObject2.Current, ElectricProperties.ToleranceOfEquality);
        Assert.AreEqual(ourObject1.Resistance, ourObject2.Resistance, ElectricProperties.ToleranceOfEquality);

        Assert.AreEqual(ourObject2.Amperage, ourObject3.Amperage, ElectricProperties.ToleranceOfEquality);
        Assert.AreEqual(ourObject2.Current, ourObject3.Current, ElectricProperties.ToleranceOfEquality);
        Assert.AreEqual(ourObject2.Resistance, ourObject3.Resistance, ElectricProperties.ToleranceOfEquality);
    }

    [Test]
    public void CorrectConnections()
    {
        var props = ElectricProperties.CreateFromIR(DefaultAmperage, DefaultResistance);

        Assert.AreEqual(props.Resistance*2, (props & props).Resistance);
        Assert.AreEqual(2.0/props.Resistance, (props | props).Resistance);
    }
}