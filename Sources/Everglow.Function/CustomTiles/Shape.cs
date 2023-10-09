using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.CustomTiles;

public static class Shape
{
	[DebuggerDisplay("position = ({position.X}, {position.Y}) size = ({size.X}, {size.Y})")]
	public struct AABB
	{
		public Vector2 position;

		public Vector2 size;

		public AABB(Vector2 position, Vector2 size)
		{
			this.position = position;
			this.size = size;
		}

		public AABB(Vector2 position, float sizeX, float sizeY)
		{
			this.position = position;
			size = new Vector2(sizeX, sizeY);
		}

		public AABB(float x, float y, float sizeX, float sizeY)
		{
			position = new Vector2(x, y);
			size = new Vector2(sizeX, sizeY);
		}

		public override readonly string ToString()
		{
			return $"({position.X}, {position.Y}, {size.X}, {size.Y})";
		}

		public readonly Vector2 TopLeft => position;

		public readonly Vector2 TopRight => position + new Vector2(size.X, 0);

		public readonly Vector2 BottomLeft => position + new Vector2(0, size.Y);

		public readonly Vector2 BottomRight => position + size;

		public readonly Vector2 Center => position + size / 2;
	}

	/// <summary>
	/// 判断两个AABB是否相交
	/// </summary>
	/// <param name="a"> </param>
	/// <param name="b"> </param>
	/// <returns> </returns>
	public static bool Intersect(this AABB a, AABB b)
	{
		if (a.position.X > b.position.X + b.size.X || a.position.X + a.size.X < b.position.X ||
			a.position.Y > b.position.Y + b.size.Y || a.position.Y + a.size.Y < b.position.Y)
		{
			return false;
		}
		return true;
	}

	public static bool Intersect(this AABB a, AABB b, out AABB area)
	{
		if (a.position.X > b.position.X + b.size.X || a.position.X + a.size.X < b.position.X ||
			a.position.Y > b.position.Y + b.size.Y || a.position.Y + a.size.Y < b.position.Y)
		{
			area = default;
			return false;
		}
		var topLeft = Vector2.Max(a.TopLeft, b.TopLeft);
		var bottomRight = Vector2.Min(a.BottomRight, b.BottomRight);
		area = new AABB(topLeft, bottomRight - topLeft);
		return true;
	}
}