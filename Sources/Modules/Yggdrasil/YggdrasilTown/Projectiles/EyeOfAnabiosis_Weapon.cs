using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis_Weapon : ModProjectile
{
	public override string Texture => ModAsset.EyeOfAnabiosis_Mod;

	private Player Owner => Main.player[Projectile.owner];

	public override void SetDefaults()
	{
		Projectile.width = 36;
		Projectile.height = 36;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 2;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
	}

	public override void AI()
	{
		if (Owner.HeldItem.type == ModContent.ItemType<EyeOfAnabiosis>())
		{
			Projectile.timeLeft = 2;
		}
		else
		{
			Projectile.Kill();
			return;
		}

		if (!Owner.active)
		{
			Projectile.Kill();
			return;
		}

		Owner.heldProj = Projectile.whoAmI;
		Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI + Owner.direction * MathF.PI * 2 / 3);

		Projectile.Center = Owner.MountedCenter;
		Projectile.velocity *= 0;

		if (Owner.controlUseItem)
		{
			// TODO: after ItemCheck_ApplyManaRegenDelay is called, player could regen
			// mana immediately, and the Owner.itemTimeMax is 0, need to be fixed
			if (Owner.itemTime == 0 && Owner.ItemCheck_PayMana(Owner.HeldItem, true))
			{
				Item item = Owner.HeldItem;
				Vector2 shootVelocity = Vector2.Normalize(Main.MouseWorld - Owner.position) * item.shootSpeed;
				Projectile.NewProjectile(
					Owner.GetSource_ItemUse(item),
					Owner.Center,
					shootVelocity,
					ModContent.ProjectileType<EyeOfAnabiosis_Projectile>(),
					item.damage,
					item.knockBack,
					Owner.whoAmI,
					0,
					Main.rand.Next(20));

				Owner.ItemCheck_ApplyManaRegenDelay(Owner.HeldItem);
				Owner.itemTime = Owner.itemTimeMax;

				Owner.itemTime = Owner.HeldItem.useTime;
			}

			Owner.direction = (Main.MouseWorld - Owner.MountedCenter).X < 0 ? -1 : 1;
		}
	}

	public override bool PreDraw(ref Color lightColor) => false;

	public override void PostDraw(Color lightColor)
	{
		Owner.heldProj = Projectile.whoAmI;

		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
		var drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		var effects = Owner.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		var rotation = Owner.direction == 1 ? 0f : MathF.PI;
		var position = Owner.Center - Main.screenPosition + new Vector2(Owner.direction * (texture.Width / 2 - 12), -texture.Height / 2 + 16);
		Main.spriteBatch.Draw(texture, position, null, drawColor, rotation, texture.Size() / 2, 1f, effects, 0);

		var texture2 = ModAsset.YggdrasilAmberLaser_crystal.Value;
		var position2 = Owner.Bottom - Main.screenPosition;
		Main.spriteBatch.Draw(texture2, position2, null, Color.White, 0, texture.Size() / 2, 0.5f, SpriteEffects.None, 0);
	}
}