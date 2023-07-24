using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Terraria.GameContent;

namespace Everglow.Commons.Skeleton2D;

public class Skeleton2D
{
	public Vector2 Position
	{
		get; set;
	}
	public float Rotation
	{
		get; set;
	}
	public List<Slot> Slots
	{
		get; set;
	}

	private List<Bone2D> m_bones;

	public Dictionary<string, Animation> Animations
	{
		get; set;
	}

	public Skeleton2D(List<Bone2D> bones)
	{
		m_bones = bones;
		Slots = new List<Slot>();
		//m_bones = new List<Bone2D>();
		//var bone1 = new Bone2D();
		//bone1.Position = Vector2.Zero;
		//bone1.Rotation = (float)Main.time * 0.1f;
		//bone1.Length = 120;

		//var bone2 = new Bone2D();
		//bone2.Position = new Vector2(0, 0);
		//bone2.Rotation = (float)Main.time * 0.1f;
		//bone2.Length = 100;

		//var bone3 = new Bone2D();
		//bone3.Position = new Vector2(0, 0);
		//bone3.Rotation = (float)Main.time * 0.1f;
		//bone3.Length = 80;

		//var bone4 = new Bone2D();
		//bone4.Position = new Vector2(0, 0);
		//bone4.Rotation = (float)Main.time * 0.1f;
		//bone4.Length = 50;


		//bone3.AddChild(bone4);
		//bone2.AddChild(bone3);
		//bone1.AddChild(bone2);

		//m_bones.Add(bone1);
	}

	public void PlayAnimation(string name, float time)
	{
		var animation = Animations[name];
		foreach (var boneTimeline in animation.BonesTimeline)
		{
			foreach (var track in boneTimeline.Tracks)
			{
				track.LocateKeyFrames(time, out IKeyFrame cur, out IKeyFrame next);
				cur?.Interpolate(time, next);
			}
		}

		foreach (var slotTimeline in animation.SlotsTimeline)
		{
			foreach (var track in slotTimeline.Tracks)
			{
				track.LocateKeyFrames(time, out IKeyFrame cur, out IKeyFrame next);
				cur?.Interpolate(time, next);
			}
		}
	}


	/// <summary>
	/// 递归渲染骨头们的Debug View
	/// </summary>
	/// <param name="bone"></param>
	/// <param name="vertices"></param>
	private void _recRenderBones(Bone2D bone, List<Vertex2D> vertices)
	{
		var Y = Vector2.TransformNormal(Vector2.UnitY, bone.LocalTransform);
		var X = Vector2.TransformNormal(Vector2.UnitX, bone.LocalTransform);

		vertices.Add(new Vertex2D(bone.WorldSpacePosition + Y * 5f - Main.screenPosition, bone.Color, Vector3.Zero));
		vertices.Add(new Vertex2D(bone.WorldSpacePosition - Y * 5f - Main.screenPosition, bone.Color, Vector3.Zero));
		vertices.Add(new Vertex2D(bone.WorldSpacePosition + X * Math.Max(4, bone.Length) - Main.screenPosition, bone.Color * 20f, Vector3.Zero));


		foreach (var child in bone.Children)
		{
			_recRenderBones(child, vertices);
		}
	}

	private void Render(SpriteBatch sb)
	{
		foreach (var slot in Slots)
		{
			var bone = slot.Bone;
			slot.Attachment?.Render(bone);
			//foreach (var attachment in slot.Attachments)
			//{
			//    attachment?.Render(bone);
			//}
		}
	}

	private void RenderBones_Debug()
	{
		var vertices = new List<Vertex2D>();

		foreach (var slot in Slots)
		{
			var bone = slot.Bone;
			_recRenderBones(bone, vertices);
		}

		Main.graphics.GraphicsDevice.Textures[0] = TextureAssets.MagicPixel.Value;
		if (vertices.Count > 2)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count / 3);
	}

	public void InverseKinematics(Vector2 target)
	{
		var posArray = new List<Vector2>();
		var rotationArray = new List<float>();
		var t_bones = new List<Bone2D>();
		var bone = m_bones[0];
		float parentRot = Rotation;
		float rotation = parentRot + bone.Rotation;
		Vector2 pos = Position + bone.Position.RotatedBy(parentRot);
		posArray.Add(pos);
		rotationArray.Add(rotation);
		pos += rotation.ToRotationVector2() * bone.Length;
		parentRot = rotation;
		t_bones.Add(bone);

		while (bone.Children.Count > 0)
		{
			bone = bone.Children[0];
			rotation = parentRot + bone.Rotation;
			pos += bone.Position.RotatedBy(parentRot);
			posArray.Add(pos);
			rotationArray.Add(rotation);
			pos += rotation.ToRotationVector2() * bone.Length;
			parentRot = rotation;
			t_bones.Add(bone);
		}

		var tailPos = posArray[t_bones.Count - 1] +
			rotationArray[t_bones.Count - 1].ToRotationVector2() * t_bones[^1].Length;

		//var magicPixelTexture = TextureAssets.MagicPixel.Value;
		//Main.spriteBatch.Draw(magicPixelTexture, tailPos - Main.screenPosition, new Rectangle(0, 0, 1, 1),
		//                   Color.Red, 0f, Vector2.One * 0.5f, 3f, SpriteEffects.None, 0f);
		for (int i = t_bones.Count - 1; i >= 0; i--)
		{
			var b = t_bones[i];
			// 离目标点足够近了，那就不用继续迭代
			if (Vector2.DistanceSquared(target, tailPos) < 0.05)
				break;
			float rr = (target - posArray[i]).ToRotation();
			float rr2 = MathHelper.WrapAngle((tailPos - posArray[i]).ToRotation());
			float rrd = MathHelper.WrapAngle(rr - rr2);
			b.Rotation = MathHelper.WrapAngle(b.Rotation + rrd);

			tailPos = posArray[i] + (tailPos - posArray[i]).RotatedBy(rrd);
		}
	}

	/// <summary>
	/// 递归计算每个骨头的世界坐标和局部变换
	/// </summary>
	/// <param name="bone"></param>
	/// <param name="parentTransform"></param>
	/// <param name="position"></param>
	public void _settleBoneTransforms(Bone2D bone, Matrix parentTransform, Vector2 position)
	{
		var currentTransform = parentTransform * bone.GenerateLocalTransformMatrix();
		bone.LocalTransform = currentTransform;
		foreach (var child in bone.Children)
		{
			child.WorldSpacePosition = position + Vector2.Transform(child.Position, bone.LocalTransform);
			_settleBoneTransforms(child, bone.LocalTransform, child.WorldSpacePosition);
		}
	}

	public void DrawDebugView(SpriteBatch sb)
	{
		var state = GraphicsUtils.SaveSpriteBatchState(sb);
		sb.End();
		{
			sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
			Render(sb);
			sb.End();
			Main.graphics.GraphicsDevice.RasterizerState = new RasterizerState() { FillMode = FillMode.WireFrame };
			// 这里使用SpriteBatch默认Shader绘制三角形

			_settleBoneTransforms(m_bones[0], Matrix.CreateRotationZ(Rotation), Position);

			RenderBones_Debug();
		}
		GraphicsUtils.RestoreSpriteBatchState(sb, state);
	}
}
