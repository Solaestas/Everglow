using Everglow.Commons.Utilities;
using Terraria.DataStructures;
using static Terraria.GameContent.Prefixes.PrefixLegacy;

namespace Everglow.Commons.Weapons.Clubs;

/// <summary>
/// Default damage = 5 width*height = 48*48 useT = useA = 4 useStyle = ItemUseStyleID.Shoot rare = ItemRarityID.White value = 50
/// noMelee = true noUseGraphic = true autoReuse = true
/// DamageType = DamageClass.Melee
/// knockBack = 4f shootSpeed = 1f
/// </summary>
public abstract class ClubItem : ModItem
{
	public override void SetStaticDefaults()
	{
	}

	public override void SetDefaults()
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

		SetDef();
		Item.shoot = ProjType;
		ItemSets.SwordsHammersAxesPicks[Type] = true;
	}

	public override bool AllowPrefix(int pre)
	{
		return true;
	}

	public override bool MeleePrefix()
	{
		return true;
	}

	public virtual void SetDef()
	{
	}

	public int ProjType;
	public int ProjTypeSmash;
	public bool CanDown;
	public override bool AltFunctionUse(Player player) => true;
	public override void UpdateInventory(Player player)
	{
		for (int h = 0; h < 7; h++)
		{
			Vector2 pos = player.Center + new Vector2(0, h * 16 * player.gravDir);
			if (TileCollisionUtils.PlatformCollision(pos))
			{
				CanDown = false;
				return;
			}
		}
		for (int h = 7; h < 120; h++)
		{
			Vector2 pos = player.Center + new Vector2(0, h * 16 * player.gravDir);
			if (TileCollisionUtils.PlatformCollision(pos))
			{
				CanDown = true;
				return;
			}
		}
		CanDown = false;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if(player.altFunctionUse != 2)
		{
			if (player.ownedProjectileCounts[type] < 1)
			{
				Projectile.NewProjectile(source, position + velocity * 2f, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
			}
			return false;
		}
		int typeDown = ProjTypeSmash;
		if (typeDown > 0)
		{
			if (CanDown)
			{
				if (player.ownedProjectileCounts[typeDown] < 1 && Main.mouseLeftRelease)
				{
					player.mount.Dismount(player);
					Projectile p = Projectile.NewProjectileDirect(source, position + velocity * 2f, Vector2.Zero, typeDown, (int)(damage * 1.62f), knockback, player.whoAmI, 0f, 0f);
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
					Projectile p = Projectile.NewProjectileDirect(source, position + velocity * 2f, Vector2.Zero, type, damage, knockback, player.whoAmI, 0f, 0f);
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