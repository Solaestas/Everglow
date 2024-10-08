using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class FeatheredStaff_staff : ModProjectile
{
	public override string Texture => ModAsset.Auburn_FeatheredStaff_Mod;

	public override void SetDefaults()
	{
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 360000;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
	}

	public override void OnSpawn(IEntitySource source)
	{
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
			Projectile.rotation = (float)(Math.Atan2(MouseToPlayer.Y, MouseToPlayer.X) + Math.PI * 0.25);
			Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer).RotatedBy(Projectile.ai[0] / 0.8d) * 28f + new Vector2(0, 0);
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
				Projectile.Center = player.MountedCenter + Vector2.Normalize(MouseToPlayer).RotatedBy(Projectile.ai[0] / 4d) * 28f;
			}
			else
			{
				Projectile.Kill();
			}
		}
		if (Projectile.Center.X < player.MountedCenter.X)
		{
			player.direction = -1;
		}
		else
		{
			player.direction = 1;
		}
	}

	private void Shoot()
	{
		Player player = Main.player[Projectile.owner];
		int projectileNumber = 3;
		Item item = player.HeldItem;
		for (int i = 1; i < projectileNumber; i++)
		{
			Vector2 shootDirection = Vector2.Normalize(Main.MouseWorld - player.position) * item.shootSpeed;
			Projectile.NewProjectile(
				player.GetSource_ItemUse( item),
				player.Center,
				shootDirection.RotatedByRandom(MathHelper.ToRadians(15)),
				ModContent.ProjectileType<FeatheredStaff>(),
				item.damage,
				item.knockBack,
				player.whoAmI,
				0,
				Main.rand.Next(20));
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public override void PostDraw(Color lightColor)
	{
		if (!release)
		{
			return;
		}

		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		SpriteEffects se = SpriteEffects.None;
		if (player.direction == -1)
		{
			se = SpriteEffects.FlipVertically;
		}

		float rot0 = Projectile.rotation - (float)(Math.PI * 0.25) + MathF.PI * 0.3f * player.direction;
		Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition, null, drawColor, rot0, texMain.Size() / 2f, 1f, se, 0);
	}
}