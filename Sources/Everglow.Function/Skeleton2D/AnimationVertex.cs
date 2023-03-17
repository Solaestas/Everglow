namespace Everglow.Commons.Skeleton2D;

/// <summary>
/// 绑定在当前顶点上的骨骼加权数据
/// </summary>
public class BoneBinding
{
	public Bone2D Bone
	{
		get; set;
	}

	public Vector2 BindPosition
	{
		get; set;
	}

	public float Weight
	{
		get; set;
	}
}
public class AnimationVertex
{
	public Vector2 UV
	{
		get; set;
	}

	public List<BoneBinding> BoneBindings
	{
		get; set;
	}
}
