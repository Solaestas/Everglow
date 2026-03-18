using Everglow.Commons.Templates.Weapons;
using Everglow.Myth.LanternMoon.Items;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Projectiles.Weapons;

public class GildingRevolver_Proj : HandholdProjectile
{
	public int Timer = 0;

	public float NormalBulletTimer = 0;

	public int ReloadCooling = 0;

	public int NormalBulletsCount;

	public int LanternBulletCount;

	public int UsedBulletsCount = 0;

	public int LanternBulletTimer = 0;

	public override void SetDef()
	{
		Projectile.width = 64;
		Projectile.height = 64;
		Projectile.friendly = false;
		TextureRotation = 0;
		MaxRotationSpeed = 0.05f;
		DepartLength = 20;
		DrawOffset = new Vector2(0, -4);
		ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
	}

	public override void AI()
	{
		if (ReloadCooling <= 0)
		{
			if (NormalBulletTimer < 120)
			{
				NormalBulletTimer += 3;
			}
		}
		else
		{
			ReloadCooling--;
		}
		Timer++;
		base.AI();
	}

	public override void HeldProjectileAI()
	{
		Player player = Main.player[Projectile.owner];
		player.heldProj = Projectile.whoAmI;
		int dir = 1;
		if (Main.MouseWorld.X < player.MountedCenter.X)
		{
			dir = -1;
		}
		Vector2 mouseToPlayer = Main.MouseWorld - ArmRootPos;
		mouseToPlayer = Vector2.Normalize(mouseToPlayer);
		Projectile.rotation = mouseToPlayer.ToRotation() + MathHelper.PiOver4;
		Projectile.Center = player.MountedCenter + mouseToPlayer * 12;
		Item item = player.HeldItem;
		if (item is not null)
		{
			GildingRevolver gildingRevolver = item.ModItem as GildingRevolver;
			if (gildingRevolver is not null)
			{
				Projectile.timeLeft = 2;
			}
		}
		if (player.controlUseItem)
		{
			ReloadCooling = 30;
			Projectile.hide = false;
			ArmRootPos = player.MountedCenter + new Vector2(-4 * player.direction, -2);
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.rotation - MathF.PI * 0.25f) * player.gravDir - MathF.PI * 0.5f);
			if (player.itemTime == player.itemTimeMax && NormalBulletTimer >= 20)
			{
				Shoot();
				NormalBulletTimer -= 20;
			}
		}
		else if (player.controlUseTile)
		{
			Projectile.hide = false;
			if(LanternBulletCount == 0)
			{
				LanternBulletTimer++;
				if(LanternBulletTimer > 30)
				{
					LanternBulletCount = 1;
					LanternBulletTimer = 0;
				}
			}
		}
		else
		{
			Projectile.hide = true;
		}
		player.direction = dir;
	}

	private void Shoot()
	{
		Player player = Main.player[Projectile.owner];
		Item item = player.HeldItem;
		if (item is null)
		{
			return;
		}
		GildingRevolver gildingRevolver = item.ModItem as GildingRevolver;
		if (gildingRevolver is not null && gildingRevolver.ShootType >= 0)
		{
			Vector2 vel = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * player.HeldItem.shootSpeed;
			Vector2 addPos = new Vector2(0, -2).RotatedBy(Projectile.rotation - MathHelper.PiOver4);
			Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + vel.NormalizeSafe() * -2 + DrawOffset, vel, gildingRevolver.ShootType, item.damage, item.knockBack, Projectile.owner);
			UsedBulletsCount++;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawBaseTexture(lightColor);
		return false;
	}

	public override void DrawBaseTexture(Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		if (!Projectile.hide)
		{
			var texMain = ModAsset.GildingRevolver_Proj.Value;
			SpriteEffects se = SpriteEffects.None;
			if (player.direction == -1)
			{
				se = SpriteEffects.FlipVertically;
			}
			float rot = Projectile.rotation - (float)(Math.PI * 0.25) + TextureRotation * player.direction;
			Vector2 drawCenter = Projectile.Center - Main.screenPosition + DrawOffset;
			Main.spriteBatch.Draw(texMain, drawCenter, null, lightColor, rot, texMain.Size() * 0.5f, 1f, se, 0);
		}

		var textureUI = ModAsset.GildingRevolver_Proj_Bullets.Value;
		Rectangle darkFrame = new Rectangle(0, 0, 92, 30);
		Main.spriteBatch.Draw(textureUI, player.Center - Main.screenPosition + new Vector2(0, -50), darkFrame, Color.White, 0, darkFrame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		for (int k = 0; k < 6; k++)
		{
			if (NormalBulletTimer < (k + 1) * 20 - 5)
			{
				break;
			}
			Rectangle normalFrame = new Rectangle(0, 32, 10, 26);
			if (NormalBulletTimer < (k + 1) * 20)
			{
				normalFrame = new Rectangle(40, 32, 10, 26);
			}
			Main.spriteBatch.Draw(textureUI, player.Center - Main.screenPosition + new Vector2(k * 12 - 41, -50), normalFrame, Color.White, 0, normalFrame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		}
	}
}