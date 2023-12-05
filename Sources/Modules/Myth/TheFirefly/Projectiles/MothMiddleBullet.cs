using Everglow.Commons.Weapons;
using Everglow.Myth.TheFirefly.VFXs;
using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.Projectiles;

public class MothMiddleBullet : TrailingProjectile
{
	public override void SetDef()
	{
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.timeLeft = 1600;
		Projectile.tileCollide = true;
		TrailColor = new Color(0.1f, 0.4f, 0.9f, 0);
		TrailTexture = Commons.ModAsset.Trail_6.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_0_black.Value;
		base.SetDef();
	}
	public override void OnSpawn(IEntitySource source)
	{
		

		base.OnSpawn(source);
	}
	public override void AI()
	{
		Projectile.rotation = Projectile.velocity.ToRotation();

		Timer++;
		Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		if (TimeTokill >= 0 && TimeTokill <= 2)
			Projectile.Kill();
		TimeTokill--;
		if (TimeTokill < 0)
		{
			if (Projectile.timeLeft > 300 && Projectile.timeLeft < 1500)
			{
				Player player = Main.player[Player.FindClosest(Projectile.Center, 0, 0)];
				Vector2 toPlayer = player.Center - Projectile.Center - Projectile.velocity;
				if (toPlayer.Length() > 200)
				{
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Normalize(toPlayer) * 8f, 0.005f + 0.0025f * MathF.Sin((float)Main.time * 0.015f + Projectile.whoAmI) + Projectile.ai[0]);
				}
			}
			if(Projectile.timeLeft < 200)
			{
				KillMainStructure();
			}
			GenerateFire(1);
			GenerateSpark(2);
		}
		else
		{
			Projectile.velocity *= 0f;
			return;
		}
	}
	public void GenerateFire(int Frequency)
	{
		float mulVelocity = Projectile.ai[0] / 5f;
		for (int g = 0; g < Frequency; g++)
		{
			float sqrtRand = MathF.Pow(Main.rand.NextFloat(1), 0.4f);
			Vector2 newVelocity = new Vector2(0, mulVelocity * sqrtRand * 0.6f).RotatedByRandom(MathHelper.TwoPi);
			var fire = new MothBlueFireDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + newVelocity,
				maxTime = Main.rand.Next(19, 35),
				scale = Main.rand.NextFloat(0.2f, 0.5f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, 0 }
			};
			Ins.VFXManager.Add(fire);
		}
	}
	public void GenerateSpark(int Frequency)
	{
		float mulVelocity = Projectile.ai[0] / 5f;
		for (int g = 0; g < Frequency; g++)
		{
			float sqrtRand = MathF.Pow(Main.rand.NextFloat(1), 0.4f);
			Vector2 newVelocity = new Vector2(0, mulVelocity * sqrtRand * 0.9f).RotatedByRandom(MathHelper.TwoPi);
			var smog = new MothShimmerScaleDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				coord = new Vector2(Main.rand.NextFloat(1f), Main.rand.NextFloat(1f)),
				maxTime = Main.rand.Next(10, 95),
				scale = Main.rand.NextFloat(0.4f, 2.4f),
				rotation = Main.rand.NextFloat(6.283f),
				rotation2 = Main.rand.NextFloat(6.283f),
				omega = Main.rand.NextFloat(-30f, 30f),
				phi = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(-0.005f, 0.005f), 0, 0 }
			};
			Ins.VFXManager.Add(smog);
		}
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
			float timeValue = (float)Main.time * 0.03f;

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
		Main.graphics.GraphicsDevice.Textures[0] = TrailTextureBlack;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
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
		for (int i = 1; i < SmoothTrail.Count; ++i)
		{
			float mulFac = Timer / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
			if (mulFac > 1f)
			{
				mulFac = 1f;
			}
			float factor = i / (float)SmoothTrail.Count * mulFac;
			float width = TrailWidthFunction(factor);
			float timeValue = (float)Main.time * 0.005f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = Color.Lerp(TrailColor, new Color(0.4f, 0f, 1f, 0), i / (float)SmoothTrail.Count);

			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor * 2 + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor * 2 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor * 2 + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor * 2 + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor * 2 + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor * 2 + timeValue, 0.5f, width)));
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
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		if (bars2.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars2.ToArray(), 0, bars2.Count - 2);
		if (bars3.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars3.ToArray(), 0, bars3.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		DrawTrailDark();
		if (TimeTokill <= 0)
		{
			var texDark = ModAsset.MothMiddleBullet_dark.Value;
			var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
			var texStar = Commons.ModAsset.StarSlash.Value;
			Main.spriteBatch.Draw(texDark, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, texDark.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition, null, new Color(1f, 1f, 1f, 0), Projectile.rotation, texMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);

			Vector2 addVel = Vector2.Normalize(Projectile.velocity) * 10;
			Main.spriteBatch.Draw(texStar, Projectile.Center - Main.screenPosition + addVel, null, TrailColor, 0, texStar.Size() / 2f, 0.5f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texStar, Projectile.Center - Main.screenPosition + addVel, null, TrailColor, MathHelper.PiOver2, texStar.Size() / 2f, new Vector2(0.5f, 1f), SpriteEffects.None, 0);
		}
		return false;
	}
	public override void Explosion()
	{
		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<MothBulletExplosion>(), 50, 3, Projectile.owner, 10f);
		base.Explosion();
	}
}