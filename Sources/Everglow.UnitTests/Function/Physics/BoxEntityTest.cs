using Everglow.Commons.CustomTiles.Core;
using Microsoft.Xna.Framework;

namespace Everglow.UnitTests.Function.Physics;

[TestClass]
public class BoxEntityTest
{
	public class BoxEntity_Test : BoxEntity
	{
	}

	[TestMethod]
	public void HasCollision_ReturnTrue()
	{
		var rigidBox = new BoxEntity_Test
		{
			Position = Vector2.Zero,
			Size = new Vector2(10, 10),
		};

		var box = new BoxImpl()
		{
			Position = Vector2.Zero,
			Size = new Vector2(10, 20),
			Gravity = 1,
		};

		Assert.IsTrue(rigidBox.Collision(box, Vector2.One, out var _));
	}

	[TestMethod]
	public void NoCollision_ReturnFalse()
	{
		var rigidBox = new BoxEntity_Test
		{
			Position = Vector2.Zero,
			Size = new Vector2(10, 10),
		};

		var box = new BoxImpl()
		{
			Position = new Vector2(20, 20),
			Size = new Vector2(10, 20),
			Gravity = 1,
		};

		Assert.IsFalse(rigidBox.Collision(box, Vector2.One, out var _));
	}
}