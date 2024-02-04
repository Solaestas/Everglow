using Everglow.Commons.DataStructures;
using Everglow.Commons.Weapons;
using Everglow.Myth.LanternMoon.Gores;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.LanternKing;

public class ExplosiveLantern : TrailingProjectile
{
	public override void SetDef()
	{
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.hostile = true;
		Projectile.friendly = false;
		Projectile.ignoreWater = true;
		Projectile.tileCollide = false;
		Projectile.extraUpdates = 3;
		Projectile.timeLeft = 3600;
		Projectile.alpha = 0;
		Projectile.penetrate = -1;
		Projectile.scale = 1f;

		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Projectile.type] = true;
		TrailColor = new Color(1f, 0.7f, 0f, 0f);
		TrailWidth = 40f;
		SelfLuminous = true;
		TrailTexture = Commons.ModAsset.Trail_1.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		TrailShader = ModAsset.TrailingDissolve.Value;
	}
	public override Color? GetAlpha(Color lightColor)
	{
		return new Color?(new Color(1f, 1f, 1f, 0.5f));
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo info)
	{
		base.OnHitPlayer(target, info);
	}
	public override void OnSpawn(IEntitySource source)
	{
	}
	public override void AI()
	{
		base.AI();
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return base.PreDraw(ref lightColor);
	}
	public override void DrawSelf()
	{
		var texture2D = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Projectile.frameCounter += 1;
		if (Projectile.frameCounter == 8)
		{
			Projectile.frameCounter = 0;
			Projectile.frame += 1;
		}
		if (Projectile.frame > 3)
			Projectile.frame = 0;
		float timeValue = (float)Main.time * 0.03f;
		var colorT = new Color(1f *  (float)(Math.Sin(timeValue) + 2) / 3f, 1f *  (float)(Math.Sin(timeValue) + 2) / 3f, 1f *  (float)(Math.Sin(timeValue) + 2) / 3f, 0.5f *  (float)(Math.Sin(timeValue) + 2) / 3f);

		Main.spriteBatch.Draw(texture2D, Projectile.Center - Main.screenPosition, null, colorT, Projectile.rotation, texture2D.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 1f);
		Main.spriteBatch.Draw(ModAsset.LanternFire.Value, Projectile.Center - Main.screenPosition, new Rectangle?(new Rectangle(0, 30 * Projectile.frame, 20, 30)), colorT, 0, new Vector2(10, 15), Projectile.scale * 0.5f, SpriteEffects.None, 1f);
	}
	public override void DrawTrail()
	{
		float velocityValue = Math.Clamp(1.2f - Projectile.velocity.Length() / 4f, 0f, 1.2f);
		TrailWidth = 40;
		if (Projectile.velocity.Length() < 2f)
		{
			TrailWidth *= Math.Max(Projectile.velocity.Length() * 2 - 3f, 0);
		}
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
			float timeValue = (float)Main.time * 0.0005f + Projectile.whoAmI / 15f;
			factor += timeValue;

			Vector2 drawPos = SmoothTrail[i] + halfSize;
			float value = i / (float)SmoothTrail.Count;
			Color drawC = new Color(value, value * value * 0.7f, 0f, 0f);

			bars.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 2f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars2.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 1f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars2.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
			bars3.Add(new Vertex2D(drawPos + new Vector2(0, 1).RotatedBy(MathHelper.TwoPi * 0f / 3f) * TrailWidth, drawC, new Vector3(factor + timeValue, 1, width)));
			bars3.Add(new Vertex2D(drawPos, drawC, new Vector3(factor + timeValue, 0.5f, width)));
		}
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = TrailShader;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.Parameters["duration"].SetValue(velocityValue - 0.2f);
		effect.CurrentTechnique.Passes["Test"].Apply();
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
		Main.spriteBatch.Begin(sBS);
	}
	public override void KillMainStructure()
	{
		var gore2 = new FloatLanternGore3
		{
			Active = true,
			Visible = true,
			velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
			noGravity = false,
			position = Projectile.Center
		};
		Ins.VFXManager.Add(gore2);
		var gore3 = new FloatLanternGore4
		{
			Active = true,
			Visible = true,
			velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
			noGravity = false,
			position = Projectile.Center
		};
		Ins.VFXManager.Add(gore3);
		var gore4 = new FloatLanternGore5
		{
			Active = true,
			Visible = true,
			velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
			noGravity = false,
			position = Projectile.Center
		};
		Ins.VFXManager.Add(gore4);
		var gore5 = new FloatLanternGore6
		{
			Active = true,
			Visible = true,
			velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
			noGravity = false,
			position = Projectile.Center
		};
		Ins.VFXManager.Add(gore5);
		ScreenShaker Gsplayer = Main.player[Projectile.owner].GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 33).RotatedByRandom(6.283);
		var p = Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center, Vector2.zeroVector, ModContent.ProjectileType<DarkLanternBombExplosion>(), Projectile.damage, Projectile.knockBack);
		p.friendly = Projectile.friendly;
		p.hostile = Projectile.hostile;
		base.KillMainStructure();
	}
	public override void DrawTrailDark()
	{
		base.DrawTrailDark();
	}
}