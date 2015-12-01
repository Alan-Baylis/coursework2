using NUnit.Framework;

[TestFixture]
public class CableUnitTest {
    [Test]
    public void CablePropertiesInfluenceElectricProperties()
    {
        const int crossSectionalSquare = 5;
        const int length = 5;
        var resistivity = HelperClass.GetResistivity("silver");
        var resistance = resistivity*length/crossSectionalSquare;

        var cable = new Cable(resistivity, length, crossSectionalSquare);

        Assert.AreEqual(cable.Properties.Resistance, resistance);
    }
}
