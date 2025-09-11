using Everglow.Commons.Templates.Weapons;
using Everglow.Myth.TheTusk.Projectiles.Weapon;
using Terraria.Audio;

namespace Everglow.Myth.TheTusk.Projectiles;

public class TuskCurse : TrailingProjectile
{
	public override void SetDefaults()
	{
		base.SetDefaults();
	}

	public override void SetCustomDefaults()
	{
		TrailColor = new Color(1, 0, 0, 0f);
		TrailTexture = Commons.ModAsset.Trail_2_thick.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_2_black_thick.Value;
		base.SetCustomDefaults();
	}

	public override void AI()
	{
		base.AI();
		Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		Projectile.velocity *= 0.98f;
		Projectile.velocity.Y += 0.4f;
	}

	public override void OnKill(int timeLeft)
	{
	}

	public override void DestroyEntity()
	{
		SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt, Projectile.Center);
		for (int h = 0; h < 20; h++)
		{
			Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
			Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, DustID.VampireHeal, 0, 0, 0, default, 15f * Main.rand.NextFloat(0.4f, 1.1f));
			dust.noGravity = true;
			dust.velocity = v3;
		}
		Projectile.NewProjectile(Terraria.Entity.InheritSource(Projectile), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TuskBloodPool>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
		base.DestroyEntity();
	}

	public override void DrawSelf()
	{
		base.DrawSelf();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		if (TimeAfterEntityDestroy <= 0)
		{
			var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - Projectile.velocity, null, lightColor, Projectile.rotation, texMain.Size() / 2f, 1f, SpriteEffects.None, 0);
		}
		return false;
	}
}