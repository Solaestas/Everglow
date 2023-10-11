namespace Everglow.Commons.Collider;

public static class Extensions
{
	public static bool IsH(this Direction dir)
	{
		return dir is Direction.Left or Direction.Right;
	}

	public static bool IsV(this Direction dir)
	{
		return dir is Direction.Up or Direction.Down;
	}

	public static Vector2 ToVector2(this Direction dir)
	{
		return dir switch
		{
			Direction.Up => new Vector2(0, -1),
			Direction.Left => new Vector2(-1, 0),
			Direction.Right => new Vector2(1, 0),
			Direction.Down => new Vector2(0, 1),
			Direction.UpLeft => new Vector2(-1, -1),
			Direction.UpRight => new Vector2(1, -1),
			Direction.DownLeft => new Vector2(-1, 1),
			Direction.DownRight => new Vector2(1, 1),
			_ => Vector2.Zero
		};
	}

	/// <summary>
	/// 以朝右为0, -π &lt; 返回值 &lt;= π
	/// </summary>
	/// <param name="dir"> </param>
	/// <returns> </returns>
	public static float ToRotation(this Direction dir)
	{
		return dir switch
		{
			Direction.Up => -MathHelper.PiOver2,
			Direction.Left => MathHelper.Pi,
			Direction.Right => 0,
			Direction.Down => MathHelper.PiOver2,
			Direction.UpLeft => -MathHelper.PiOver4 * 3,
			Direction.UpRight => MathHelper.PiOver4,
			Direction.DownLeft => MathHelper.PiOver4 * 3,
			Direction.DownRight => MathHelper.PiOver4,
			_ or Direction.None or Direction.In => 0,
		};
	}

	public static Direction GetControlDirectionH(this Player player)
	{
		return player.controlLeft ^ player.controlRight ? player.controlLeft ? Direction.Left : Direction.Right : Direction.None;
	}

	public static Direction GetControlDirectionV(this Player player)
	{
		return player.controlUp ^ player.controlDown ? player.controlDown ? Direction.Down : Direction.Up : Direction.None;
	}
}