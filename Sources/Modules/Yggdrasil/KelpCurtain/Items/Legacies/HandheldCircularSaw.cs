using Everglow.Yggdrasil.KelpCurtain.Projectiles.Legacies;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.ID;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Legacies;

public class HandheldCircularSaw : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 46;
		Item.height =54;
		Item.useAnimation = 5;
		Item.useTime = 5;
		Item.shootSpeed = 5f;
		Item.knockBack = 1.5f;
		Item.damage = 24; 
		Item.rare = ItemRarityID.Green;

		Item.DamageType = DamageClass.Melee;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.value = Item.sellPrice(gold: 1);
	}
	public override bool CanUseItem(Player player)
	{
		if (player.ownedProjectileCounts[ModContent.ProjectileType<HandheldCircularSaw_proj>()] == 0)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				if (player.altFunctionUse != 2)
					Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<HandheldCircularSaw_proj>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
			}
			return false;
		}
		return base.CanUseItem(player);
	}
}
