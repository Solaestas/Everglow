namespace Everglow.Common.CustomTile.DataStructures;

[DebuggerDisplay("begin = ({begin.X}, {begin.Y}) end = ({end.X}, {end.Y})")]
public struct Edge
{
	public Vector2 begin;
	public Vector2 end;
	public Vector2 BeginToEnd => end - begin;
	public float K => (end.Y - begin.Y) / (end.X - begin.X);
	public float B => begin.Y - K * begin.X;
	public Vector2 NormalLine => new(end.Y - begin.Y, begin.X - end.Y);
	public Edge(Vector2 begin, Vector2 end)
	{
		this.begin = begin;
		this.end = end;
	}

	public override string ToString()
	{
		return $"({begin.X}, {begin.Y}) ({end.X}, {end.Y})";
	}
}
