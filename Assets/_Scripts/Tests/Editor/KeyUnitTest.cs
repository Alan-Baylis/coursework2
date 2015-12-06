using System;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class KeyUnitTest {
    [Test]
    public void KeyMakesDifference()
    {
        var random = new Random();
        var key = new Key(electricProperties: ElectricProperties.CreateFromIR(10, 2));
        var elementsList = new List<AbstractElement>
        {
            HelperClass.GetRandomBattery(random),
            HelperClass.GetRandomCable(random),
            key,
            HelperClass.GetRandomCable(random)
        };

        HelperClass.Join(elementsList, 0, 1);
        HelperClass.Join(elementsList, 1, 2);
        HelperClass.Join(elementsList, 2, 3);
        HelperClass.Join(elementsList, 3, 0);
        ((Battery)elementsList[0]).GiveProperties();

        Assert.AreNotEqual(elementsList[0].Properties.Amperage, elementsList[1].Properties.Amperage);

        key.Switch();

        ((Battery)elementsList[0]).GiveProperties();

        Assert.AreEqual(elementsList[0].Properties.Amperage, elementsList[1].Properties.Amperage);
    }
}
