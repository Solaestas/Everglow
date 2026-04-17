using Everglow.Myth.LanternMoon.Projectiles.Weapons;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Items.Weapons;

/// <summary>
/// Mark target with a lantern label.
/// Do at least 500 damage to a labeled target will remove the label and trigger an explosion.
/// </summary>
public class KeroseneLanternFlameThrower : ModItem
{
	public override string LocalizationCategory => LocalizationUtils.Categories.RangedWeapons;

	public KeroseneLanternFlameThrower_UI_Bar Visual { get; private set; } = null;

	public float AmmoAmount = 0;

	public float PowerRate = 0.5f;

	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.width = 42;
		Item.height = 42;
		Item.useAnimation = 5;
		Item.useTime = 5;
		Item.knockBack = 1.5f;
		Item.damage = 70;
		Item.rare = ItemRarityID.Lime;
		Item.DamageType = DamageClass.Ranged;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.shootSpeed = 1f;
		Item.shoot = ModContent.ProjectileType<KeroseneLanternFlameThrower_Hold>();
		Item.value = 15000;
		Item.useAmmo = AmmoID.Gel;
	}

	public override void HoldItem(Player player)
	{
		if (Visual is not null && Visual.Active)
		{
			return;
		}
		var helper = new KeroseneLanternFlameThrower_UI_Bar()
		{
			Owner = player,
		};
		Ins.VFXManager.Add(helper);
		Visual = helper;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (Visual is null || !Visual.Active)
		{
			return false;
		}
		if (player.ownedProjectileCounts[type] <= 0 && !Visual.HoverButtom)
		{
			var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			proj.rotation = velocity.ToRotationSafe() + MathHelper.PiOver4;
			if (Visual is not null)
			{
				PowerRate = Visual.ButtomValue;
			}
			else
			{
				PowerRate = 0.5f;
			}
		}
		return false;
	}

	public override bool CanConsumeAmmo(Item ammo, Player player)
	{
		if (Visual is null || !Visual.Active)
		{
			return false;
		}
		if (player.ownedProjectileCounts[Item.shoot] <= 0)
		{
			return false;
		}
		if (AmmoAmount > PowerRate * 5)
		{
			AmmoAmount -= (PowerRate + 0.2f) * 5;
			return false;
		}
		else
		{
			AmmoAmount = 100;
		}
		return true;
	}
}