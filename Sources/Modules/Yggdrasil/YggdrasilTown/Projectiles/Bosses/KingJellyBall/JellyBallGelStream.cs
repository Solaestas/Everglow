using Everglow.Commons.DataStructures;
using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.NPCs;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Bosses.KingJellyBall;

public class JellyBallGelStream : TrailingProjectile
{
	public override void SetDef()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.scale = 1;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 3600;

		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 21;
		TrailColor = new Color(0.1f, 0.3f, 1, 0f);
		TrailWidth = 40f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail_2_thick.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_2_black.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
	}

	public override void AI()
	{
		if (TimeTokill < 0)
		{
			if (Projectile.velocity.Y <= 12)
			{
				Projectile.velocity.Y += 0.2f;
			}
			Projectile.rotation += Projectile.ai[0];
			if (Projectile.timeLeft > 540)
			{
				GenerateSmog(1);
			}
		}
		base.AI();
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.rotation = Main.rand.NextFloat(6.283f);
		Projectile.ai[0] = Main.rand.NextFloat(-0.15f, 0.15f);
	}

	public override void KillMainStructure()
	{
		for (int g = 0; g < 36; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(6, 24)).RotatedByRandom(MathHelper.TwoPi);
			float mulScale = Main.rand.NextFloat(7f, 20f);
			var blood = new JellyBallGelDrop
			{
				velocity = afterVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(42, 84),
				scale = mulScale,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
			};
			Ins.VFXManager.Add(blood);
		}
		NPC.NewNPCDirect(Projectile.GetSource_FromAI(), Projectile.Center - Projectile.oldVelocity, ModContent.NPCType<JellyBall>(), default, 0, 127);
		SoundEngine.PlaySound(SoundID.Item127.WithVolume(1f), Projectile.Center);
		base.KillMainStructure();
	}

	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		if (Projectile.penetrate == 1)
		{
			KillMainStructure();
		}
		int maxTime = 60;
		if (Main.expertMode)
		{
			maxTime = 120;
		}
		if (Main.masterMode)
		{
			maxTime = 150;
		}
		target.AddBuff(ModContent.BuffType<JellyBallStick>(), maxTime);
	}

	public void GenerateSmog(int Frequency)
	{
		for (int g = 0; g < Frequency; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(2, 24)).RotatedByRandom(MathHelper.TwoPi);
			float mulScale = Main.rand.NextFloat(6f, 12f);
			var blood = new JellyBallGelDrop
			{
				velocity = afterVelocity / mulScale,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(42, 84),
				scale = mulScale,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
			};
			Ins.VFXManager.Add(blood);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		float value = (Projectile.timeLeft - 540) / 60f;
		TrailColor = new Color(0.1f, 0.3f, 1, 0f) * value;
		DrawTrailDark();
		DrawTrail();

		if (TimeTokill <= 0)
		{
			Vector2 drawCenter = Projectile.Center - Main.screenPosition;
			var drop = new List<Vertex2D>();
			int step = 30;
			float timeValue = (float)Main.time * 0.03f;
			var drawColor = new Color(0.1f, 0.3f, 1f, 0.6f);
			for (int theta = 0; theta <= step; theta++)
			{
				float a = 14;
				float b = 7 + 0.7f * MathF.Sin(timeValue);
				float rot = theta / (float)step * MathHelper.TwoPi;
				float r = a - b * MathF.Sin(rot);

				r *= Projectile.scale;
				Vector2 toDistance = new Vector2(-r, 0).RotatedBy(rot);
				toDistance.Y *= 1.3f;

				drop.Add(drawCenter + toDistance.RotatedBy(Projectile.velocity.ToRotation() - MathHelper.PiOver2), drawColor, new Vector3(theta / (float)step, 0, 0));
				drop.Add(drawCenter, drawColor, new Vector3(theta / (float)step, 1, 0));
			}
			SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			if (drop.Count > 2)
			{
				Main.graphics.graphicsDevice.Textures[0] = ModAsset.JellyBallGelStream.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, drop.ToArray(), 0, drop.Count - 2);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			var normal = Vector2.Normalize(Projectile.velocity);
			Vector2 normalLeft = Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.PiOver2);
			var dropTrail = new List<Vertex2D>();
			for (int k = 0; k <= 6; k++)
			{
				Color dropColor = drawColor;
				if (k == 0)
				{
					dropColor = Color.Transparent;
				}
				dropTrail.Add(drawCenter + normalLeft * 14 - normal * 7 * (k - 2), dropColor, new Vector3(-k / 10f + timeValue, 0.8f, 1 - k / 6f));
				dropTrail.Add(drawCenter - normalLeft * 14 - normal * 7 * (k - 2), dropColor, new Vector3(-k / 10f + timeValue, 0.2f, 1 - k / 6f));
			}
			Effect effect = ModAsset.JellyBallGelStream_Trail_dissolve.Value;
			var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
			var model = Matrix.CreateTranslation(new Vector3(0)) * Main.GameViewMatrix.TransformationMatrix;
			effect.Parameters["uTransform"].SetValue(model * projection);
			effect.Parameters["uThredshold"].SetValue(0.05f);
			effect.CurrentTechnique.Passes["Test"].Apply();
			if (dropTrail.Count > 2)
			{
				Main.graphics.graphicsDevice.Textures[0] = Commons.ModAsset.Trail_2_thick.Value;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, dropTrail.ToArray(), 0, dropTrail.Count - 2);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(sBS);
		}
		return false;
	}

	public override void DrawTrail()
	{
		var unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> smoothTrail_current = GraphicsUtils.CatmullRom(unSmoothPos);
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < smoothTrail_current.Count - 1; x++)
		{
			SmoothTrail.Add(smoothTrail_current[x]);
		}
		if (unSmoothPos.Count != 0)
		{
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);
		}

		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)-Main.time * 0.014f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			var drawC = new Color(0.1f, 0.3f, 1f, 0f);
			if (!SelfLuminous)
			{
				Color lightC = Lighting.GetColor(drawPos.ToTileCoordinates());
				drawC.R = (byte)(lightC.R * drawC.R / 255f);
				drawC.G = (byte)(lightC.G * drawC.G / 255f);
				drawC.B = (byte)(lightC.B * drawC.B / 255f);
			}

			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth * Projectile.scale, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth * Projectile.scale, drawC, new Vector3(factor + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth * Projectile.scale, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
		}

		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTexture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}

		if (bars2.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		}

		if (bars3.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public override void DrawTrailDark()
	{
		var unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> smoothTrail_current = GraphicsUtils.CatmullRom(unSmoothPos); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < smoothTrail_current.Count - 1; x++)
		{
			SmoothTrail.Add(smoothTrail_current[x]);
		}
		if (unSmoothPos.Count != 0)
		{
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);
		}

		Vector2 halfSize = new Vector2(Projectile.width, Projectile.height) / 2f;
		var bars = new List<Vertex2D>();
		var bars2 = new List<Vertex2D>();
		var bars3 = new List<Vertex2D>();
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)-Main.time * 0.014f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = Color.White * 1f;

			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth * Projectile.scale, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth * Projectile.scale, drawC, new Vector3(factor + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth * Projectile.scale, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
		}

		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTextureBlack;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}

		if (bars2.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		}

		if (bars3.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => base.Colliding(projHitbox, targetHitbox);

	public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
	{
		modifiers.HitDirectionOverride = Projectile.velocity.X > 0 ? 1 : -1;
		base.ModifyHitPlayer(target, ref modifiers);
	}
}