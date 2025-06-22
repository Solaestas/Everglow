using Everglow.Commons.DataStructures;
using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Summon;

public class WoodlandWraithStaff_BloodStream : TrailingProjectile
{
	public const float GlowTime = 90;

	public override void SetDef()
	{
		TrailColor = new Color(0.4f, 0.0f, 0.0f, 0f);
		TrailWidth = 12f;
		TrailTexture = Commons.ModAsset.Trail_8.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_8_black.Value;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;

		Projectile.width = 14;
		Projectile.height = 30;
	}

	public override void AI()
	{
		base.AI();
		if (TimeTokill < 0)
		{
			Projectile.velocity.Y += 0.5f;
		}
	}

	public override void KillMainStructure()
	{
		for (int k = 0;k < 6;k++)
		{
			float mulScale = Main.rand.NextFloat(6f, 20f);
			var blood = new BloodDrop
			{
				velocity = new Vector2(0, Main.rand.NextFloat(3, 6)).RotatedByRandom(MathHelper.TwoPi),
				Active = true,
				Visible = true,
				position = Projectile.Center,
				maxTime = Main.rand.Next(82, 164),
				scale = mulScale,
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
			};
			Ins.VFXManager.Add(blood);
		}
		base.KillMainStructure();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrailDark();
		DrawTrail();
		if (TimeTokill <= 0)
		{
			DrawSelf();
		}
		return false;
	}

	public override void DrawSelf()
	{
		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		var color = Color.Lerp(Color.White, Lighting.GetColor(Projectile.Center.ToTileCoordinates()), MathF.Sqrt(Timer / GlowTime));
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, color, Projectile.velocity.ToRotation() - MathHelper.PiOver2, texMain.Size() / 2f, 0.6f, SpriteEffects.None, 0);
	}

	public override void DrawTrail() => base.DrawTrail();

	public override void DrawTrailDark()
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
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
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
}