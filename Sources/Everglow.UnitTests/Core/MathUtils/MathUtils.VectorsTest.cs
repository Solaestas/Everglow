using Microsoft.Xna.Framework;
using Everglow.Commons.Utilities;

namespace Everglow.UnitTests.Core.MathUtils;

[TestClass]
public class MathUtils_VectorsTest
{
	[TestMethod]
	public void Approach_ArgsValueNotEnough_ReturnTarget()
	{
		var from = new Vector2(0, 0);
		var to = new Vector2(3, 4);
		var result = from.Approach(to, 5);
		Assert.AreEqual(to, result);
	}

	[TestMethod]
	public void Approach_ArgsValueEnough_ReturnApproached()
	{
		var from = new Vector2(0, 0);
		var to = new Vector2(3, 4);
		var result = from.Approach(to, 2);
		Assert.AreEqual(new Vector2(1.2f, 1.6f), result);

		from = new Vector2(0, 0);
		to = new Vector2(6, 8);
		result = from.Approach(to, 5);
		Assert.AreEqual(new Vector2(3, 4), result);
	}
}