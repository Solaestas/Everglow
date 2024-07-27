using Everglow.Commons.DataStructures;
using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class BrittleRockSlingshotStone : TrailingProjectile
{
	public override void SetDef()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.scale = 0.75f;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 600;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 21;
		TrailColor = new Color(1, 0.3f, 1, 0f);
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
				float value = (Projectile.timeLeft - 540) / 30f;
				Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(12 * Projectile.scale), (int)(Projectile.width + 24 * Projectile.scale), (int)(Projectile.height + 24 * Projectile.scale), ModContent.DustType<RockElemental_Energy_normal>());
				d.velocity = Projectile.velocity * 0.5f;
				d.scale = Main.rand.NextFloat(0.75f, 1f) * Math.Min(value, 1) * Projectile.ai[2];
				d.noGravity = true;
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
		for (int x = 0; x < 3; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, ModContent.DustType<RockElemental_fragments>(), 0f, 0f, 0, default, 0.7f);
			d.velocity = new Vector2(0, Main.rand.NextFloat(0.1f, 1.6f)).RotatedByRandom(6.283);
		}
		for (int x = 0; x < 5; x++)
		{
			Dust d = Dust.NewDustDirect(Projectile.position - new Vector2(4), Projectile.width, Projectile.height, DustID.WitherLightning, 0f, 0f, 0, default, Main.rand.NextFloat(0.4f, 0.6f));
			d.velocity = new Vector2(0, Main.rand.NextFloat(0.7f, 4.1f)).RotatedByRandom(6.283);
		}
		GenerateSmog(3);

		if (Projectile.ai[1] != 1)
		{
			int n = Main.rand.Next(3, 4);
			for (int i = 0; i < n; i++)
			{
				Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.TwoPi) * 0.4f, Type, Projectile.damage / n, Projectile.knockBack / n, Projectile.owner, 0, 1, Projectile.ai[2] * 0.8f);
				p.scale = Projectile.scale / MathF.Sqrt(n);
				p.penetrate = 1;
				p.ai[1] = 1;
			}
			if (Projectile.ai[2] >= 0.8f)
			{
				Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<BrittleRockSlingshotStone_Explosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 1);
			}
			SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
		}
		else
		{
			SoundEngine.PlaySound(SoundID.NPCHit2.WithVolume(Projectile.ai[2]), Projectile.Center);
		}
		base.KillMainStructure();
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (Projectile.penetrate == 1)
		{
			KillMainStructure();
		}
	}

	public void GenerateSmog(int Frequency)
	{
		for (int g = 0; g < Frequency; g++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new RockSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(37, 45),
				scale = Main.rand.NextFloat(10f, 25f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		float value = (Projectile.timeLeft - 540) / 60f;
		TrailColor = new Color(1, 0.3f, 1, 0f) * value * Projectile.ai[2];
		DrawTrail();
		if (TimeTokill <= 0)
		{
			Texture2D texture = ModAsset.RockElemental_Stonefragment.Value;
			Vector2 drawCenter = Projectile.Center - Main.screenPosition;
			lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
			if (Projectile.timeLeft > 540)
			{
				for (int i = 0; i < 7; i++)
				{
					Main.EntitySpriteDraw(texture, drawCenter, null, new Color(0.7f, 0.4f, 1f, 0f) * value * 0.4f * Projectile.ai[2], Projectile.rotation + i, texture.Size() * 0.5f, Projectile.scale * (1.15f + value * 0.3f), SpriteEffects.None, 0);
				}
			}
			Main.EntitySpriteDraw(texture, drawCenter, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
		}
		return false;
	}

	public override void DrawTrail()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
			{
				break;
			}

			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos); // 平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
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
			Color drawC = TrailColor;
			if (!SelfLuminous)
			{
				Color lightC = Lighting.GetColor((drawPos / 16f).ToPoint());
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

		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
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

	public override void DrawTrailDark() => base.DrawTrailDark();

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => base.Colliding(projHitbox, targetHitbox);
}