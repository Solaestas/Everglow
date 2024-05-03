namespace Everglow.Commons.IIID;

public class Model
{
	/// <summary>
	/// V:代表顶点。格式为V X Y Z<br/>
	/// V后面的X Y Z表示三个顶点坐标。浮点型
	/// </summary>
	public List<Vector3> Positions = [];

	/// <summary>
	/// 表示纹理坐标。格式为VT TU TV。浮点型
	/// </summary>
	public List<Vector2> TexCoords = [];

	/// <summary>
	/// VN:法向量。每个三角形的三个顶点都要指定一个法向量。格式为VN NX NY NZ。浮点型
	/// </summary>
	public List<Vector3> Normals = [];

	/// <summary>
	/// F:面。面后面跟着的整型值分别是属于这个面的顶点、纹理坐标、法向量的索引。<br/>
	/// 面的格式为:f Vertex1/Texture1/Normal1 Vertex2/Texture2/Normal2 Vertex3/Texture3/Normal3
	/// </summary>
	public List<Face> Faces = [];
}