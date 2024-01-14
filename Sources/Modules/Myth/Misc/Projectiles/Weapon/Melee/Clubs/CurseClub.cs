using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Myth.Common;
using Everglow.Myth.MagicWeaponsReplace.Projectiles.CursedFlames;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class CurseClub : ClubProj
{
	public override void SetDef()
	{
		Beta = 0.005f;
		MaxOmega = 0.45f;
		vfxTimer = 0;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		target.AddBuff(BuffID.CursedInferno, (int)(818 * Omega));
	}
	float vfxTimer = 0;
	public override void AI()
	{
		base.AI();

		if (Omega > 0.1f)
		{
			for (float d = 0.1f; d < Omega; d += 0.1f)
			{
				GenerateDust();
			}
			if (FlyClubCooling > 0)
				FlyClubCooling--;
			if (FlyClubCooling <= 0 && Omega > 0.2f)
			{
				FlyClubCooling = 44;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CurseClub_fly>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack * 0.4f, Projectile.owner);
			}
		}
		vfxTimer += (Omega * 16);
		if (vfxTimer >= 1)
		{
			GenerateVFX();
			vfxTimer = 0;
		}
	}
	private int FlyClubCooling = 0;
	public void GenerateVFXNew(int Frequency)
	{
		Player player = Main.player[Projectile.owner];
		float mulVelocity = Main.rand.NextFloat(0.75f, 1.5f);
		for (int g = 0; g < Frequency; g++)
		{
			Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(10f)).RotatedByRandom(MathHelper.TwoPi);
			float mulScale = Main.rand.NextFloat(6f, 14f);
			Vector2 startPos = new Vector2(MathF.Sqrt(Main.rand.NextFloat(0.7f, 1f)) * 48f, 0).RotatedBy(Projectile.rotation + MathHelper.PiOver4 * Projectile.spriteDirection);
			if (Main.rand.NextBool(2))
			{
				startPos *= -1;
			}
			var fire = new CurseFlameDust
			{
				velocity = afterVelocity * mulVelocity / mulScale + startPos.RotatedBy(MathHelper.PiOver2) * Omega * 0.4f + player.velocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + startPos,
				maxTime = Main.rand.Next(36, 75),
				scale = Main.rand.NextFloat(10f, 40f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, 1f }
			};
			Ins.VFXManager.Add(fire);
		}
		for (int g = 0; g < Frequency * 2; g++)
		{
			Vector2 newVelocity = new Vector2(0, mulVelocity * Main.rand.NextFloat(0f, 4f)).RotatedByRandom(MathHelper.TwoPi) + Projectile.velocity * 0.2f;
			var spark = new CurseFlameSparkDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Vector2.Normalize(Projectile.velocity) * Main.rand.NextFloat(72f),
				maxTime = Main.rand.Next(37, 145),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 47.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) }
			};
			Ins.VFXManager.Add(spark);
		}
	}
	private void GenerateVFX()
	{
		Vector2 v2 = Main.player[Projectile.owner].velocity;
		float mulVelocity = 0.3f + Omega;
		var v0 = new Vector2(1, 1);
		v0 *= Main.rand.NextFloat(Main.rand.NextFloat(HitLength * 0.75f, HitLength), HitLength);
		v0.X *= Projectile.spriteDirection;
		if (Main.rand.NextBool(2))
			v0 *= -1;
		v0 = v0.RotatedBy(Projectile.rotation + Main.rand.NextFloat(Omega));
		float Speed = Math.Min(Omega * 0.15f, 0.061f) * 2.4f;
		var v1 = new Vector2(-v0.Y * Speed, v0.X * Speed);

		var cf = new CursedFlameDust
		{
			velocity = v1 + v2 * 0.5f,
			Active = true,
			Visible = true,
			position = Projectile.Center + v0,
			maxTime = Main.rand.Next(27, 72),
			ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Omega * 0.19f, Main.rand.NextFloat(3.6f, 30f) * mulVelocity }
		};
		Ins.VFXManager.Add(cf);
		for (int g = 0; g < 4; g++)
		{
			v0 = new Vector2(1, 1);
			v0 *= Main.rand.NextFloat(Main.rand.NextFloat(HitLength * 0.75f, HitLength), HitLength);
			v0.X *= Projectile.spriteDirection;
			if (Main.rand.NextBool(2))
				v0 *= -1;
			v0 = v0.RotatedBy(Projectile.rotation + Main.rand.NextFloat(Omega));
			Speed = Math.Min(Omega * 0.15f, 0.061f) * 2.4f;
			Vector2 newVelocity = new Vector2(-v0.Y * Speed, v0.X * Speed);
			var spark = new CurseFlameSparkDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + v0,
				maxTime = Main.rand.Next(37, 75),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(4f, 27.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) }
			};
			Ins.VFXManager.Add(spark);
		}
	}
	private void GenerateDust()
	{
		var v0 = new Vector2(1, 1);
		v0 *= Main.rand.NextFloat(Main.rand.NextFloat(1, HitLength), HitLength);
		v0.X *= Projectile.spriteDirection;
		if (Main.rand.NextBool(2))
			v0 *= -1;
		v0 = v0.RotatedBy(Projectile.rotation);
		float Speed = Math.Min(Omega * 0.5f, 0.221f);
		var D = Dust.NewDustDirect(Projectile.Center + v0 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.CursedTorch, -v0.Y * Speed, v0.X * Speed, 150, default, Main.rand.NextFloat(0.4f, 1.1f));
		D.noGravity = true;
		D.velocity = new Vector2(-v0.Y * Speed, v0.X * Speed);
	}
	public override void PostDraw(Color lightColor)
	{
		SpriteEffects effects = SpriteEffects.None;
		if (Projectile.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		Texture2D texture = ModAsset.CurseClub_glow.Value;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, effects, 0f);
		for (int i = 0; i < 5; i++)
		{
			float fade = Omega * 2f + 0.2f;
			fade *= (5 - i) / 5f;
			var color2 = new Color(fade, fade, fade, 0);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation - i * 0.75f * Omega, texture.Size() / 2f, Projectile.scale, effects, 0f);
		}
	}
	public override void PostPreDraw()
	{
		List<Vector2> SmoothTrailX = GraphicsUtils.CatmullRom(trailVecs.ToList());//平滑
		var SmoothTrail = new List<Vector2>();
		for (int x = 0; x < SmoothTrailX.Count - 1; x++)
		{
			SmoothTrail.Add(SmoothTrailX[x]);
		}
		if (trailVecs.Count != 0)
			SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);

		int length = SmoothTrail.Count;
		if (length <= 3)
			return;
		Vector2[] trail = SmoothTrail.ToArray();
		var bars = new List<Vertex2D>();

		float fade = Omega * 2f + 0.2f;
		var color2 = new Color(Math.Min(fade * 0.6f, 0.6f), fade, fade * 0.01f, fade);

		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
			float w2 = MathF.Sqrt(TrailAlpha(factor));
			w *= w2 * w;
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.5f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center + trail[i] * 1.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 0, w)));
		}
		bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
		bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
			float w2 = MathF.Sqrt(TrailAlpha(factor));
			w *= w2 * w;
			bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.5f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(Projectile.Center - trail[i] * 1.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 0, w)));
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

		//Effect MeleeTrail = MythContent.QuickEffect("Misc/Projectiles/Weapon/Melee/Clubs/MetalClubTrail");
		//MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.CurseClub_trail.Value;

		//MeleeTrail.Parameters["tex1"].SetValue((Texture2D)ModContent.Request<Texture2D>(Texture));
		//if (ReflectTexturePath != "")
		//{
		//    try
		//    {
		//        MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(ReflectTexturePath).Value);
		//    }
		//    catch
		//    {
		//        MeleeTrail.Parameters["tex1"].SetValue((Texture2D)ModContent.Request<Texture2D>(Texture));
		//    }
		//}
		var lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
		lightColor.W = 0.7f * Omega;
		//MeleeTrail.Parameters["Light"].SetValue(lightColor);
		//MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
}
