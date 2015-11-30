using System.Collections.Generic;
using NUnit.Framework;
using System;
[TestFixture]
public class AbstractElementUnitTest
{
	[Test]
	public void ConnectingClosedCircuitWith2Elements ()
	{
        /*
         * (0)---(1)
         *   \___/
         */
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
        /*
         * (0)--(1)--(2)
         *   \_______/
         */
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
        /*
         *     .(0)----(1)---(8).
         *    /  |      |\      |
         *   (5) |      | --(9) |
         *    \  |      |     \ |
         *     -(4)----(2)----(10)
         *      /        \
         *     /          \
         *   (6)---(7)    (3)
         */
        var random = new Random();
        var elementsList = new List<AbstractElement>
        {
            HelperClass.GetRandomBattery(random),
            HelperClass.GetRandomCable(random),
            HelperClass.GetRandomCable(random),
            HelperClass.GetRandomCable(random),
            HelperClass.GetRandomCable(random),
            HelperClass.GetRandomCable(random),
            HelperClass.GetRandomCable(random),
            HelperClass.GetRandomCable(random),
            HelperClass.GetRandomCable(random),
            HelperClass.GetRandomCable(random),
            HelperClass.GetRandomCable(random),
        };

        HelperClass.Join(elementsList, 0, 1);
        HelperClass.Join(elementsList, 1, 2);
        HelperClass.Join(elementsList, 1, 8);
        HelperClass.Join(elementsList, 1, 9);
        HelperClass.Join(elementsList, 2, 3);
        HelperClass.Join(elementsList, 2, 4);
        HelperClass.Join(elementsList, 4, 5);
        HelperClass.Join(elementsList, 4, 0);
        HelperClass.Join(elementsList, 4, 6);
        HelperClass.Join(elementsList, 4, 7);
        HelperClass.Join(elementsList, 5, 0);
        HelperClass.Join(elementsList, 8, 10);
        HelperClass.Join(elementsList, 9, 10);
        HelperClass.Join(elementsList, 10, 2);

        elementsList[0].GiveProperties();

        for (var i = 1; i < elementsList.Count; ++i)
        {
            if (i == 3 || i == 6 || i == 7) continue;
            Assert.AreNotEqual(elementsList[i].Properties.Amperage, elementsList[3].Properties.Amperage,
                string.Format("index = {0}, element = {1}", i, elementsList[i].Name));
            Assert.AreNotEqual(elementsList[i].Properties.Amperage, elementsList[6].Properties.Amperage,
                string.Format("index = {0}, element = {1}", i, elementsList[i].Name));
            Assert.AreNotEqual(elementsList[i].Properties.Amperage, elementsList[7].Properties.Amperage,
                string.Format("index = {0}, element = {1}", i, elementsList[i].Name));
        }
        Assert.AreEqual(elementsList[1].Properties.Amperage, elementsList[8].Properties.Amperage,
            string.Format("element = {0}", elementsList[8].Name));
        Assert.AreEqual(elementsList[1].Properties.Amperage, elementsList[9].Properties.Amperage,
            string.Format("element = {0}", elementsList[9].Name));
    }
}
