using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class CyanVineStaff_proj : ModProjectile
{
	public override string Texture => "Everglow/Yggdrasil/YggdrasilTown/Projectiles/CyanVineStaff_proj";

	public override void SetDefaults()
	{
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 360000;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
	}

	public override Color? GetAlpha(Color lightColor)
	{
		return new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
	}

	private bool release = true;
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathF.PI * 0.75f);

		Vector2 MouseToPlayer = Main.MouseWorld - player.MountedCenter;
		MouseToPlayer = Vector2.Normalize(MouseToPlayer);
		if (player.controlUseItem && release)
		{
			Projectile.ai[0] *= 0.9f;
			Projectile.ai[1] -= 1f;
			float useDuration = (player.itemTime - 5) / (float)player.itemTimeMax;
			useDuration *= useDuration;
			Projectile.rotation = (float)(Math.Atan2(MouseToPlayer.Y, MouseToPlayer.X) + Math.PI * 0.25) - useDuration * player.direction * 0.6f;
			Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer).RotatedBy(Projectile.ai[0] / 0.8d) * (8f - Projectile.ai[0] * 8) + new Vector2(0, 0);
			Projectile.velocity *= 0;
			if (player.itemTime == player.itemTimeMax - 1)
			{
				Shoot();
			}
			if (player.itemTime == 0)
			{
				if (player.ItemCheck_PayMana(player.HeldItem, true))
				{
					player.ItemCheck_ApplyManaRegenDelay(player.HeldItem);
					player.itemTime = player.itemTimeMax;
				}
				else
				{
					Projectile.Kill();
				}
			}
		}
		if (!player.controlUseItem && release)
		{
			if (Projectile.ai[1] > 0)
			{
				Projectile.ai[0] *= 0.9f;
				Projectile.ai[1] -= 1f;
				Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer).RotatedBy(Projectile.ai[0] / 4d) * (8f - Projectile.ai[0] * 4);
			}
			else
			{
				Projectile.Kill();
			}
		}
		if (Projectile.Center.X < player.MountedCenter.X)
			player.direction = -1;
		else
		{
			player.direction = 1;
		}
	}
	private void Shoot()
	{
		SoundEngine.PlaySound(SoundID.Item72.WithVolumeScale(0.8f), Projectile.Center);
		Vector2 v0 = Main.MouseWorld - Main.player[Projectile.owner].MountedCenter;
		v0 = Vector2.Normalize(v0);
		Player player = Main.player[Projectile.owner];

		Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + v0 * 36, v0 * player.HeldItem.shootSpeed * 0.5f, ModContent.ProjectileType<CyanVineStaff_proj_shoot>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 20);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override void PostDraw(Color lightColor)
	{
		if (!release)
			return;
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		SpriteEffects se = SpriteEffects.None;
		if (player.direction == -1)
			se = SpriteEffects.FlipVertically;
		float rot0 = Projectile.rotation - (float)(Math.PI * 0.25) + MathF.PI * 0.3f * player.direction;
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition - new Vector2(0, 6), null, drawColor, rot0, texMain.Size() / 2f, 1f, se, 0);
	}
}