using Everglow.Commons.DataStructures;
using Everglow.Commons.Vertex;
using Terraria.GameContent;
using Spine;

namespace Everglow.Commons.Skeleton2D;

public class Skeleton2D
{
	private Atlas atlas;
	private Skeleton skeleton;
	private AnimationState animation_state;

	public Skeleton Skeleton { get { return skeleton; } }

	public AnimationState AnimationState { get { return animation_state; } }

	public Skeleton2D(Skeleton skeleton, Atlas atlas, AnimationState state)
	{
		this.skeleton = skeleton;
		this.atlas = atlas;
		this.animation_state = state;
	}

	// public Dictionary<string, Animation> Animations { get; set; }

	public Vector2 Position { get; set; }

	public float Rotation { get; set; }

	public List<Slot> Slots { get; set; }

	/// <summary>
	/// 递归计算每个骨头的世界坐标和局部变换
	/// </summary>
	/// <param name="bone"> </param>
	/// <param name="parentTransform"> </param>
	/// <param name="position"> </param>
	public static void SettleBoneTransforms(Bone2D bone, Matrix parentTransform, Vector2 position)
	{
		var currentTransform = parentTransform * bone.GenerateLocalTransformMatrix();
		bone.LocalTransform = currentTransform;
		foreach (var child in bone.Children)
		{
			child.WorldSpacePosition = position + Vector2.Transform(child.Position, bone.LocalTransform);
			SettleBoneTransforms(child, bone.LocalTransform, child.WorldSpacePosition);
		}
	}

	//public void DrawDebugView(SpriteBatch sb)
	//{
	//	using ((SpriteBatchState.Immediate with { BlendState = BlendState.NonPremultiplied }).BeginScope(sb))
	//	{
	//		Render();
	//		Main.graphics.GraphicsDevice.RasterizerState = new RasterizerState() { FillMode = FillMode.WireFrame };

	//		// 这里使用SpriteBatch默认Shader绘制三角形
	//		SettleBoneTransforms(m_bones[0], Matrix.CreateRotationZ(Rotation), Position);

	//		RenderBonesDebug();
	//	}
	//}

	//public void InverseKinematics(Vector2 target)
	//{
	//	var posArray = new List<Vector2>();
	//	var rotationArray = new List<float>();
	//	var t_bones = new List<Bone2D>();
	//	var bone = m_bones[0];
	//	float parentRot = Rotation;
	//	float rotation = parentRot + bone.Rotation;
	//	Vector2 pos = Position + bone.Position.RotatedBy(parentRot);
	//	posArray.Add(pos);
	//	rotationArray.Add(rotation);
	//	pos += rotation.ToRotationVector2() * bone.Length;
	//	parentRot = rotation;
	//	t_bones.Add(bone);

	//	while (bone.Children.Count > 0)
	//	{
	//		bone = bone.Children[0];
	//		rotation = parentRot + bone.Rotation;
	//		pos += bone.Position.RotatedBy(parentRot);
	//		posArray.Add(pos);
	//		rotationArray.Add(rotation);
	//		pos += rotation.ToRotationVector2() * bone.Length;
	//		parentRot = rotation;
	//		t_bones.Add(bone);
	//	}

	//	var tailPos = posArray[t_bones.Count - 1] +
	//		rotationArray[t_bones.Count - 1].ToRotationVector2() * t_bones[^1].Length;

	//	for (int i = t_bones.Count - 1; i >= 0; i--)
	//	{
	//		var b = t_bones[i];

	//		// 离目标点足够近了，那就不用继续迭代
	//		if (Vector2.DistanceSquared(target, tailPos) < 0.05)
	//			break;
	//		float rr = (target - posArray[i]).ToRotation();
	//		float rr2 = MathHelper.WrapAngle((tailPos - posArray[i]).ToRotation());
	//		float rrd = MathHelper.WrapAngle(rr - rr2);
	//		b.Rotation = MathHelper.WrapAngle(b.Rotation + rrd);

	//		tailPos = posArray[i] + (tailPos - posArray[i]).RotatedBy(rrd);
	//	}
	//}

	public void PlayAnimation(string name, float time)
	{
		//var animation = Animations[name];
		//foreach (var boneTimeline in animation.BonesTimeline)
		//{
		//	foreach (var track in boneTimeline.Tracks)
		//	{
		//		track.LocateKeyFrames(time, out IKeyFrame cur, out IKeyFrame next);
		//		cur?.Interpolate(time, next);
		//	}
		//}

		//foreach (var slotTimeline in animation.SlotsTimeline)
		//{
		//	foreach (var track in slotTimeline.Tracks)
		//	{
		//		track.LocateKeyFrames(time, out IKeyFrame cur, out IKeyFrame next);
		//		cur?.Interpolate(time, next);
		//	}
		//}
	}

	/// <summary>
	/// 递归渲染骨头们的Debug View
	/// </summary>
	/// <param name="bone"> </param>
	/// <param name="vertices"> </param>
	private static void RecRenderBones(Bone2D bone, List<Vertex2D> vertices)
	{
		var Y = Vector2.TransformNormal(Vector2.UnitY, bone.LocalTransform);
		var X = Vector2.TransformNormal(Vector2.UnitX, bone.LocalTransform);

		vertices.Add(new Vertex2D(bone.WorldSpacePosition + Y * 5f - Main.screenPosition, bone.Color, Vector3.Zero));
		vertices.Add(new Vertex2D(bone.WorldSpacePosition - Y * 5f - Main.screenPosition, bone.Color, Vector3.Zero));
		vertices.Add(new Vertex2D(bone.WorldSpacePosition + X * Math.Max(4, bone.Length) - Main.screenPosition, bone.Color * 20f, Vector3.Zero));

		foreach (var child in bone.Children)
		{
			RecRenderBones(child, vertices);
		}
	}

	//private void Render()
	//{
	//	foreach (var slot in Slots)
	//	{
	//		var bone = slot.Bone;
	//		slot.Attachment?.Render(bone);
	//	}
	//}

	//private void RenderBonesDebug()
	//{
	//	var vertices = new List<Vertex2D>();

	//	foreach (var slot in Slots)
	//	{
	//		var bone = slot.Bone;
	//		RecRenderBones(bone, vertices);
	//	}

	//	Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
	//	if (vertices.Count > 2)
	//		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count / 3);
	//}
}