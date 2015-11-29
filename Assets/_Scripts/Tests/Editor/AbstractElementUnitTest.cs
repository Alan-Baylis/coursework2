using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class AbstractElementUnitTest
{
	[Test]
	public void Action1 ()
	{
		var cable = new Cable(ElectricProperties.CreateFromUR(0, 10));
        var battery = new Battery(ElectricProperties.CreateFromUR(5, 2));

        cable.Connect(battery);
        battery.Connect(cable);
        cable.GiveProperties();

		Assert.AreEqual (cable.Properties.Amperage, battery.Properties.Amperage);
	}

    [Test]
    public void ConnectingClosedCircuitAndChecking()
    {
        var cable1 = new Cable(ElectricProperties.CreateFromUR(0, 10));
        var battery = new Battery(ElectricProperties.CreateFromUR(2, 10));
        var cable2 = new Cable(ElectricProperties.CreateFromUR(3, 10));
        cable2.Properties.SetUR(2, 3);

        cable1.Connect(battery);
        battery.Connect(cable2);
        cable2.Connect(cable1);
        cable1.GiveProperties();
        Assert.AreEqual(battery.Properties.Amperage, cable1.Properties.Amperage);
    }

    [Test]
    public void ConnectingBigCircuitWithLotsOfBranches()
    {
        var elementsList = new List<AbstractElement>
        {
            new Battery(ElectricProperties.CreateFromUR(10, 1)),
            new Cable(ElectricProperties.CreateFromUR(0, 5)),
            new Cable(ElectricProperties.CreateFromUR(0, 6)),
            new Cable(ElectricProperties.CreateFromUR(0, 9)),
            new Cable(ElectricProperties.CreateFromUR(0, 3)),
            new Cable(ElectricProperties.CreateFromUR(0, 10)),
            new Cable(ElectricProperties.CreateFromUR(0, 4))
        };
        const int batteryIndex = 0;
        const int el1Index = 1;
        const int el2Index = 2;
        const int el3Index = 3;
        const int el4Index = 4;
        const int el5Index = 5;
        const int el6Index = 6;

        elementsList[batteryIndex].Connect(elementsList[el1Index]);

        elementsList[el1Index].Connect(elementsList[el2Index]);

        elementsList[el2Index].Connect(elementsList[el3Index]);
        elementsList[el2Index].Connect(elementsList[el4Index]);

        elementsList[el4Index].Connect(elementsList[el5Index]);
        elementsList[el4Index].Connect(elementsList[batteryIndex]);
        elementsList[el4Index].Connect(elementsList[el6Index]);

        elementsList[el5Index].Connect(elementsList[batteryIndex]);

        elementsList[batteryIndex].GiveProperties();

        for (var i = 1; i < 6; ++i)
        {
            if (i == el3Index || i == el6Index) continue;
            Assert.AreNotEqual(elementsList[i].Properties.Amperage, elementsList[el3Index].Properties.Amperage, string.Format("index = {0}, type = {1}", i, elementsList[i].GetType()));
            Assert.AreNotEqual(elementsList[i].Properties.Amperage, elementsList[el6Index].Properties.Amperage, string.Format("index = {0}, type = {1}", i, elementsList[i].GetType()));
        }
    }
}
