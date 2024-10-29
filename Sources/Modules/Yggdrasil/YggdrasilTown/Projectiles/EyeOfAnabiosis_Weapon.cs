using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis_Weapon : ModProjectile
{
	private const int MaxChargeTime = 720;
	private const int MaxTargetCount = 3;
	private const int SearchDistance = 500;

	private int ChargeTimer { get; set; } = 0;

	public override string Texture => ModAsset.EyeOfAnabiosis_Mod;

	private Player Owner => Main.player[Projectile.owner];

	private Item HeldItem => Main.mouseItem.IsAir ? Owner.HeldItem : Main.mouseItem;

	private Vector2 OwnerMouseWorld
	{
		get => new Vector2(Projectile.ai[0], Projectile.ai[1]);
		set
		{
			Projectile.ai[0] = value.X;
			Projectile.ai[1] = value.Y;
		}
	}

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
		SyncOwnerMouseWorld();
		KillHoldout();
		ManageHoldout();
		HoldoutAI();
	}

	private void SyncOwnerMouseWorld()
	{
		if (Projectile.owner != Main.myPlayer)
		{
			return;
		}

		if (Main.MouseWorld == OwnerMouseWorld)
		{
			return;
		}

		OwnerMouseWorld = Main.MouseWorld;
		Projectile.netUpdate = true;
	}

	private void KillHoldout()
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

	private void ManageHoldout()
	{
		Owner.heldProj = Projectile.whoAmI;
		Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathF.PI + Owner.direction * MathF.PI * 2 / 3);

		Projectile.Center = Owner.MountedCenter;
		Projectile.velocity *= 0;
	}

	private void HoldoutAI()
	{
		if (ChargeTimer++ < MaxChargeTime)
		{
			return;
		}
		else
		{
			if (Main.time % 3 == 0)
			{
				var offset = new Vector2(MathF.Cos((float)Main.time * 3f) * Owner.width / 2, MathF.Sin((float)Main.time * 2f) * Owner.width / 2);
				Dust.NewDust(Owner.Center + offset, 1, 1, DustID.Shadowflame, newColor: new Color(51, 202, 235), Scale: Main.rand.NextFloat(0.8f, 1.1f));
			}
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
				Vector2 projPosition = Projectile.Center + new Vector2(Owner.direction * Projectile.width, Projectile.height / 6);
				if (Owner.direction == -1)
				{
					projPosition.X += Projectile.width * 2 / 3;
				}
				Vector2 projVelocity = Vector2.Normalize(OwnerMouseWorld - projPosition) * HeldItem.shootSpeed;

				List<NPC> targets = SearchTargets();
				foreach (NPC target in targets)
				{
					projVelocity.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
					Projectile.NewProjectile(Owner.GetSource_ItemUse(HeldItem), projPosition, projVelocity, ModContent.ProjectileType<EyeOfAnabiosis_Projectile>(), HeldItem.damage, HeldItem.knockBack, Projectile.owner, target.whoAmI);
				}
				for (int i = 0; i < MaxTargetCount - targets.Count; i++)
				{
					projVelocity.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
					Projectile.NewProjectile(Owner.GetSource_ItemUse(HeldItem), projPosition, projVelocity, ModContent.ProjectileType<EyeOfAnabiosis_Projectile>(), HeldItem.damage, HeldItem.knockBack, Projectile.owner, -1);
				}

				SoundEngine.PlaySound(SoundID.Item91, Projectile.Center);
				Owner.ItemCheck_ApplyManaRegenDelay(HeldItem);
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

		return targets.OrderBy(x => Vector2.Distance(Owner.Center, x.Center)).Take(MaxTargetCount).ToList();
	}

	public override bool PreDraw(ref Color lightColor)
	{
		// Weapon
		var texture = ModContent.Request<Texture2D>(Texture).Value;
		var drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16.0));
		var effects = Owner.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipVertically;
		var rotation = Owner.direction == 1 ? 0f : MathF.PI;
		var position = Projectile.Center - Main.screenPosition + new Vector2(Owner.direction * (texture.Width / 2 - 12), -texture.Height / 2 + 16);
		Main.spriteBatch.Draw(texture, position, null, drawColor, rotation, texture.Size() / 2, 1f, effects, 0);

		// Magic Circle
		var texture2 = ModAsset.EyeOfAnabiosis_MagicCircle.Value;
		var position2 = Owner.Bottom - Main.screenPosition;
		position2 += texture2.Size() / 2;
		Main.spriteBatch.Draw(texture2, position2, null, drawColor, 0, texture.Size() / 2, 1, SpriteEffects.None, -1);

		return false;
	}
}