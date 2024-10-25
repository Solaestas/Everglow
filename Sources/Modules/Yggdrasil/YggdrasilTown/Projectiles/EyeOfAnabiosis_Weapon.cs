using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis_Weapon : ModProjectile
{
	private const int MaxChargeTime = 720;
	private const int SearchDistance = 500;

	private int ChargeTimer { get; set; } = 0;

	public override string Texture => ModAsset.EyeOfAnabiosis_Mod;

	private Player Owner => Main.player[Projectile.owner];

	private Item HeldItem => Main.mouseItem.IsAir ? Owner.HeldItem : Main.mouseItem;

	public override void SetDefaults()
	{
		Projectile.width = 72;
		Projectile.height = 66;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 2;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Magic;
		Projectile.penetrate = -1;
		Projectile.ignoreWater = true;
	}

	public override void AI()
	{
		KillHoldout();
		ManageHoldout();
		HoldoutAI();
	}

	public void KillHoldout()
	{
		bool canUseHoldout =
			Owner == null
			|| !Owner.active
			|| Owner.dead
			|| Owner.CCed
			|| Owner.noItems;
		if (canUseHoldout)
		{
			Projectile.Kill();
			return;
		}

		bool manaOK = Owner.CheckMana(HeldItem);
		if (!manaOK)
		{
			Projectile.Kill();
			return;
		}

		if (HeldItem.type == ModContent.ItemType<EyeOfAnabiosis>())
		{
			Projectile.timeLeft = 2;
		}
		else
		{
			Projectile.Kill();
			return;
		}
	}

	public void ManageHoldout()
	{
		Owner.heldProj = Projectile.whoAmI;
		Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI + Owner.direction * MathF.PI * 2 / 3);

		Projectile.Center = Owner.MountedCenter;
		Projectile.velocity *= 0;
	}

	public void HoldoutAI()
	{
		if (ChargeTimer++ < MaxChargeTime)
		{
			Console.WriteLine($"Charging: {ChargeTimer} / {MaxChargeTime}");
			return;
		}

		if (!Owner.controlUseItem)
		{
			return;
		}

		if (Owner.itemTime == 0)
		{
			bool manaCostPaid = Owner.CheckMana(HeldItem, pay: true);
			if (manaCostPaid)
			{
				SoundEngine.PlaySound(SoundID.Item91, Projectile.Center);

				Vector2 projPosition = Projectile.Center + new Vector2(Owner.direction * Projectile.width, Projectile.height / 6);
				if (Owner.direction == -1)
				{
					projPosition.X += Projectile.width / 2;
				}
				Vector2 projVelocity;
				int type = ModContent.ProjectileType<EyeOfAnabiosis_Projectile>();

				List<NPC> targets = SearchTargets();
				if (targets.Count > 0)
				{
					foreach (NPC target in targets)
					{
						projVelocity = Vector2.Normalize(target.Center - projPosition) * HeldItem.shootSpeed;
						Projectile.NewProjectile(Owner.GetSource_ItemUse(HeldItem), projPosition, projVelocity, type, HeldItem.damage, HeldItem.knockBack, Projectile.owner, target.whoAmI);
					}
				}
				else
				{
					projVelocity = Vector2.Normalize(Main.MouseWorld - projPosition) * HeldItem.shootSpeed;
					Projectile.NewProjectile(Owner.GetSource_ItemUse(HeldItem), projPosition, projVelocity, type, HeldItem.damage, HeldItem.knockBack, Projectile.owner, -1);
				}

				Owner.ItemCheck_ApplyManaRegenDelay(HeldItem);
				Owner.itemTime = Owner.itemTimeMax;
				Owner.itemTime = HeldItem.useTime;
			}
		}

		Owner.direction = (Main.MouseWorld - Owner.MountedCenter).X < 0 ? -1 : 1;
	}

	private List<NPC> SearchTargets()
	{
		List<NPC> targets = [];

		foreach (NPC npc in Main.ActiveNPCs)
		{
			if (!npc.friendly
				&& !npc.dontTakeDamage
				&& npc.CanBeChasedBy()
				&& Vector2.Distance(Owner.Center, npc.Center) <= SearchDistance)
			{
				targets.Add(npc);
			}
		}

		return targets.OrderBy(x => Vector2.Distance(Owner.Center, x.Center)).Take(3).ToList();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		// Weapon
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
		var drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		var effects = Owner.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		var rotation = Owner.direction == 1 ? 0f : MathF.PI;
		var position = Projectile.Center - Main.screenPosition + new Vector2(Owner.direction * (texture.Width / 2 - 12), -texture.Height / 2 + 16);
		Main.spriteBatch.Draw(texture, position, null, drawColor, rotation, texture.Size() / 2, 1f, effects, 0);

		// Magic Circle
		var texture2 = ModAsset.YggdrasilAmberLaser_crystal.Value;
		var position2 = Owner.Bottom - Main.screenPosition;
		var circleScale = ChargeTimer < MaxChargeTime ? new Vector2(0.5f, 0.2f) : new Vector2(0.5f, 0.5f);
		Main.spriteBatch.Draw(texture2, position2, null, drawColor, 0, texture.Size() / 2, circleScale, SpriteEffects.None, 0);

		return false;
	}
}