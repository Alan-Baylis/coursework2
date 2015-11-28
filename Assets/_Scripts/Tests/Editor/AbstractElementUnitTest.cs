using NUnit.Framework;
using NSubstitute;

[TestFixture]
public class AbstractElementUnitTest
{
	[Test]
	public void Action1 ()
	{
		var V1 = new Cable();
		var V2 = new Battery();
        V2.Properties.SetUR(5, 1);
		//var V3 = (AbstractElement)((NodeBase)fac.CreateNewNode (0, 0));

        V1.Connect(V2);
        V2.Connect(V1);
        V1.GiveProperties();

		Assert.AreEqual (V1.Properties.Amperage, V2.Properties.Amperage);
	}
}
