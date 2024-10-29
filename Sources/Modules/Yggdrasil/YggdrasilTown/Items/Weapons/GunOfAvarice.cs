using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class GunOfAvarice : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 62;
		Item.height = 32;
		Item.scale = 0.75f;

		Item.DamageType = DamageClass.Ranged;
		Item.damage = 16;
		Item.knockBack = 1.5f;
		Item.crit = 2;
		Item.noMelee = true;

		Item.useTime = 27;
		Item.useAnimation = 27;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.autoReuse = false;

		Item.rare = ItemRarityID.Orange;
		Item.value = Item.sellPrice(gold: 10);

		Item.shoot = ModContent.ProjectileType<GunOfAvariceBullet>();
		Item.shootSpeed = 12;
	}

	private const int MagazineCapacity = 14;

	public const int AutoReloadDuration = 90;
	public const float AutoReloadSuccessChance = 0.8f;
	public const float AutoReloadFailureDamageRatio = 2.8f;

	public const int ManualReloadDuration = 30;

	public const float DamageBonusPerLevel = 0.25f;
	public const int MaxLevel = 10;

	public const int TargetValueBonusInCopper = 75;

	private int AmmoAmount { get; set; } = MagazineCapacity;

	private int Level { get; set; } = 1;

	public override bool CanUseItem(Player player) =>
		player.ownedProjectileCounts[ModContent.ProjectileType<GunOfAvariceAutoReload>()] <= 0 &&
		player.ownedProjectileCounts[ModContent.ProjectileType<GunOfAvariceManualReload>()] <= 0;

	// TODO: Keep holding weapon when it's selected
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.altFunctionUse != 2)
		{
			SoundEngine.PlaySound(SoundID.Item41);
			AmmoAmount--;
			Projectile.NewProjectile(source, position, velocity, type, (int)(damage * (1 + DamageBonusPerLevel * Level)), knockback);

			if (AmmoAmount <= 0)
			{
				int result = 0;
				if (Main.rand.NextFloat() >= AutoReloadSuccessChance)
				{
					result = 1;
					player.Hurt(PlayerDeathReason.ByCustomReason($"{player.name} died in explosion!"), (int)(Item.damage * 2.8f), 0, false, false, 0);
				}
				else
				{
					Level = Level + 1 > MaxLevel ? MaxLevel : Level + 1;
				}

				Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<GunOfAvariceAutoReload>(), 0, 0, ai0: result);
				AmmoAmount = MagazineCapacity;
			}
		}
		else if (AmmoAmount < MagazineCapacity)
		{
			Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<GunOfAvariceManualReload>(), 0, 0);
			AmmoAmount = MagazineCapacity;
			Level = 1;
		}
		return false;
	}

	public override bool AltFunctionUse(Player player) => true;

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		tooltips.Add(new TooltipLine(Mod, "GunOfAvariceAmmo", $"Current Ammo: {AmmoAmount}/{MagazineCapacity}"));
		tooltips.Add(new TooltipLine(Mod, "GunOfAvariceLevel", $"Current Ammo: {Level}"));
	}
}