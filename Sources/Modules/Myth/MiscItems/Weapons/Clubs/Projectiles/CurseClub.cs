using Everglow.Commons.Utilities;
using Everglow.Myth.Common;
using Everglow.Myth.MagicWeaponsReplace.Projectiles.CursedFlames;

namespace Everglow.Myth.MiscItems.Weapons.Clubs.Projectiles;

public class CurseClub : ClubProj
{
	public override void SetDef()
	{
		Beta = 0.005f;
		MaxOmega = 0.45f;
	}
	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
	{
		target.AddBuff(BuffID.CursedInferno, (int)(818 * Omega));
	}
	public override void AI()
	{
		base.AI();

		if (Omega > 0.1f)
		{
			for (float d = 0.1f; d < Omega; d += 0.1f)
			{
				GenerateDust();
				if (Main.rand.NextBool(2))
					GenerateVFX();
			}
			if (FlyClubCooling > 0)
				FlyClubCooling--;
			if (FlyClubCooling <= 0 && Omega > 0.2f)
			{
				FlyClubCooling = 44;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<CurseClub_fly>(), (int)(Projectile.damage * 0.3f), Projectile.knockBack * 0.4f, Projectile.owner);
			}
		}
		else
		{
			GenerateDust();
			if (Main.rand.NextBool(2))
				GenerateVFX();
		}
	}
	private int FlyClubCooling = 0;
	private void GenerateVFX()
	{
		Vector2 v2 = Main.player[Projectile.owner].velocity;
		float mulVelocity = 0.3f + Omega + v2.Length() / 10f;
		var v0 = new Vector2(1, 1);
		v0 *= Main.rand.NextFloat(Main.rand.NextFloat(HitLength * 0.75f, HitLength), HitLength);
		v0.X *= Projectile.spriteDirection;
		if (Main.rand.NextBool(2))
			v0 *= -1;
		v0 = v0.RotatedBy(Projectile.rotation + Main.rand.NextFloat(Omega));
		float Speed = Math.Min(Omega * 0.15f, 0.061f);
		var v1 = new Vector2(-v0.Y * Speed, v0.X * Speed);

		var cf = new CursedFlameDust
		{
			velocity = v1 + v2 * 0.5f,
			Active = true,
			Visible = true,
			position = Projectile.Center + v0,
			maxTime = Main.rand.Next(27, 72),
			ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.01f, 0.01f), Main.rand.NextFloat(3.6f, 10f) * mulVelocity }
		};
		Ins.VFXManager.Add(cf);
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
		Texture2D texture = MythContent.QuickTexture("MiscItems/Weapons/Clubs/Projectiles/CurseClub_glow");
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

		//Effect MeleeTrail = MythContent.QuickEffect("MiscItems/Weapons/Clubs/Projectiles/MetalClubTrail");
		//MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
		Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("MiscItems/Weapons/Clubs/Projectiles/CurseClub_trail");

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
