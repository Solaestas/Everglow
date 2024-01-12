using System.Net;
using Everglow.Commons.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class AmberBall : TrailingProjectile
{
	public override void SetDef()
	{
		TrailColor = new Color(0.8f, 0.6f, 0f, 0);
		TrailTexture = Commons.ModAsset.Trail_3.Value;
		TrailWidth = 50;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 5;
		Projectile.ArmorPenetration = 0;
		base.SetDef();
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if(TimeTokill < 0)
		{
			KillMainStructure();
		}
	}
	public override void AI()
	{
		Timer++;
		if (TimeTokill >= 0 && TimeTokill <= 2)
			Projectile.Kill();
		TimeTokill--;
		if (TimeTokill < 0)
		{
			Projectile.rotation += 0.15f;
		}
		else
		{
			Projectile.velocity *= 0f;
			return;
		}
		Projectile.velocity.Y += 0.1f;
		Projectile.velocity *= 0.99f;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		if(TimeTokill < 0)
		{
			return base.Colliding(projHitbox, targetHitbox);
		}
		else
		{
			if(TimeTokill <  4)
			{
				return Rectangle.Intersect(new Rectangle(projHitbox.Left - 48, projHitbox.Top - 48, 128, 128), targetHitbox) != Rectangle.emptyRectangle;
			}
		}
		return false;
	}
	public override void KillMainStructure()
	{
		Projectile.damage = (int)(Projectile.damage * 5f / 3f);
		Projectile.ArmorPenetration += 4;
		base.KillMainStructure();
	}
	public override bool PreDraw(ref Color lightColor)
	{
		//DrawTrailDark();
		//DrawTrail();
		DrawSelf();
		return false;
	}
	public override void DrawSelf()
	{
		Projectile.friendly = true;
		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		var texMain2 = ModAsset.AmberBall_black.Value;
		if(TimeTokill > 0)
		{
			float scale = 1 - TimeTokill / 30f;
			var texMain3 = ModAsset.AmberBall_white.Value;
			Main.spriteBatch.Draw(texMain2, Projectile.Center - Main.screenPosition - Projectile.velocity, null, Color.White, 0, texMain2.Size() / 2f, 1f + scale * scale * 1f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, new Color(1f, 1f, 1f, 0), 0, texMain.Size() / 2f, 1f + scale * scale * 1f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texMain3, Projectile.Center - Main.screenPosition - Projectile.velocity, null, new Color(scale, scale, scale, scale), 0, texMain3.Size() / 2f, 1f + scale * scale * 1f, SpriteEffects.None, 0);
			return;
		}
		Main.spriteBatch.Draw(texMain2, Projectile.Center - Main.screenPosition - Projectile.velocity, null, Color.White, 0, texMain2.Size() / 2f, 1f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, new Color(1f, 1f, 1f, 0), 0, texMain.Size() / 2f, 1f, SpriteEffects.None, 0);
	}
	public override void OnKill(int timeLeft)
	{
		for(int x = 0;x < 12;x++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 6f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new AmberFlameDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(27, 35),
				scale = Main.rand.NextFloat(5.20f, 16.35f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 }
			};
			Ins.VFXManager.Add(somg);
		}
		for (int x = 0; x < 8; x++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 8f)).RotatedByRandom(MathHelper.TwoPi);
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<YggdrasilAmberFlame>());
			dust.velocity = newVelocity;
			dust.noGravity = true;
		}
		for (int x = 0; x < 12; x++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(0f, 8f)).RotatedByRandom(MathHelper.TwoPi);
			Dust dust = Dust.NewDustDirect(Projectile.Center, 0, 0, ModContent.DustType<YggdrasilAmberMoon>());
			dust.velocity = newVelocity;
			dust.scale = Main.rand.NextFloat(1.2f, 2f);
		}
		SoundEngine.PlaySound(SoundID.NPCDeath1.WithPitchOffset(1), Projectile.Center);
		base.OnKill(timeLeft);
	}
	public override void DrawTrailDark()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

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
			Color drawC = Color.White;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 0, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_3_black.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		if (bars3.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public override void DrawTrail()
	{
		List<Vector2> unSmoothPos = new List<Vector2>();
		for (int i = 0; i < Projectile.oldPos.Length; ++i)
		{
			if (Projectile.oldPos[i] == Vector2.Zero)
				break;
			unSmoothPos.Add(Projectile.oldPos[i]);
		}
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(unSmoothPos);//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (unSmoothPos.Count != 0)
			SmoothTrail.Add(unSmoothPos[unSmoothPos.Count - 1]);

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
			Color drawC = TrailColor;
			if (!SelfLuminous)
			{
				Color lightC = Lighting.GetColor((drawPos / 16f).ToPoint());
				drawC.R = (byte)(lightC.R * drawC.R / 255f);
				drawC.G = (byte)(lightC.G * drawC.G / 255f);
				drawC.B = (byte)(lightC.B * drawC.B / 255f);
			}
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 0, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = TrailTexture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		if (bars3.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
}