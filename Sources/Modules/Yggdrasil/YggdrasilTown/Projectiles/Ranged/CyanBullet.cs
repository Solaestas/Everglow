using Everglow.Commons.Templates.Weapons;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Ranged;

public class CyanBullet : TrailingProjectile
{
	public override void SetCustomDefaults()
	{
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.penetrate = 6;
		Projectile.timeLeft = 3600;
		TrailTexture = Commons.ModAsset.Trail.Value;
		TrailTextureBlack = Commons.ModAsset.Trail_black.Value;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
		TrailColor = new Color(0.3f, 0.7f, 0.8f, 0f);
		TrailWidth = 6f;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.damage += 2;
		base.OnSpawn(source);
	}

	public override void AI()
	{
		if (TimeAfterEntityDestroy < 0)
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.3f, 0.7f, 0.8f));
		}
		else
		{
			Lighting.AddLight(Projectile.Center, new Vector3(0.3f, 0.7f, 0.8f) * TimeAfterEntityDestroy / 7f);
		}
		if (Projectile.timeLeft == 3540)
		{
			Projectile.damage -= 2;
		}
		base.AI();
	}

	public override void DrawSelf()
	{
		Texture2D light_dark = Commons.ModAsset.StarSlash_black.Value;
		Main.spriteBatch.Draw(light_dark, Projectile.Center - Main.screenPosition, null, new Color(0.6f, 0.6f, 0.6f, 0.6f), Projectile.velocity.ToRotationSafe() + MathHelper.PiOver2, light_dark.Size() * 0.5f, 0.7f, SpriteEffects.None, 0);
		Texture2D light = Commons.ModAsset.StarSlash.Value;
		Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0.1f, 0.7f, 0.6f, 0f), Projectile.velocity.ToRotationSafe() + MathHelper.PiOver2, light.Size() * 0.5f, 0.7f, SpriteEffects.None, 0);

		Texture2D ball = Commons.ModAsset.TileBlock.Value;
		Main.spriteBatch.Draw(ball, Projectile.Center - Main.screenPosition, null, new Color(0.1f, 0.7f, 0.4f, 0.5f), (float)Main.time * 0.03f + Projectile.whoAmI, ball.Size() * 0.5f, 0.4f, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(ball, Projectile.Center - Main.screenPosition, null, new Color(0.1f, 0.7f, 0.4f, 0.5f), (float)Main.time * 0.03f + Projectile.whoAmI + MathHelper.PiOver4, ball.Size() * 0.5f, 0.4f, SpriteEffects.None, 0);
		return;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		base.OnHitNPC(target, hit, damageDone);
	}

	public override void DestroyEntity()
	{
		SoundEngine.PlaySound(SoundID.NPCHit4.WithVolumeScale(0.8f), Projectile.Center);
		for (int i = 0; i < 5; i++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2.0f, 24f)).RotatedByRandom(MathHelper.TwoPi);
			var spark = new Spark_MoonBladeDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(3, 7),
				scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(16f, 27.0f)),
				rotation = Main.rand.NextFloat(6.283f),
				noGravity = true,
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.03f, 0.03f) },
			};
			Ins.VFXManager.Add(spark);
		}
		base.DestroyEntity();
	}
}