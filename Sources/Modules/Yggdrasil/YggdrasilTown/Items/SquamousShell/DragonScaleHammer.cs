using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.SquamousShell;

public class DragonScaleHammer : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 68;
		Item.height = 72;
		Item.useAnimation = 5;
		Item.useTime = 5;
		Item.shoot = ModContent.ProjectileType<DragonScaleHammerProj>();
		Item.shootSpeed = 5f;
		Item.knockBack = 15f;
		Item.damage = 30;
		Item.rare = ItemRarityID.Green;

		Item.DamageType = DamageClass.Melee;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.value = 2400;
	}

	public override bool CanUseItem(Player player)
	{
		if (base.CanUseItem(player))
		{
			if (Main.myPlayer == player.whoAmI)
			{
				if (player.altFunctionUse != 2)
				{
					Item.useAnimation = 5;
					Item.useTime = 5;
					Item.noMelee = true;
					Item.noUseGraphic = true;
					Item.autoReuse = false;
					Item.hammer = 0;
					var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<DragonScaleHammerProj>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
					proj.netUpdate2 = true;
				}
				else//右键
				{
					//Item.shoot = -1;
					//Item.shootSpeed = 0;
					//Item.useAnimation = 25;
					//Item.useTime = 25;
					Item.noMelee = false;
					//Item.noUseGraphic = false;
					Item.autoReuse = true;
					Item.useStyle = ItemUseStyleID.Swing;
					Item.hammer = 20;
					return base.CanUseItem(player);
				}
			}
			return false;
		}
		return base.CanUseItem(player);
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		return false;
	}
	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}
	public override bool AltFunctionUse(Player player)
	{
		return true;
	}
}