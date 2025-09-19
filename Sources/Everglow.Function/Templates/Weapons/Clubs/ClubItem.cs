using Everglow.Commons.Utilities;
using Terraria.DataStructures;
using static Terraria.GameContent.Prefixes.PrefixLegacy;

namespace Everglow.Commons.Templates.Weapons.Clubs;

/// <summary>
/// A template for club-type weapons.
/// </summary>
public abstract class ClubItem : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.MeleeWeapons;

	public int ProjType { get; set; } = default!;

	public int ProjSmashType { get; set; } = default!;

	public override void SetStaticDefaults()
	{
		ItemSets.SwordsHammersAxesPicks[Type] = true;
	}

	public sealed override void SetDefaults()
	{
		Item.damage = 5;
		Item.width = 48;
		Item.height = 48;
		Item.useTime = 4;
		Item.value = 50;
		Item.useTime = 8;
		Item.useAnimation = 8;

		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.shootSpeed = 1f;
		Item.knockBack = 16f;

		Item.DamageType = DamageClass.Melee;
		Item.useStyle = ItemUseStyleID.Thrust;
		Item.rare = ItemRarityID.White;

		SetCustomDefaults();
		Item.shoot = ProjType;
	}

	public virtual void SetCustomDefaults()
	{
	}

	public override bool AllowPrefix(int pre) => true;

	public override bool MeleePrefix() => true;

	public override bool AltFunctionUse(Player player) => CanDown(player);

	private static bool CanDown(Player player)
	{
		for (int h = 0; h < 7; h++)
		{
			Vector2 pos = player.Center + new Vector2(0, h * 16 * player.gravDir);
			Point bottomPos = pos.ToTileCoordinates();
			bottomPos.X = Math.Clamp(bottomPos.X, 20, Main.maxTilesX - 20);
			bottomPos.Y = Math.Clamp(bottomPos.Y, 20, Main.maxTilesY - 20);
			if (TileCollisionUtils.PlatformCollision(pos) || ((player.waterWalk || player.waterWalk2) && Main.tile[bottomPos].LiquidAmount > 0 && !player.wet))
			{
				return false;
			}
		}
		for (int h = 7; h < 120; h++)
		{
			Vector2 pos = player.Center + new Vector2(0, h * 16 * player.gravDir);
			Point bottomPos = pos.ToTileCoordinates();
			bottomPos.X = Math.Clamp(bottomPos.X, 20, Main.maxTilesX - 20);
			bottomPos.Y = Math.Clamp(bottomPos.Y, 20, Main.maxTilesY - 20);
			if (TileCollisionUtils.PlatformCollision(pos) || ((player.waterWalk || player.waterWalk2) && Main.tile[bottomPos].LiquidAmount > 0 && !player.wet))
			{
				return true;
			}
		}
		return false;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.altFunctionUse != 2)
		{
			if (player.ownedProjectileCounts[type] < 1)
			{
				Projectile.NewProjectile(source, position + velocity * 2f, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
			}
			return false;
		}
		int typeDown = ProjSmashType;
		if (typeDown > 0)
		{
			if (CanDown(player))
			{
				if (player.ownedProjectileCounts[typeDown] < 1 && Main.mouseLeftRelease)
				{
					player.mount.Dismount(player);
					var p = Projectile.NewProjectileDirect(source, position + velocity * 2f, Vector2.Zero, typeDown, (int)(damage * 1.62f), knockback, player.whoAmI, 0f, 0f);
					p.scale = Item.scale;

					// 查重
					if (player.ownedProjectileCounts[type] >= 1)
					{
						foreach (Projectile proj in Main.projectile)
						{
							if (proj != null && proj.active)
							{
								if (proj.owner == player.whoAmI && proj.type == type)
								{
									proj.Kill();
								}
							}
						}
					}
				}
			}
			else
			{
				if (player.ownedProjectileCounts[type] + player.ownedProjectileCounts[typeDown] < 1)
				{
					var p = Projectile.NewProjectileDirect(source, position + velocity * 2f, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
					p.scale = Item.scale;
				}
			}
		}
		else
		{
			if (player.ownedProjectileCounts[type] < 1)
			{
				Projectile.NewProjectile(source, position + velocity * 2f, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
			}
		}
		return false;
	}
}