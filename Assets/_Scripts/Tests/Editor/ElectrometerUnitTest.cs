using System;
using System.Collections;
using NUnit.Framework;

[TestFixture]
public class ElectrometerUnitTest {

    [Test]
    public void OhmmeterTest()
    {
        var battery = HelperClass.GetRandomBattery();
        var cable1 = new Cable("test_one", 1, 2);
        var cable2 = new Cable("test_one", 1, 2);
        var ohmmeter = new Ohmmeter(2);
        battery.Connect(ohmmeter);
        ohmmeter.Connect(battery);
        ohmmeter.SetMeasuredSubcircuit(cable1);
        cable1.Connect(cable2);

        battery.GiveProperties();

        Assert.AreEqual((cable1.Properties.Resistance + cable2.Properties.Resistance + 2).ToString(), ohmmeter.GetValue().Replace(" Ohm", ""));
    }

    [Test]
    public void VoltmeterTest()
    {
        var battery = HelperClass.GetRandomBattery();
        var cable1 = new Cable("test_one", 1, 2);
        var cable2 = new Cable("test_one", 1, 2);
        var voltmeter = new Voltmeter(2);
        battery.Connect(voltmeter);
        voltmeter.Connect(battery);
        voltmeter.SetMeasuredSubcircuit(cable1);
        cable1.Connect(cable2);

        battery.GiveProperties();

        Assert.AreEqual((battery.Properties.Amperage * (cable1.Properties.Resistance + cable2.Properties.Resistance + 2)).ToString(), voltmeter.GetValue().Replace(" Volt", ""));
    }
}
