using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Everglow.Commons.Templates.Weapons;
using Everglow.Yggdrasil.KelpCurtain.VFXs;
using Terraria;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Magic;

public class GreenSungloSpore : TrailingProjectile
{
	public override void SetCustomDefaults()
	{
		TrailColor = new Color(0.475f, 1f, 0.475f, 0f);
		TrailWidth = 25f;
		TrailTexture = Commons.ModAsset.Trail_2.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_2_black.Value;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;

		Projectile.width = 20;
		Projectile.height = 20;

		Projectile.friendly = false;
		Projectile.hostile = false;
	}

	public override void AI()
	{
		Projectile.velocity.Y += 0.25f;
		base.AI();
	}

	public override void OnKill(int timeLeft)
	{
		int tileX = ((int)Projectile.Center.X) / 16;
		int tileY = ((int)Projectile.Center.Y) / 16;
		do
		{
			tileY -= 3;
		}
		while (WorldGen.SolidTile2(tileX, tileY) || WorldGen.SolidTile2(tileX, tileY + 1) || WorldGen.SolidTile2(tileX, tileY + 2));

		for (; tileY < Main.maxTilesY - 10
			&& (Main.tile[tileX, tileY + 3] == null || Main.tile[tileX - 1, tileY + 3] == null || Main.tile[tileX - 1, tileY + 3] == null
			|| !WorldGen.SolidTile2(tileX, tileY + 3) || !WorldGen.SolidTile2(tileX - 1, tileY + 3) || !WorldGen.SolidTile2(tileX - 1, tileY + 3)); tileY++)
		{
		}

		Vector2 pos = new Vector2(tileX * 16, tileY * 16 + 16);
		Projectile.NewProjectileDirect(null, pos, Vector2.Zero, ModContent.ProjectileType<GreenSungloThorns>(), 0, 0, Projectile.owner);
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		base.ModifyHitNPC(target, ref modifiers);
		modifiers.FinalDamage *= 0;
		modifiers.HideCombatText();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		if (TimeAfterEntityDestroy <= 0)
		{
			var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, lightColor, Projectile.velocity.ToRotation() + MathHelper.PiOver2 * 0.5f, texMain.Size() / 2f, 0.8f, SpriteEffects.None, 0);
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
			float timeValue = (float)Main.time * 0.075f;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			Color drawC = TrailColor;
			factor *= 1.5f;
			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 0, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(-factor * 2 + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(-factor * 2 + timeValue, 0.5f, width)));
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
}