namespace Everglow.Commons.Skeleton2D;

public class Bone2D
{
	/// <summary>
	/// 骨骼的名字
	/// </summary>
	public string Name
	{
		get; set;
	}

	/// <summary>
	/// 相对于父骨骼的位移变化量
	/// </summary>
	public Vector2 Position
	{
		get; set;
	}

	/// <summary>
	/// 相对于父骨骼变换的旋转变换
	/// </summary>
	public float Rotation
	{
		get; set;
	}

	/// <summary>
	/// 相对于父骨骼变换的缩放变换
	/// </summary>
	public Vector2 Scale
	{
		get; set;
	}

	/// <summary>
	/// 骨骼朝着指定方向延伸的长度
	/// </summary>
	public float Length
	{
		get; set;
	}

	/// <summary>
	/// 父骨骼实例
	/// </summary>
	public Bone2D Parent
	{
		get; set;
	}

	/// <summary>
	/// 它所绑定的子骨骼们
	/// </summary>
	public List<Bone2D> Children => m_children;

	public Color Color
	{
		get; set;
	}

	/// <summary>
	/// 骨头的局部坐标变换矩阵
	/// </summary>
	public Matrix LocalTransform
	{
		get; set;
	}


	public Vector2 WorldSpacePosition
	{
		get; set;
	}

	private List<Bone2D> m_children = new();

	public Bone2D()
	{
		LocalTransform = Matrix.Identity;
	}

	public Matrix GenerateLocalTransformMatrix()
	{
		return Matrix.CreateScale(new Vector3(Scale, 1f)) *
			Matrix.CreateRotationZ(Rotation);
	}

	public Vector2 GetWorldPosition(Vector2 localPosition)
	{
		return WorldSpacePosition + Vector2.Transform(localPosition, LocalTransform);
	}

	public void AddChild(Bone2D bone)
	{
		bone.Parent = this;
		m_children.Add(bone);
	}
}
