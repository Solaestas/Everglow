namespace Everglow.Yggdrasil.CorruptWormHive.Items.Weapons;

public class TrueDeathSickle : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 116;
		Item.height = 140;
		Item.useAnimation = 5;
		Item.useTime = 5;

		Item.knockBack = 2.5f;
		Item.damage = 480;
		Item.rare = ItemRarityID.Purple;

		Item.DamageType = DamageClass.Melee;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		Item.value = Item.sellPrice(gold: 1);
	}
	public override bool CanUseItem(Player player)
	{
		if (base.CanUseItem(player))
		{
			if (Main.myPlayer == player.whoAmI)
			{
				if (player.altFunctionUse != 2)
					Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.TrueDeathSickle>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
				//else//右键
				//{
				//    Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.TrueDeathSickle_Super>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
				//}
			}
			return false;
		}
		return base.CanUseItem(player);
	}
	//public override bool AltFunctionUse(Player player)
	//{
	//    return true;
	//}
}
