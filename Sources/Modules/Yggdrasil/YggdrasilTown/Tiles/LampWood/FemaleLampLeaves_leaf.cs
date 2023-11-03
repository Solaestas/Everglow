using Everglow.Commons.VFX.Scene;
using SubworldLibrary;

namespace Everglow.Yggdrasil.Common;
[Pipeline(typeof(WCSPipeline))]
public class FemaleLampLeaves_leaf : BackgroundVFX
{
	public override void OnSpawn()
	{
		texture = ModAsset.FemaleLampWood_leaves_dark.Value;
	}
	public override void Update()
	{
		if (!SubworldSystem.IsActive<YggdrasilWorld>())
		{
			Active = false;
		}
		foreach(Player player in Main.player)
		{
			if(player != null && player.active && !player.dead)
			{
				if (Collision.CheckAABBvLineCollision(player.position, new Vector2(player.Hitbox.Width, player.Hitbox.Height), position, position + new Vector2(80, 140).RotatedBy(rotation) * scale))
				{
					omega -= Vector3.Cross(new Vector3(player.velocity, 0), new Vector3(new Vector2(80, 140).RotatedBy(rotation), 0)).Z / 30000f;
				}
			}
		}
		omega += (startRotation - rotation) / 100f;
		rotation += omega;
		rotation = rotation * 0.92f + startRotation * 0.08f;
		base.Update();
	}
	public float rotation;
	public float omega;
	public float startRotation;
	public float scale;
	public override void Draw()
	{
		Color lightColor0 = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);
		Color lightColor1 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)position.Y / 16);
		Color lightColor2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y + texture.Height) / 16);
		Color lightColor3 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)(position.Y + texture.Height) / 16);

		Ins.Batch.BindTexture<Vertex2D>(texture);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position, lightColor0, new Vector3(0, 0, 0)),
			new Vertex2D(position + new Vector2(texture.Width, 0).RotatedBy(rotation) * scale,lightColor1, new Vector3(1, 0, 0)),

			new Vertex2D(position + new Vector2(0, texture.Height).RotatedBy(rotation) * scale,lightColor2, new Vector3(0, 1, 0)),
			new Vertex2D(position + new Vector2(texture.Width, texture.Height).RotatedBy(rotation) * scale,lightColor3, new Vector3(1, 1, 0))
		};
		if (direction < 0)
		{
			bars = new List<Vertex2D>()
			{
				new Vertex2D(position, lightColor0, new Vector3(1, 0, 0)),
				new Vertex2D(position + new Vector2(texture.Width, 0).RotatedBy(rotation) * scale,lightColor1, new Vector3(0, 0, 0)),

				new Vertex2D(position + new Vector2(0, texture.Height).RotatedBy(rotation) * scale,lightColor2, new Vector3(1, 1, 0)),
				new Vertex2D(position + new Vector2(texture.Width, texture.Height).RotatedBy(rotation) * scale,lightColor3, new Vector3(0, 1, 0))
			};
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
public class FemaleLampLeaves_leaf_fore : ForegroundVFX
{
	public override void OnSpawn()
	{
		texture = ModAsset.FemaleLampWood_leaves.Value;
	}
	public override void Update()
	{
		if (!SubworldSystem.IsActive<YggdrasilWorld>())
		{
			Active = false;
		}
		foreach (Player player in Main.player)
		{
			if (player != null && player.active && !player.dead)
			{
				if (Collision.CheckAABBvLineCollision(player.position, new Vector2(player.Hitbox.Width, player.Hitbox.Height), position, position + new Vector2(80, 140).RotatedBy(rotation) * scale))
				{
					omega -= Vector3.Cross(new Vector3(player.velocity, 0), new Vector3(new Vector2(80, 140).RotatedBy(rotation), 0)).Z / 30000f;
				}
			}
		}
		omega += (startRotation - rotation) / 100f;
		rotation += omega;
		rotation = rotation * 0.92f + startRotation * 0.08f;
		base.Update();
	}
	public float rotation;
	public float omega;
	public float startRotation;
	public float scale;
	public override void Draw()
	{
		Color lightColor0 = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16);
		Color lightColor1 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)position.Y / 16);
		Color lightColor2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y + texture.Height) / 16);
		Color lightColor3 = Lighting.GetColor((int)(position.X + texture.Width) / 16, (int)(position.Y + texture.Height) / 16);

		Ins.Batch.BindTexture<Vertex2D>(texture);
		List<Vertex2D> bars = new List<Vertex2D>()
		{
			new Vertex2D(position, lightColor0, new Vector3(0, 0, 0)),
			new Vertex2D(position + new Vector2(texture.Width, 0).RotatedBy(rotation) * scale,lightColor1, new Vector3(1, 0, 0)),

			new Vertex2D(position + new Vector2(0, texture.Height).RotatedBy(rotation) * scale,lightColor2, new Vector3(0, 1, 0)),
			new Vertex2D(position + new Vector2(texture.Width, texture.Height).RotatedBy(rotation) * scale,lightColor3, new Vector3(1, 1, 0))
		};
		if (direction < 0)
		{
			bars = new List<Vertex2D>()
			{
				new Vertex2D(position, lightColor0, new Vector3(1, 0, 0)),
				new Vertex2D(position + new Vector2(texture.Width, 0).RotatedBy(rotation) * scale,lightColor1, new Vector3(0, 0, 0)),

				new Vertex2D(position + new Vector2(0, texture.Height).RotatedBy(rotation) * scale,lightColor2, new Vector3(1, 1, 0)),
				new Vertex2D(position + new Vector2(texture.Width, texture.Height).RotatedBy(rotation) * scale,lightColor3, new Vector3(0, 1, 0))
			};
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}
