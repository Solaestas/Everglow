using Everglow.Myth.LanternMoon.Projectiles.Weapons;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Items.Weapons;

/// <summary>
/// Mark target with a lantern label.
/// Do at least 500 damage to a labeled target will remove the label and trigger an explosion.
/// </summary>
public class GildingRevolver : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.RangedWeapons;

	public int ShootType = -1;

	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.width = 42;
		Item.height = 42;
		Item.useAnimation = 5;
		Item.useTime = 5;
		Item.knockBack = 1.5f;
		Item.damage = 99;
		Item.rare = ItemRarityID.Lime;
		Item.DamageType = DamageClass.Ranged;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.useAmmo = AmmoID.Bullet;
		Item.shootSpeed = 27;
		Item.shoot = ProjectileID.Bullet;
		Item.value = 15000;
		Item.crit = 14;
	}

	public override void HoldItem(Player player)
	{
		if (player.ownedProjectileCounts[ModContent.ProjectileType<GildingRevolver_Proj>()] <= 0)
		{
			Projectile.NewProjectile(Item.GetSource_FromAI(), player.Center, Vector2.zeroVector, ModContent.ProjectileType<GildingRevolver_Proj>(), 0, 0, player.whoAmI);
		}
	}

	public override bool AltFunctionUse(Player player)
	{
		return true;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		ShootType = type;
		return false;
	}

	public override bool CanConsumeAmmo(Item ammo, Player player)
	{
		foreach (var proj in Main.projectile)
		{
			if (proj is not null && proj.active)
			{
				if (proj.type == ModContent.ProjectileType<GildingRevolver_Proj>() && proj.owner == player.whoAmI)
				{
					var gProj = proj.ModProjectile as GildingRevolver_Proj;
					if (gProj.UsedBulletsCount > 0)
					{
						gProj.UsedBulletsCount--;
						return true;
					}
					break;
				}
			}
		}
		return false;
	}
}