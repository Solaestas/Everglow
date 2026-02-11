using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;

namespace Everglow.Myth.LanternMoon.Gores;
public abstract class BurningGore : VisualGore
{
	public float rotateSpeed;
	/// <summary>
	/// 轻度值,下降速度的减缓程度,0~0.1为佳
	/// </summary>
	public float LightValue = 0;

	/// <summary>
	/// 溶解动画灰度图
	/// </summary>
	public Texture2D DissolveAnimationTexture;

	/// <summary>
	/// 不溶解的部分贴图
	/// </summary>
	public Texture2D NoDissolvePartTexture;

	/// <summary>
	/// 是否启用骨骼
	/// </summary>
	public bool HasBone = false;


	/// <summary>
	/// 随机值
	/// </summary>
	public float[] ai;


	public virtual void SetRandomValues()
	{
	}
	public override void OnSpawn()
	{
		base.OnSpawn();
		SetRandomValues();
	}

	public override void Update()
	{
		timer++;
		scale *= 0.999f;
		if ((width <= 0 || height <= 0) && Texture is not null)
		{
			width = Texture.Width;
			height = Texture.Height;
			weight = width * height * Main.rand.NextFloat(0.85f, 1.15f);
		}
		if (tileCollide)
		{
			float velocityValue = velocity.Length() / 25f;
			velocityValue = Math.Clamp(velocityValue, 0.0f, 1.0f);
			if (TileUtils.PlatformCollision(position + new Vector2(velocity.X, 0)))
			{
				velocity.X = 0;
			}
			if (TileUtils.PlatformCollision(position + new Vector2(0, velocity.Y)))
			{
				velocity.Y = 0;
			}
			else
			{
				if (!noGravity)
				{
					velocity.Y += LightValue;
					velocity.X += Main.windSpeedCurrent / width * 20f;
				}
			}
		}
		else
		{
			if (!noGravity)
			{
				velocity.Y += LightValue;
				velocity.X += Main.windSpeedCurrent / width * 20f;
			}
		}

		rotation += rotateSpeed;
		velocity *= MathF.Pow(0.999f, velocity.Length() / weight * 2500);

		position += velocity;

		if (timer > maxTime)
		{
			Active = false;
		}
	}
	public override void Draw()
	{
		if (NoDissolvePartTexture == null)
		{
			return;
		}
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

		var bars = new List<Vertex2D>()
		{
			new Vertex2D(v0, c0, new Vector3(0, 0, 0)),
			new Vertex2D(v1, c1, new Vector3(1, 0, 0)),

			new Vertex2D(v2, c2, new Vector3(0, 1, 0)),
			new Vertex2D(v3, c3, new Vector3(1, 1, 0)),
		};
		Ins.Batch.Draw(NoDissolvePartTexture, bars, PrimitiveType.TriangleStrip);
	}

	public virtual void DrawDissolvePart()
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

		float alpha2 = (timer - 100) / (maxTime - 100f);
		alpha2 = Math.Clamp(alpha2, 0.0f, 1.0f);

		var bars = new List<Vertex2D>()
		{
			new Vertex2D(v0, c0, new Vector3(0, 0, alpha2)),
			new Vertex2D(v1, c1, new Vector3(1, 0, alpha2)),

			new Vertex2D(v2, c2, new Vector3(0, 1, alpha2)),
			new Vertex2D(v3, c3, new Vector3(1, 1, alpha2)),
		};
		Ins.Batch.Draw(Texture, bars, PrimitiveType.TriangleStrip);
	}

}
