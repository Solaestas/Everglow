using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Common.CustomTile.Collide;
using Everglow.Common.CustomTile.DataStructures;

namespace Everglow.Common.CustomTile;
public static class Extensions
{
	public static bool IsH(this Direction dir) => dir == Direction.Left || dir == Direction.Right;
	public static bool IsV(this Direction dir) => dir == Direction.Top || dir == Direction.Bottom;
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
		_ => Vector2.Zero
	};
	/// <summary>
	/// 以朝右为0, -π &lt; 返回值 &lt;= π
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
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
	public static CAABB GetCollider(this Entity entity) => new(new AABB(entity.position.X, entity.position.Y, entity.width, entity.height));



}
