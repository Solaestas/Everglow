using Everglow.Commons.DataStructures;
using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Dusts.TownNPCAttack;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Everglow.Yggdrasil.YggdrasilTown.VFXs.TownNPCAttack;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.TownNPCAttack;

public class Betty_Watermelon : TrailingProjectile
{
	public override void SetDef()
	{
		Projectile.width = 28;
		Projectile.height = 28;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = true;
		Projectile.penetrate = 1;
		Projectile.timeLeft = 600;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
		TrailColor = new Color(0.9f, 0.9f, 0.9f, 0f);
		TrailWidth = 60f;
		SelfLuminous = false;
		TrailTexture = Commons.ModAsset.Trail_10.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_10_black.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
	}

	public override void AI()
	{
		if (TimeTokill < 0)
		{
			if (Projectile.velocity.Y <= 21)
			{
				Projectile.velocity.Y += 0.6f;
			}
			if (Projectile.timeLeft % 6 == 0)
			{
				Projectile.frame++;
				if (Projectile.frame > 11)
				{
					Projectile.frame = 0;
				}
			}
		}
		Timer++;
		Projectile.rotation += Projectile.ai[0];
		if (TimeTokill >= 0 && TimeTokill <= 2)
		{
			Projectile.Kill();
		}
		Projectile.velocity *= 0.99f;
		TimeTokill--;
		if (TimeTokill < 0)
		{
		}
		else
		{
			Projectile.velocity *= 0f;
			return;
		}
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.frame = Main.rand.Next(12);
		Projectile.rotation = Main.rand.NextFloat(6.283f);
		Projectile.ai[0] = Main.rand.NextFloat(-0.15f, 0.15f);
	}

	public override void KillMainStructure()
	{
		for (int x = 0; x < 48; x++)
		{
			var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Betty_Watermelon_Mesocarp_Dust>());
			d.velocity = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(1f)) * 8f).RotatedByRandom(6.283);
			d.scale = Main.rand.NextFloat(0.8f, 1.8f);
			d.velocity -= Projectile.velocity.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat() * 0.6f;
			d.position += d.velocity;
		}
		for (int x = 0; x < 6; x++)
		{
			var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Betty_Watermelon_Skin_Dust>());
			d.velocity = new Vector2(0, MathF.Sqrt(Main.rand.NextFloat(1f)) * 3.6f).RotatedByRandom(6.283);
			d.scale = Main.rand.NextFloat(0.8f, 2f);
			if(d.frame.Y == 0)
			{
				d.scale *= 0.5f;
			}
			d.velocity -= Projectile.velocity.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat() * 0.3f;
			d.position += d.velocity;
		}
		for (int j = 0; j < 4; j++)
		{
			Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0, 6f)).RotatedByRandom(MathHelper.TwoPi);
			int type = ModContent.Find<ModGore>("Everglow/Betty_Watermelon_Gore" + Main.rand.Next(4)).Type;
			Gore gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, v0, type, Projectile.scale);
			gore.velocity *= 0.3f;
			gore.scale = Main.rand.NextFloat(0.8f, 1.2f);
			gore.velocity -= Projectile.velocity.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat() * 0.1f;
		}
		for (int j = 0; j < 4; j++)
		{
			Vector2 v0 = new Vector2(0, Main.rand.NextFloat(0, 6f)).RotatedByRandom(MathHelper.TwoPi);
			int type = ModContent.Find<ModGore>("Everglow/Betty_Watermelon_Gore" + Main.rand.Next(4, 7)).Type;
			Gore gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center, v0, type, Projectile.scale);
			gore.velocity *= 0.3f;
			gore.scale = Main.rand.NextFloat(0.8f, 1.2f);
			gore.velocity -= Projectile.velocity.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat() * 0.1f;
		}
		for (int j = 0; j < 12; j++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 8f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new BettyWatermelonDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(60, 75),
				scale = Main.rand.NextFloat(50f, 155f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
		SoundEngine.PlaySound(SoundID.DD2_MonkStaffGroundImpact, Projectile.Center);
		base.KillMainStructure();
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (Projectile.penetrate == 1)
		{
			KillMainStructure();
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		float value = (Projectile.timeLeft - 540) / 60f;
		TrailColor = new Color(1, 0.3f, 1, 0f) * value * Projectile.ai[2];
		DrawTrailDark();
		DrawTrail();
		if (TimeTokill <= 0)
		{
			Texture2D texture = ModAsset.Betty_Watermelon.Value;
			Vector2 drawCenter = Projectile.Center - Main.screenPosition;
			lightColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
			Rectangle frame = new Rectangle(0, 48 * Projectile.frame, 48, 48);
			Main.EntitySpriteDraw(texture, drawCenter, frame, lightColor, Projectile.rotation, frame.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
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
		for (int i = SmoothTrail.Count - 1; i > 0; --i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)Main.time * 0.06f;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = new Color(0.02f, 0.67f, 0.45f, 0) * 0.3f;
			Color lightC = Lighting.GetColor((drawPos / 16f).ToPoint());
			drawC.R = (byte)(lightC.R * drawC.R / 255f);
			drawC.G = (byte)(lightC.G * drawC.G / 255f);
			drawC.B = (byte)(lightC.B * drawC.B / 255f);
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 0, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
		}
		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
		for (int i = SmoothTrail.Count - 1; i > 0; --i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)Main.time * 0.06f;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = Color.White * 0.4f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 0, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
		}
		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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
}