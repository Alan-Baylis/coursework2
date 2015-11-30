using System.Collections.Generic;
using NUnit.Framework;
using System;
[TestFixture]
public class AbstractElementUnitTest
{
    /// <summary>
    /// Connects element from list to another element from that list.
    /// </summary>
    /// <param name="all">list</param>
    /// <param name="ind1">index of element which you connect</param>
    /// <param name="ind2">index of element to which you connect</param>
    void Join(List<AbstractElement> all, int ind1, int ind2)
    {
        all[ind1].Connect(all[ind2]);
    }

	[Test]
	public void Action1 ()
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
         *     _(0)----(1)
         *    /  |      |
         *   (5) |      |
         *    \  |      |
         *     -(4)----(2)
         *      /        \
         *     /          \
         *   (6)          (3)
         */
        var random = new Random();
        var elementsList = new List<AbstractElement>
        {
            new Battery(ElectricProperties.CreateFromUR(random.Next(10, 20), random.Next(1, 3))),
            new Cable(ElectricProperties.CreateFromUR(0, random.Next(1, 15))),
            new Cable(ElectricProperties.CreateFromUR(0, random.Next(1, 15))),
            new Cable(ElectricProperties.CreateFromUR(0, random.Next(1, 15))),
            new Cable(ElectricProperties.CreateFromUR(0, random.Next(1, 15))),
            new Cable(ElectricProperties.CreateFromUR(0, random.Next(1, 15))),
            new Cable(ElectricProperties.CreateFromUR(0, random.Next(1, 15)))
        };
        const int batteryIndex = 0;
        const int el1Index = 1;
        const int el2Index = 2;
        const int el3Index = 3;
        const int el4Index = 4;
        const int el5Index = 5;
        const int el6Index = 6;

        Join(elementsList, batteryIndex, el1Index);
        Join(elementsList, el1Index, el2Index);
        Join(elementsList, el2Index, el3Index);
        Join(elementsList, el2Index, el4Index);
        Join(elementsList, el4Index, el5Index);
        Join(elementsList, el4Index, batteryIndex);
        Join(elementsList, el4Index, el6Index);
        Join(elementsList, el5Index, batteryIndex);

        elementsList[batteryIndex].GiveProperties();

        for (var i = 1; i < 6; ++i)
        {
            if (i == el3Index || i == el6Index) continue;
            Assert.AreNotEqual(elementsList[i].Properties.Amperage, elementsList[el3Index].Properties.Amperage,
                string.Format("index = {0}, element = {1}", i, elementsList[i].Name));
            Assert.AreNotEqual(elementsList[i].Properties.Amperage, elementsList[el6Index].Properties.Amperage,
                string.Format("index = {0}, element = {1}", i, elementsList[i].Name));
        }
    }
}
