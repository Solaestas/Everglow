using Everglow.Commons.Templates.Weapons.Clubs;

namespace Everglow.Myth.Misc.Projectiles.Weapon.Melee.Clubs;

public class MeteorClub : ClubProj
{
	public override void SetDef()
	{
		Beta = 0.005f;
		MaxOmega = 0.45f;
		WarpValue = 0.5f;
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		int type = DustID.Flare;
		for (float d = 0.1f; d < Omega; d += 0.04f)
		{
			var D = Dust.NewDustDirect(target.Center - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, type, 0, 0, 150, default, Main.rand.NextFloat(0.4f, 1.1f));
			D.noGravity = true;
			D.velocity = new Vector2(0, Main.rand.NextFloat(Omega * 25f)).RotatedByRandom(6.283);
		}
		target.AddBuff(BuffID.OnFire, 300);
	}
	public override void AI()
	{
		base.AI();
		if (Omega > 0.1f)
		{
		}
	}
	internal float ReflectStrength = 1.2f;
	private void GenerateDust()
	{

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
		var BasePos = Projectile.Center - Main.screenPosition;
		var c0 = new Color(MathF.Sqrt(Omega), Omega * Omega * 0.8f, Omega * Omega * 0.2f, Omega * 0.3f);
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
			float w2 = MathF.Sqrt(TrailAlpha(factor));
			w *= w2;
			bars.Add(new Vertex2D(BasePos + trail[i] * 0.0f * Projectile.scale, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(BasePos + trail[i] * Projectile.scale, c0, new Vector3(factor, 0, w * ReflectStrength)));
		}
		bars.Add(new Vertex2D(BasePos, Color.Transparent, new Vector3(0, 0, 0)));
		bars.Add(new Vertex2D(BasePos, Color.Transparent, new Vector3(0, 0, 0)));
		for (int i = 0; i < length; i++)
		{
			float factor = i / (length - 1f);
			float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
			float w2 = MathF.Sqrt(TrailAlpha(factor));
			w *= w2 * w;
			bars.Add(new Vertex2D(BasePos - trail[i] * 0.0f * Projectile.scale, c0, new Vector3(factor, 1, 0f)));
			bars.Add(new Vertex2D(BasePos - trail[i] * Projectile.scale, c0, new Vector3(factor, 0, w * ReflectStrength)));
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.MeteorClub_trail.Value;

		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
	}
	public override void PostDraw(Color lightColor)
	{
		SpriteEffects effects = SpriteEffects.None;
		if (Projectile.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		Texture2D texture = ModAsset.MeteorClub_glow.Value;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, effects, 0f);
		for (int i = 0; i < 5; i++)
		{
			float fade = Omega * 2f + 0.2f;
			fade *= (5 - i) / 5f;
			var color2 = new Color(fade, fade, fade, 0);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation - i * 0.75f * Omega, texture.Size() / 2f, Projectile.scale, effects, 0f);
		}
	}
}
