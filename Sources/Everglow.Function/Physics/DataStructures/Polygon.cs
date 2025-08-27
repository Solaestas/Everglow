namespace Everglow.Commons.Physics.DataStructures;

public class Polygon
{
	public Polygon()
	{
	}

	public Polygon(List<Vector2> points)
	{
		Points = points;
	}

	public List<Vector2> Points { get; private set; }
}