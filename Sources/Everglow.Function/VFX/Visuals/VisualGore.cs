using Everglow.Commons.Enums;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.VFX.Visuals;

public abstract class VisualGore : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawNPCs;

	public Vector2 position;
	public Vector2 velocity;
	public float scale = 1;
	public float rotation = 0;
	public float alpha = 1;
	public bool tileCollide = true;
	public bool noGravity = false;
	public Texture2D texture;
	public int width;
	public int height;
	public int timer = 0;
	public int maxTime = 600;
	public float weight = 1000f;
	/// <summary>
	/// base.OnSpawn();之前必须填入Texture2D
	/// </summary>
	public override void OnSpawn()
	{
		timer = 0;
		width = texture.Width; 
		height = texture.Height;
		weight = width * height * Main.rand.NextFloat(0.85f, 1.15f);
	}
	public override void Update()
	{
		timer++;
		if(tileCollide)
		{
			float velocityValue = velocity.Length() / 25f;
			velocityValue = Math.Clamp(velocityValue, 0.0f, 1.0f);
			if (TileUtils.PlatformCollision(position + new Vector2(velocity.X, 0)))
			{
				velocity.X *= -0.75f * velocityValue;
			}
			if (TileUtils.PlatformCollision(position + new Vector2(0, velocity.Y)))
			{
				velocity.Y *= -0.75f * velocityValue;
			}
			else
			{
				if (!noGravity)
				{
					velocity.Y += 0.5f;
					velocity.X += Main.windSpeedCurrent / width * 20f;
				}
			}
		}
		else
		{
			if (!noGravity)
			{
				velocity.Y += 0.15f;
				velocity.X += Main.windSpeedCurrent / width * 20f;
			}
		}

		rotation += velocity.X / 40f;
		velocity *= MathF.Pow(0.999f,velocity.Length() / weight * 2500);

		position += velocity;

		if (timer > maxTime)
		{
			Active = false;
		}
	}
	public override void Draw()
	{
		Vector2 v0 = position + new Vector2(-width, -height).RotatedBy(rotation) * 0.5f * scale;
		Vector2 v1 = position + new Vector2(width, -height).RotatedBy(rotation) * 0.5f * scale;
		Vector2 v2 = position + new Vector2(-width, height).RotatedBy(rotation) * 0.5f * scale;
		Vector2 v3 = position + new Vector2(width, height).RotatedBy(rotation) * 0.5f * scale;

		alpha = (maxTime - timer) / 120f;
		alpha = Math.Clamp(alpha, 0.0f, 1.0f);

		Color c0 = Lighting.GetColor((v0 / 16f).ToPoint()) * alpha;
		Color c1 = Lighting.GetColor((v1 / 16f).ToPoint()) * alpha;
		Color c2 = Lighting.GetColor((v2 / 16f).ToPoint()) * alpha;
		Color c3 = Lighting.GetColor((v3 / 16f).ToPoint()) * alpha;

		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(v0, c0, new Vector3(0, 0, 0)),
			new Vertex2D(v1, c1, new Vector3(1, 0, 0)),

			new Vertex2D(v2, c2, new Vector3(0, 1, 0)),
			new Vertex2D(v3, c3, new Vector3(1, 1, 0))
		};
		Main.graphics.GraphicsDevice.Textures[0] = texture;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
	}
}