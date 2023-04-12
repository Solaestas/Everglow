using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Geometries;
internal class Mesh
{
	/// <summary>
	/// 顶点坐标
	/// </summary>
	Vector3[] _vertices;
	/// <summary>
	/// 顶点缓存
	/// </summary>
	Vector3[] _verticesbuffer;
	/// <summary>
	/// 顶点颜色
	/// </summary>
	Color[] _color;
	/// <summary>
	/// 法线图映射坐标
	/// </summary>
	Vector2[] _normals;
	/// <summary>
	/// uv图映射坐标
	/// </summary>
	Vector2[] _uv;
	/// <summary>
	/// 三角面顶点索引组元
	/// </summary>
	(int, int, int)[] _triangluarplanes;
	/// <summary>
	/// 包围盒
	/// </summary>
	public BoundaryBox Box;
	public Vector3[] Vectices => _verticesbuffer;
	public int PlaneCount => _triangluarplanes.Length;
	public Mesh(Vector3[] vs, Color[] cs, Vector2[] ns, Vector2[] uv, (int, int, int)[] ts = null)
	{
		if (vs.Length != cs.Length || vs.Length != ns.Length || vs.Length != uv.Length)
		{
			throw new ArgumentException("顶点,顶点颜色,法线映射,UV必须一一对应");
		}
		FixCenter();
		_verticesbuffer = new Vector3[_vertices.Length];
		Array.Copy(_vertices, _verticesbuffer, _vertices.Length);
		Box = new(_verticesbuffer);
		_normals = ns;
		_color = cs;
		_uv = uv;
		if (ts is not null)
		{
			foreach (var element in ts)
			{
				if (element.Item1 >= _vertices.Length || element.Item2 >= _vertices.Length || element.Item3 >= _vertices.Length)
				{
					throw new ArgumentException("三角面索引超出顶点数组长度");
				}
			}
			_triangluarplanes = ts;
		}
	}
	private Mesh()
	{
		FixCenter();
		_verticesbuffer = new Vector3[_vertices.Length];
		Array.Copy(_vertices, _verticesbuffer, _vertices.Length);
		Box = new(_verticesbuffer);
		_normals = new Vector2[_vertices.Length];
		_color = new Color[_vertices.Length];
		_uv = new Vector2[_vertices.Length];
	}
	public void FixCenter()
	{
		Vector3 fix = Vector3.zero;
		for (int i = 0; i < _vertices.Length; i++)
		{
			fix += _vertices[i];
		}
		fix /= _vertices.Length;
		for (int i = 0; i < _vertices.Length; i++)
		{
			_vertices[i] -= fix;
		}
	}
	/// <summary>
	/// 克隆网格
	/// </summary>
	/// <param name="deep"></param>
	/// <returns></returns>
	public Mesh Clone(bool deep = true) => deep ? DeepClone() : ShallowClone();
	/// <summary>
	/// 浅表克隆，共用顶点，颜色，法线，uv和三角面
	/// <br>但不共用顶点缓存，可以施加不同的transform变换</br>
	/// <br>节省内存开销，实际只产生了顶点缓存的内存开销</br>
	/// <br>修改mesh时所有共用的对象都会受到影响</br>
	/// </summary>
	/// <returns></returns>
	public Mesh ShallowClone()
	{
		Mesh mesh = new()
		{
			_vertices = _vertices,
			_verticesbuffer = new Vector3[_vertices.Length],
			_normals = _normals,
			_color = _color,
			_uv = _uv,
			_triangluarplanes = _triangluarplanes
		};
		Array.Copy(_verticesbuffer, mesh._verticesbuffer, _verticesbuffer.Length);
		return mesh;
	}
	/// <summary>
	/// 深度克隆，完全复制所有数据
	/// <br>相当于不共用原模型，会占用更多的内存</br>
	/// <br>修改时不对其他对象产生影响</br>
	/// </summary>
	/// <returns></returns>
	public Mesh DeepClone()
	{
		Mesh mesh = new()
		{
			_vertices = new Vector3[_vertices.Length],
			_normals = new Vector2[_normals.Length],
			_color = new Color[_color.Length],
			_uv = new Vector2[_uv.Length],
			_triangluarplanes = new (int, int, int)[_triangluarplanes.Length]
		};
		Array.Copy(_vertices, mesh._vertices, _vertices.Length);
		Array.Copy(_normals, mesh._normals, _normals.Length);
		Array.Copy(_color, mesh._color, _color.Length);
		Array.Copy(_uv, mesh._uv, _uv.Length);
		Array.Copy(_triangluarplanes, mesh._triangluarplanes, _triangluarplanes.Length);
		return mesh;
	}
	/// <summary>
	/// 将transform应用到网格，将计算后的顶点位置写入缓存
	/// <br>不改变uv映射关系和法线映射关系</br>
	/// </summary>
	/// <param name="transform"></param>
	public void ApplyTransform(Transform transform)
	{
		for (int i = 0; i < _vertices.Length; i++)
		{
			_verticesbuffer[i] = Vector3.Transform(_vertices[i], transform.Rotation) * transform.Scale + transform.Position;
		}
		Box = new(_verticesbuffer);
	}
	public void GetTrianglePlanes(Vector3[] buffer)
	{
		if (buffer.Length < _triangluarplanes.Length * 3)
		{
			throw new IndexOutOfRangeException("缓冲区长度不足");
		}
		for (int i = 0; i < _triangluarplanes.Length; i++)
		{
			buffer[i * 3] = _verticesbuffer[_triangluarplanes[i].Item1];
			buffer[i * 3 + 1] = _verticesbuffer[_triangluarplanes[i].Item2];
			buffer[i * 3 + 2] = _verticesbuffer[_triangluarplanes[i].Item3];
		}
	}
	public Vector3[] GetTrianglePlanes()
	{
		Vector3[] result = new Vector3[_triangluarplanes.Length * 3];
		GetTrianglePlanes(result);
		return result;
	}
	/// <summary>
	/// 占位方法，可能考虑改写为虚拟方法，由继承类具体实现
	/// </summary>
	/// <param name="writer"></param>
	/// <exception cref="NotImplementedException"></exception>
	public static void Save(BinaryWriter writer)
	{
		throw new NotImplementedException();
	}
	/// <summary>
	/// 占位方法，可能考虑改写为虚拟方法，由继承类具体实现
	/// </summary>
	/// <param name="reader"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public static Mesh Load(BinaryReader reader)
	{
		throw new NotImplementedException();
	}
	/// <summary>
	/// 占位方法，用于运行时临时生产网格
	/// <br>但我不推荐这种做法，预制网格比较好</br>
	/// <br>注:因为动态网格切分和映射创建实在是太麻烦了</br>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="newmesh"></param>
	/// <param name="objects"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public static Mesh GetMesh<T>(bool newmesh, params object[] objects)
	{
		throw new NotImplementedException();
	}
}