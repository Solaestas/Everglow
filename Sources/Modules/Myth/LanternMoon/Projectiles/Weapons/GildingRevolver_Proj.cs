using Everglow.Commons.Templates.Weapons;
using Everglow.Myth.LanternMoon.Items;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.Weapons;

public class GildingRevolver_Proj : HandholdProjectile
{
	public int Timer = 0;

	public override void SetDef()
	{
		Projectile.width = 64;
		Projectile.height = 64;
		Projectile.friendly = false;
		TextureRotation = 0;
		MaxRotationSpeed = 0.05f;
		DepartLength = 20;
		DrawOffset = new Vector2(0, 0);
	}

	public override void OnSpawn(IEntitySource source)
	{
	}

	public override void AI()
	{
		Timer++;
		base.AI();
	}

	public override void HeldProjectileAI()
	{
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		ArmRootPos = player.MountedCenter + new Vector2(-4 * player.direction, -2);
		player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.rotation - MathF.PI * 0.25f) * player.gravDir - MathF.PI * 0.5f);
		int dir = 1;
		if (Main.MouseWorld.X < player.MountedCenter.X)
		{
			dir = -1;
		}
		Vector2 mouseToPlayer = Main.MouseWorld - ArmRootPos;
		mouseToPlayer = Vector2.Normalize(mouseToPlayer);
		float powerRate = 1;
		if (player.HeldItem is not null && player.HeldItem.type == ModContent.ItemType<KeroseneLanternFlameThrower>())
		{
			KeroseneLanternFlameThrower kLFT = player.HeldItem.ModItem as KeroseneLanternFlameThrower;
			powerRate = (kLFT.PowerRate - 0.5f) * 2f;
		}
		if (player.controlUseItem)
		{
			if (player.itemTime == 0)
			{
				Vector2 vel = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * player.HeldItem.shootSpeed;
				Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + vel.NormalizeSafe() * 60 + DrawOffset, vel, ModContent.ProjectileType<KeroseneLanternFlameThrower_Shoot>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}
		player.direction = dir;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawBaseTexture(lightColor);
		return false;
	}

	public override void DrawBaseTexture(Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		var texMain = ModAsset.KeroseneLanternFlameThrower_Hold.Value;
		SpriteEffects se = SpriteEffects.None;
		if (player.direction == -1)
		{
			se = SpriteEffects.FlipVertically;
		}

		float rot = Projectile.rotation - (float)(Math.PI * 0.25) + TextureRotation * player.direction;
		Vector2 drawCenter = Projectile.Center - Main.screenPosition + DrawOffset;

		Main.spriteBatch.Draw(texMain, drawCenter, null, lightColor, rot, texMain.Size() * 0.5f, 1f, se, 0);
	}
}