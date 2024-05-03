namespace Everglow.Commons.IIID;

public struct VertexRP(Vector3 position, Vector3 texcoord, Vector3 normal, Vector3 tangent) : IVertexType
{
	public Vector3 normal = normal;

	public Vector3 position = position;

	public Vector3 tangent = tangent;

	public Vector3 texcoord = texcoord;

	private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(
					[
		new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
		new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0),
		new VertexElement(24, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
		new VertexElement(36, VertexElementFormat.Vector3, VertexElementUsage.Normal, 1),
	]);

	public readonly VertexDeclaration VertexDeclaration => _vertexDeclaration;
}