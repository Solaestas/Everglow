using System;
using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;
using Terraria;
using Terraria.Audio;

namespace Everglow.Myth.TheFirefly.Projectiles;

internal class GlowBeadGun : ModProjectile
{
	public override string Texture => "Everglow/Myth/TheFirefly/Projectiles/GlowBeadGunTex/GlowBeadGun";

	public override void SetDefaults()
	{
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.timeLeft = 100000;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
	}

	public override Color? GetAlpha(Color lightColor)
	{
		return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
	}

	private Vector2 oldPo = Vector2.Zero;
	private int Energy = 0;

	public override bool PreAI()
	{
		if(Projectile.timeLeft <= 10)
		{
			Player player = Main.player[Projectile.owner];
			if(Projectile.timeLeft > 3)
			{
				Energy = Math.Min(Energy += 20, 180);
			}
			else
			{
				Energy = 0;
			}
			Projectile.rotation -= player.direction * Projectile.timeLeft * Projectile.ai[1] / 72000f;
			Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
			v0 = Vector2.Normalize(v0);
			Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0).RotatedBy(Projectile.ai[0] / 0.8d) * (8f - Projectile.ai[0] * 8);
			return false;
		}
		return true;
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		if (Projectile.timeLeft % 5 == 0 && Energy < 180)
			player.statMana--;
		Energy = Math.Min(++Energy, 180);
		Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
		v0 = Vector2.Normalize(v0);
		if (player.controlUseItem)
		{
			Projectile.ai[0] *= 0.9f;
			Projectile.ai[1] -= 1f;
			Projectile.rotation = (float)(Math.Atan2(v0.Y, v0.X) + Math.PI * 0.25);
			Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0).RotatedBy(Projectile.ai[0] / 0.8d) * (8f - Projectile.ai[0] * 8);
			oldPo = Projectile.Center;
			Projectile.Center = oldPo;
			Projectile.velocity *= 0;
		}
		if (!player.controlUseItem)
		{
			if (Projectile.ai[1] > 0)
			{
				Projectile.ai[0] *= 0.9f;
				Projectile.ai[1] -= 1f;
				Projectile.Center = Main.player[Projectile.owner].MountedCenter + Vector2.Normalize(v0).RotatedBy(Projectile.ai[0] / 4d) * (8f - Projectile.ai[0] * 4);
			}
			else
			{
				Shoot();			
				return;
			}
		}
		if (player.statMana <= 0 || Projectile.timeLeft < 2)
			Shoot();
	}

	private void Shoot()
	{
		Vector2 offset = new Vector2(0, -15f);
		SoundEngine.PlaySound(SoundID.Item72, Projectile.Center);
		Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
		v0 = Vector2.Normalize(v0);
		Player player = Main.player[Projectile.owner];
		ScreenShaker Gsplayer = player.GetModPlayer<ScreenShaker>();
		Gsplayer.FlyCamPosition = new Vector2(0, 2 * Energy).RotatedByRandom(6.283);
		int NumProjectiles = (int)(Energy / 20f) + 1;
		for (int i = 0; i < NumProjectiles; i++)
		{
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + v0 * 62 + offset, v0 * (12 - i) * (Energy + 20) / 240f, ModContent.ProjectileType<GlowStar>(), (int)(Projectile.damage / (i + 19f) * 19f * (Energy + 120) / 180f), Projectile.knockBack, player.whoAmI, i * (Energy + 20) / 180f, 0);
		}

		Vector2 newVelocity = v0;
		newVelocity *= 1f - Main.rand.NextFloat(0.3f);
		newVelocity *= Math.Clamp(Energy / 18f, 0.2f, 10f);
		Vector2 basePos = Projectile.Center + newVelocity * 3.7f + v0 * 62;

		for (int j = 0; j < Energy; j++)
		{
			Vector2 v = newVelocity / 27f * j;
			Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
			int num20 = Dust.NewDust(basePos + offset, 0, 0, ModContent.DustType<BlueGlowAppearStoppedByTile>(), v1.X, v1.Y, 100, default, Main.rand.NextFloat(0.6f, 1.8f) * 0.4f);
			Main.dust[num20].noGravity = true;
		}
		for (int j = 0; j < Energy; j++)
		{
			Vector2 v = newVelocity / 54f * j;
			Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
			float Scale = Main.rand.NextFloat(3.7f, 5.1f);
			int num21 = Dust.NewDust(basePos + offset + new Vector2(4, 4.5f), 0, 0, ModContent.DustType<BlueParticleDark2StoppedByTile>(), v1.X, v1.Y, 100, default, Scale);
			Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
		}
		for (int j = 0; j < 16; j++)
		{
			Vector2 v = newVelocity / 54f * j;
			Vector2 v1 = new Vector2(Main.rand.NextFloat(0, 6f), 0).RotatedByRandom(6.283) * 0.3f + v;
			v1 *= 0.2f;
			float Scale = Main.rand.NextFloat(3.7f, 5.1f);
			int num21 = Dust.NewDust(basePos + offset + new Vector2(4, 4.5f), 0, 0, ModContent.DustType<MothSmog>(), v1.X, v1.Y, 100, default, Scale);
			Main.dust[num21].alpha = (int)(Main.dust[num21].scale * 50);
		}
		player.velocity -= newVelocity * 0.1f;
		Projectile.ai[1] = Energy;
		Projectile.timeLeft = 10;
		Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + v0 * 52f + offset, Vector2.Zero, ModContent.ProjectileType<GlowBeadGunShootFlame>(), 0, 0, player.whoAmI, Energy / 450f + 0.1f, Projectile.rotation - MathF.PI / 4);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}
	public override void PostDraw(Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		Vector2 v0 = Projectile.Center - player.MountedCenter;
		if (Main.mouseLeft)
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(v0.Y, v0.X) - Math.PI / 2d));
		Texture2D TexMain = ModAsset.GlowBeadGunOff.Value;
		Texture2D TexMainG = ModAsset.GlowBeadGunGlow.Value;
		Projectile.frame = (int)(Energy % 45 / 5f);

		Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		SpriteEffects se = SpriteEffects.None;
		if (Projectile.Center.X < player.Center.X)
		{
			se = SpriteEffects.FlipVertically;
			player.direction = -1;
		}
		else
		{
			player.direction = 1;
		}
		Main.spriteBatch.Draw(TexMain, Projectile.Center - Main.screenPosition - new Vector2(0, 6), null, drawColor, Projectile.rotation - (float)(Math.PI * 0.25) + (float)(Projectile.ai[0] / -3d) * player.direction, new Vector2(35, 22), 1f, se, 0);
		Main.spriteBatch.Draw(TexMainG, Projectile.Center - Main.screenPosition - new Vector2(0, 6), new Rectangle(0, 0, (int)(Energy / 180f * 74f) + 30, TexMainG.Height), new Color(255, 255, 255, 0), Projectile.rotation - (float)(Math.PI * 0.25) + (float)(Projectile.ai[0] / -3d) * player.direction, new Vector2(35, 22), 1f, se, 0);
	}
}