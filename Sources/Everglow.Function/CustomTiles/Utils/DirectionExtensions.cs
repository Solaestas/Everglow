using Everglow.Commons.CustomTiles.Core;

namespace Everglow.Commons.CustomTiles.Utils;

public static class DirectionExtensions
{
	public static bool IsHorizontal(this Direction dir) => dir == Direction.Left || dir == Direction.Right;

	public static bool IsVertical(this Direction dir) => dir == Direction.Top || dir == Direction.Bottom;

	/// <summary>
	/// Transform <see cref="Direction"/> to <see cref="Vector2"/>.
	/// </summary>
	/// <param name="dir"> </param>
	/// <returns>
	/// Angle in unnormalized <see cref="Vector2"/>.
	/// <br/> Bottom right is <see cref="Vector2.One"/>.
	/// </returns>
	public static Vector2 ToVector2(this Direction dir) => dir switch
	{
		Direction.Top => new Vector2(0, -1),
		Direction.Left => new Vector2(-1, 0),
		Direction.Right => new Vector2(1, 0),
		Direction.Bottom => new Vector2(0, 1),
		Direction.TopLeft => new Vector2(-1, -1),
		Direction.TopRight => new Vector2(1, -1),
		Direction.BottomLeft => new Vector2(-1, 1),
		Direction.BottomRight => new Vector2(1, 1),
		_ => Vector2.Zero,
	};

	/// <summary>
	/// Transform <see cref="Direction"/> to rotation angle in radians.
	/// </summary>
	/// <param name="dir"> </param>
	/// <returns>
	/// Angle in radians, between <c>-π</c> and <c>π</c>.
	/// <br/> Right is 0.
	/// </returns>
	public static float ToRotation(this Direction dir) => dir switch
	{
		Direction.Top => -MathHelper.PiOver2,
		Direction.Left => MathHelper.Pi,
		Direction.Right => 0,
		Direction.Bottom => MathHelper.PiOver2,
		Direction.TopLeft => -MathHelper.PiOver4 * 3,
		Direction.TopRight => MathHelper.PiOver4,
		Direction.BottomLeft => MathHelper.PiOver4 * 3,
		Direction.BottomRight => MathHelper.PiOver4,
		_ or Direction.None or Direction.Inside => 0,
	};

	public static Direction GetControlDirectionH(this Player player) =>
		player.controlLeft ^ player.controlRight ? player.controlLeft ? Direction.Left : Direction.Right : Direction.None;

	public static Direction GetControlDirectionV(this Player player) =>
		player.controlUp ^ player.controlDown ? player.controlDown ? Direction.Bottom : Direction.Top : Direction.None;
}