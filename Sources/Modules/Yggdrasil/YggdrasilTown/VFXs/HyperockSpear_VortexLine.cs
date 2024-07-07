using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.RockElemental;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs;

public class HyperockSpear_VortexLinePipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.HyperockSpear_VortexLine;
	}

	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_cell.Value);
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D FlameColor = ModAsset.HeatMap_HyperockSpear_VortexLine.Value;
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

[Pipeline(typeof(HyperockSpear_VortexLinePipeline))]
public class HyperockSpear_VortexLine : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Queue<Vector2> oldPos = new Queue<Vector2>();
	public Projectile VFXOwner;
	public Vector2 positiontoProjectile;
	public Vector2 velocity;
	public bool OnTile;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;

	public override void Update()
	{
		if (VFXOwner == null || !VFXOwner.active || VFXOwner.type != ModContent.ProjectileType<HyperockSpearProj>())
		{
			Active = false;
			return;
		}
		Vector2 point = VFXOwner.Center + (OnTile ? Vector2.One.RotatedBy(VFXOwner.rotation - MathF.PI * 0.51) * 6f :
													Vector2.One.RotatedBy(VFXOwner.rotation + MathF.PI * 0.48) * 12.5f);
		HyperockSpearProj HyperockSpearProj = VFXOwner.ModProjectile as HyperockSpearProj;
		if (HyperockSpearProj != null)
		{

			Vector2 pierceAim = -velocity - positiontoProjectile;
			if (pierceAim.Length() < 30)
			{
				timer += 2;
			}
			velocity += Utils.SafeNormalize(pierceAim, Vector2.zeroVector);
			if (HyperockSpearProj.Shot == true)
			{
				timer++;
			}
		}
		oldPos.Enqueue(positiontoProjectile);
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
		positiontoProjectile += velocity;
		float pocession = 1 - timer / maxTime;
		float c = pocession * scale * 0.1f;
		Lighting.AddLight(positiontoProjectile + point, c * 0.5f, c * 0.1f, c * 0.8f);
	}

	public override void Draw()
	{
		int len = oldPos.Count;
		var bars = new List<Vertex2D>();
		Vector2 point = VFXOwner.Center + (OnTile ? Vector2.One.RotatedBy(VFXOwner.rotation - MathF.PI * 0.51) * 6f :
											        Vector2.One.RotatedBy(VFXOwner.rotation + MathF.PI * 0.48) * 12.5f);
		if (len <= 2)
		{
			for (int i = 1; i < 3; i++)
			{
				bars.Add(positiontoProjectile + point, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0 + ai[0], (i + 15 - len) / 17f, 1));
				bars.Add(positiontoProjectile + point, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0.6f + ai[0], (i + 15 - len) / 17f, 1));
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
				bars.Add(pos[i] + point + normal * width, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0 + ai[0], (i + 15 - len) / 17f, pocession));
				bars.Add(pos[i] + point - normal * width, new Color(0.3f + ai[0], 0, 0, 0), new Vector3(0.6f + ai[0], (i + 15 - len) / 17f, pocession));
			}
		}
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}
}