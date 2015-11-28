using NUnit.Framework;
using NSubstitute;
[TestFixture]
public class NodeBaseUnitTest
{
	[Test]
	public void ConnectionWorks ()
	{
		var anf = new TestAbstractNodeFactory ();
		NodeBase a = anf.CreateNewNode (0, 0);
		NodeBase b = anf.CreateNewNode (0, 0);
		a.joints.Add (anf.CreateJointPoint (a));
		a.joints [0].Connect (b);
		Assert.True (a.joints [0].Connected (b), "a is not connected to b");
		//Assert.True (b.joints [0].Connected (a), "b is not connected to a");
	}
}

