using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Summon;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class BeetleDashTrace_frontPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.BeetleDashTrace_front;
	}

	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D halo = Commons.ModAsset.Trail_10.Value;
		Ins.Batch.BindTexture<Vertex2D>(halo);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}

[Pipeline(typeof(BeetleDashTrace_frontPipeline))]
public class BeetleDashTrace_frontDust : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawProjectiles;

	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;
	public float rotation;
	public Projectile projectileOwner;
	public Queue<Vector2> trails = new Queue<Vector2>();

	public override void Update()
	{
		if (projectileOwner == null || !projectileOwner.active || projectileOwner.type != ModContent.ProjectileType<DeadBeetleEgg_beetle>())
		{
			Active = false;
			return;
		}
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			Active = false;
			return;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			Active = false;
			return;
		}
		position = projectileOwner.Center + Vector2.Normalize(projectileOwner.velocity) * 40;
		velocity = projectileOwner.velocity;
		trails.Enqueue(position);
		if (trails.Count > 30)
		{
			trails.Dequeue();
		}
		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
	}

	public override void Draw()
	{
		float timeValue = timer * 0.02f;
		float colorValue = 1;
		if (timer > maxTime - 30)
		{
			colorValue = (maxTime - timer) / 30f;
		}
		List<Vertex2D> bars = new List<Vertex2D>();
		if (trails.Count >= 3)
		{
			for (int i = 0; i < trails.Count; i++)
			{
				Vector2 pos = trails.ToArray()[i];
				Vector2 posNext = trails.ToArray()[i] + velocity;
				if (i != trails.Count - 1)
				{
					posNext = trails.ToArray()[i + 1];
				}
				float drawWidth = i / (float)(trails.Count - 1);
				Color drawColor = Lighting.GetColor(pos.ToTileCoordinates());
				drawColor.A = 0;
				drawColor *= colorValue * drawWidth;
				drawWidth = MathF.Cos(MathF.Pow(drawWidth, 3) * MathHelper.PiOver2);

				Vector2 width = Utils.SafeNormalize(pos - posNext, Vector2.One).RotatedBy(MathHelper.PiOver2) * 40;
				bars.Add(pos + width, drawColor, new Vector3(i * 0.05f + timeValue, 0.3f, drawWidth));
				bars.Add(pos - width, drawColor, new Vector3(i * 0.05f + timeValue, 0.7f, drawWidth));
			}
		}
		else
		{
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
			bars.Add(position, Color.Transparent, new Vector3(0, 0, 0));
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}