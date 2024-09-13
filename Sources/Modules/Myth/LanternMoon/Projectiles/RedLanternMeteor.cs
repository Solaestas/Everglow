using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons;

namespace Everglow.Myth.LanternMoon.Projectiles;

class RedLanternMeteor : TrailingProjectile
{
	public override void SetDef()
	{
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
		TrailColor = new Color(1, 0.05f, 0, 0f);
		TrailWidth = 2f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		TrailShader = Commons.ModAsset.Trailing.Value;
		Projectile.timeLeft = 300;
		Projectile.hostile = true;
	}
	public override void AI()
	{
		Projectile.velocity.Y -= 0.5f;
		base.AI();
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}
	public override void DrawSelf()
	{
		Texture2D star = Commons.ModAsset.StarSlash.Value;
		float width = 0.5f;
		if (Projectile.timeLeft < 200)
		{
			width = 2f;
		}
		if (Projectile.timeLeft < 150)
		{
			width *= Projectile.timeLeft / 150f;
		}
		width *= 1 + (MathF.Sin((float)Main.time * 0.23f + Projectile.whoAmI) + 0.5f) * 0.7f;
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.75f, 0, 0), MathHelper.PiOver2, star.Size() / 2f, width / 4.5f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.75f, 0, 0), 0, star.Size() / 2f, new Vector2(3f, width / 4.5f), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.75f, 0, 0), MathHelper.PiOver2 + (float)Main.timeForVisualEffects * 0.04f, star.Size() / 2f, width / 10f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(star, Projectile.Center - Main.screenPosition, null, new Color(1f, 0.75f, 0, 0), (float)Main.timeForVisualEffects * 0.04f, star.Size() / 2f, width / 10f, SpriteEffects.None, 0);
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
			float timeValue = (float)Main.time * 0.0005f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = TrailColor;
			drawC *= (1 - i / (float)SmoothTrail.Count);
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
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
	public override void DrawTrailDark()
	{
		base.DrawTrailDark();
	}
	public override void KillMainStructure()
	{
		for (int x = 0; x < 25; x++)
		{
			var spark = new RayDustDust
			{
				velocity = new Vector2(0, Main.rand.NextFloat(2, 6f)).RotateRandom(MathHelper.TwoPi),
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(57, 255),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(8f, 17.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0 }
			};
			Ins.VFXManager.Add(spark);
		}
		base.KillMainStructure();
	}
}

