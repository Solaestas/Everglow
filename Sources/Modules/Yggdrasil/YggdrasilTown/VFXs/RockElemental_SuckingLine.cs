using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class RockElemental_SuckingLinePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.RockElemental_SuckingLine;
	}

	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_cell.Value);
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D FlameColor = ModAsset.HeatMap_RockElemental_SuckingLine.Value;
		Ins.Batch.BindTexture<Vertex2D>(FlameColor);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.PointClamp, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}

[Pipeline(typeof(RockElemental_SuckingLinePipeline))]
public class RockElemental_SuckingLine : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Queue<Vector2> oldPos = new Queue<Vector2>();
	public Projectile VFXOwner;
	public Vector2 position;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;

	public override void Update()
	{
		if(VFXOwner == null || !VFXOwner.active || VFXOwner.type != ModContent.ProjectileType<RockElemental_ThrowingStone>())
		{
			Active = false;
			return;
		}
		RockElemental_ThrowingStone rockElemental_ThrowingStone = VFXOwner.ModProjectile as RockElemental_ThrowingStone;
		if(rockElemental_ThrowingStone != null)
		{
			if(rockElemental_ThrowingStone.PolymerizationTimer < 0)
			{
				timer += 2;
			}
			else
			{
				velocity = velocity.RotatedBy(ai[1]);
				Vector2 pierceAim = VFXOwner.Center - velocity - position;
				if (pierceAim.Length() < 30)
				{
					timer += 2;
				}
				velocity = Vector2.Lerp(velocity, Utils.SafeNormalize(pierceAim, Vector2.zeroVector) * 9f, 0.1f);
			}
		}
		oldPos.Enqueue(position);
		if (oldPos.Count > 30)
		{
			oldPos.Dequeue();
		}
		timer++;
		if (timer > maxTime)
		{
			Active = false;
			return;
		}
		position += velocity;
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.04f;
		Lighting.AddLight(position, c * 0.5f, c * 0.1f, c * 0.8f);
	}

	public override void Draw()
	{
		int len = oldPos.Count;
		var bars = new List<Vertex2D>();
		if (len <= 2)
		{
			for (int i = 1; i < 3; i++)
			{
				bars.Add(position, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0 + ai[0], (i + 15 - len) / 17f, 1));
				bars.Add(position, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0.6f + ai[0], (i + 15 - len) / 17f, 1));
			}
		}
		else
		{
			Vector2[] pos = oldPos.Reverse().ToArray();
			for (int i = 1; i < len; i++)
			{
				float pocession = timer / maxTime;
				if (timer - i < 20)
				{
					pocession += (20 - timer + i) / 20f;
				}
				pocession = Math.Clamp(pocession, 0, 1);
				Vector2 normal = pos[i] - pos[i - 1];
				normal = Vector2.Normalize(normal).RotatedBy(Math.PI * 0.5);
				float width = scale * (float)Math.Sin(i / (double)len * Math.PI);
				bars.Add(pos[i] + normal * width, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0 + ai[0], (i + 15 - len) / 17f, pocession));
				bars.Add(pos[i] - normal * width, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0.6f + ai[0], (i + 15 - len) / 17f, pocession));
			}
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}