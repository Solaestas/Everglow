namespace Everglow.Commons.Skeleton2D;

public abstract class Attachment
{
	public Texture2D Texture
	{
		get; set;
	}

	public abstract void Render(Bone2D bone);
}

public class RegionAttachment : Attachment
{
	public Vector2 Position
	{
		get; set;
	}
	public float Rotation
	{
		get; set;
	}

	public Vector2 Size
	{
		get; set;
	}

	public override void Render(Bone2D bone)
	{
		var transform = bone.LocalTransform;
		var X = Vector2.TransformNormal(Vector2.UnitX, transform);
		float rotWorld = X.ToRotation();

		Main.spriteBatch.Draw(Texture, bone.WorldSpacePosition + Vector2.Transform(Position, transform) - Main.screenPosition,
			null, Color.White, rotWorld + Rotation, new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
	}
}


public class MeshAttachment : Attachment
{
	public List<AnimationVertex> AnimationVertices
	{
		get; set;
	}

	public List<int> TriangleIndices
	{
		get; set;
	}

	public override void Render(Bone2D bone)
	{
		var vertices = new List<Vertex2D>();
		foreach (var v in AnimationVertices)
		{
			float weight = 0f;
			Vector2 targetPos = Vector2.Zero;
			foreach (var binding in v.BoneBindings)
			{
				targetPos += binding.Bone.GetWorldPosition(binding.BindPosition) * binding.Weight;
				weight += binding.Weight;
			}
			targetPos *= 1f / weight;
			var vx = new Vertex2D(targetPos - Main.screenPosition, Color.White, new Vector3(v.UV, 0f));
			vertices.Add(vx);
		}

		Main.graphics.GraphicsDevice.Textures[0] = Texture;
		if (vertices.Count > 2)
		{

			Main.graphics.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, vertices.Count,
				TriangleIndices.ToArray(), 0, TriangleIndices.Count / 3);
		}
	}
}
