namespace Everglow.Common.CustomTile.Collide;

public class Vertices : List<Vector2>
{
	public Vertices() { }

	public Vertices(int capacity) : base(capacity) { }

	public Vertices(IEnumerable<Vector2> vertices) : base(vertices) { }
}
