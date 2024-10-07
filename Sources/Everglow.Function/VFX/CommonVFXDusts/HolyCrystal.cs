using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.CommonVFXDusts.Misc;
using Terraria.DataStructures;

namespace Everglow.Commons.VFX.CommonVFXDusts;

public class CrystalPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.HolyCrystal;
	}

	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.Parameters["uHeatMap"].SetValue(ModAsset.HeatMap_Crystal.Value);

		effect.Parameters["uNoise"].SetValue(ModAsset.Noise_turtleCrack.Value);
		Ins.Batch.BindTexture<Vertex2D>(ModAsset.Noise_hiveCyber.Value);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
		Ins.Batch.Begin(BlendState.NonPremultiplied, DepthStencilState.None, SamplerState.PointWrap, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}

[Pipeline(typeof(CrystalPipeline))]
public class HolyCrystal : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Vector2 position;
	public Vector2 startPosition;
	public Vector2 velocity;
	public float[] ai;
	public float timer;
	public float maxTime;
	public float scale;

	public override void OnSpawn()
	{
		base.OnSpawn();
	}

	public override void Update()
	{
		float pocession = timer / maxTime;
		Vector2 direction = position - startPosition;
		if (startPosition == Vector2.zeroVector)
		{
			startPosition = position;
			if (ai[2] != 0)
			{
				Projectile projectile = Projectile.NewProjectileDirect(Main.LocalPlayer.GetSource_FromAI(), position, velocity, ModContent.ProjectileType<HolyCrystalProjectile>(), (int)ai[2], 0f, Main.LocalPlayer.whoAmI);
				projectile.timeLeft = (int)maxTime;
			}
		}
		position += velocity;
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
		velocity *= 0.93f;

		timer++;
		if (timer > maxTime)
		{
			Active = false;
		}
		int dustCount = 0;
		for (int i = 0; i < 50; i++)
		{
			Vector2 tryPos = startPosition + direction * Main.rand.NextFloat(1f);
			float try90 = pocession * 1.5f - GetDissolveDeltaValue(tryPos);
			if (Math.Abs(try90 - 0.79) < 0.05)
			{
				for (int j = 0; j < 3; j++)
				{
					if (Main.rand.NextFloat(50) < scale)
					{
						Dust dust = Dust.NewDustDirect(tryPos, 0, 0, ModContent.DustType<CrystalScaleFlame_VanillaDust>());
						dust.velocity = Utils.SafeNormalize(direction, Vector2.One);
						float width = scale;
						if (GetDissolveDeltaValue(tryPos) > 0.2f)
						{
							width *= (0.3f - GetDissolveDeltaValue(tryPos)) * 10f;
						}
						dust.position += Utils.SafeNormalize(direction, Vector2.One).RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-width, width) - new Vector2(4);
						dustCount++;
						dust.scale = Main.rand.NextFloat(0.7f, 2);
						dust.color.G = (byte)Main.rand.Next(255);
					}
				}
			}
			if (Math.Abs(try90 - 0.84) < 0.05)
			{
				if (Main.rand.NextFloat(50) < scale)
				{
					Dust dust2 = Dust.NewDustDirect(tryPos, 0, 0, DustID.MushroomSpray);
					dust2.noGravity = true;
					dust2.position -= new Vector2(4);
					dust2.scale = 0.7f;
					if (GetDissolveDeltaValue(tryPos) > 0.2f)
					{
						dust2.velocity *= (0.3f - GetDissolveDeltaValue(tryPos)) * 10f;
					}
				}
			}
			if (dustCount >= 3)
			{
				break;
			}
		}
		pocession = 1 - pocession;
		float c = pocession * scale * 0.02f;
		Lighting.AddLight(position, c, c * 0.4f, c * 0.7f);
	}

	public override void Draw()
	{
		Vector2 direction = position - startPosition;
		Vector2 normalDirLeft = Utils.SafeNormalize(direction, Vector2.zeroVector).RotatedBy(MathHelper.PiOver2);

		Vector2 point1 = startPosition - normalDirLeft * scale * 0.7f + direction * 0.8f;
		Vector2 point2 = startPosition + normalDirLeft * scale * 0.82f + direction * 0.6f;
		Vector2 point3 = startPosition + normalDirLeft * scale * 0.75f + direction * 0.88f;
		Vector2 point4 = startPosition - normalDirLeft * scale * 0.82f + direction * 0.85f;
		Vector2 point5 = startPosition - normalDirLeft * scale * 0.54f + direction * 0.91f;
		Vector2 point6 = startPosition - normalDirLeft * scale * 0.84f + direction * 0.71f;
		Vector2 point7 = startPosition - normalDirLeft * scale * 0.34f + direction * 0.91f;
		Vector2 point8 = startPosition + normalDirLeft * scale * 0.44f + direction * 0.89f;
		Vector2 point9 = startPosition + normalDirLeft * scale * 0.84f + direction * 0.07f;
		Vector2 point10 = startPosition + normalDirLeft * scale * 0.14f + direction * 0.91f;
		Vector2 point11 = startPosition + normalDirLeft * scale * 0.51f + direction * 0.88f;
		Vector2 point12 = startPosition + normalDirLeft * scale * 0.71f + direction * 0.87f;
		Vector2 point13 = startPosition + normalDirLeft * scale * 0.51f + direction * 0.01f;
		Vector2 point14 = startPosition + normalDirLeft * scale * 0.71f + direction * 0.02f;

		Vector2 point15 = startPosition - normalDirLeft * scale * 0.51f + direction * 0.88f;
		Vector2 point16 = startPosition - normalDirLeft * scale * 0.71f + direction * 0.87f;
		Vector2 point17 = startPosition - normalDirLeft * scale * 0.51f + direction * 0.01f;
		Vector2 point18 = startPosition - normalDirLeft * scale * 0.71f + direction * 0.02f;
		List<Vertex2D> bars = new List<Vertex2D>();

		AddTriangle(position, startPosition + normalDirLeft * scale, startPosition - normalDirLeft * scale, 0, bars);
		AddTriangle(position, point1, startPosition - normalDirLeft * scale, 1, bars);
		AddTriangle(position, point2, startPosition - normalDirLeft * scale, 1.4f, bars);

		AddTriangle(position, point6, startPosition - normalDirLeft * scale, 1.1f, bars);
		AddTriangle(position, point3, startPosition + normalDirLeft * scale, 1.3f, bars);

		AddTriangle(position, point3, point2, 2.5f, bars);
		AddTriangle(position, point4, point5, 2.6f, bars, 0.005f);

		AddTriangle(position, point5, point6, 2.9f, bars, 0.01f);
		AddTriangle(point7, point8, point9, 3.5f, bars, 0.02f);
		if (ai[0] > 30 && ai[0] < 95)
		{
			AddTriangle(point11, point12, point14, 3.1f, bars, 0.52f);
			AddTriangle(point14, point13, point12, 3.1f, bars, 0.52f);
		}
		if (ai[0] > 10 && ai[0] < 45)
		{
			AddTriangle(point15, point16, point18, 3.8f, bars, 0.5f);
			AddTriangle(point18, point17, point16, 3.8f, bars, 0.5f);
		}

		if (ai[0] > 20)
		{
			AddTriangle(position, point10, point3, 3.7f, bars, 0.3f);
			AddTriangle(position, point10, point5, 3.2f, bars, 0.72f);
		}
		else
		{
			AddTriangle(position, point3, point5, 3.5f, bars, 0.3f);
		}

		Ins.Batch.Draw(bars, PrimitiveType.TriangleList);
	}

	public void AddTriangle(Vector2 pos1, Vector2 pos2, Vector2 pos3, float randSeed, List<Vertex2D> bars, float addLight = 0)
	{
		float pocession = timer / maxTime;
		Vector2 direction = position - startPosition;
		Vector2 randomPoint = new Vector2(0, 0.4f).RotatedBy(direction.Length() * 0.0009f + Main.screenPosition.Length() * 0.00002f + ai[0] + randSeed) + new Vector2(ai[1]);
		Color baseColor = Lighting.GetColor((position / 16f).ToPoint());
		Color c1 = baseColor * GetTransparencyValue(pos1);
		Color c2 = baseColor * GetTransparencyValue(pos2);
		Color c3 = baseColor * GetTransparencyValue(pos3);
		c1.R = (byte)(addLight * 255f);
		c2.R = (byte)(addLight * 255f);
		c3.R = (byte)(addLight * 255f);
		bars.Add(pos1, c1, new Vector3(randomPoint, pocession * 1.5f - GetDissolveDeltaValue(pos1)));
		bars.Add(pos2, c2, new Vector3(randomPoint, pocession * 1.5f - GetDissolveDeltaValue(pos2)));
		bars.Add(pos3, c3, new Vector3(randomPoint, pocession * 1.5f - GetDissolveDeltaValue(pos3)));
	}

	public float GetDissolveDeltaValue(Vector2 drawPos)
	{
		Vector2 direction = position - startPosition;
		return (drawPos - startPosition).Length() / direction.Length() * 0.3f;
	}

	public float GetTransparencyValue(Vector2 drawPos)
	{
		Vector2 direction = position - startPosition;
		return (drawPos - startPosition).Length() / direction.Length();
	}
}

public class HolyCrystalProjectile : ModProjectile
{
	public Vector2 StartPos = Vector2.zeroVector;

	public override string Texture => "Everglow/" + ModAsset.Empty_Path;

	public override void OnSpawn(IEntitySource source)
	{
		StartPos = Projectile.Center;
	}

	public override void SetDefaults()
	{
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 60;
		Projectile.width = 20;
		Projectile.height = 20;
	}

	public override void AI()
	{
		Projectile.velocity *= 0.93f;
	}

	public override void OnKill(int timeLeft)
	{
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		float k = 0;
		return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, StartPos, Projectile.scale, ref k);
	}
}