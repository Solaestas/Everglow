namespace Everglow.Commons.Vertex;

public static class VertexExtensions
{
	public static IList<Vertex2D> Add(this IList<Vertex2D> list, Vector2 position, Color color, Vector3 texCoord)
	{
		list.Add(new Vertex2D(position, color, texCoord));
		return list;
	}

	public static IList<Vertex3D> Add(this IList<Vertex3D> list, Vector3 position, Vector3 texcoord, Vector3 normal)
	{
		list.Add(new Vertex3D(position, texcoord, normal));
		return list;
	}

	public static IList<Vertex3D_2> Add(this IList<Vertex3D_2> list, Vector3 position, Vector3 texCoord, Color color)
	{
		list.Add(new Vertex3D_2(position, texCoord, color));
		return list;
	}
}