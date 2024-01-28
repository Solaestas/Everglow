namespace Everglow.Myth.Misc.Items.Weapons.Slingshots;

public class EmeraldSlingshot : SlingshotItem
{
	//TODO:Translate:绿宝石弹弓
	public override void SetDef()
	{
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Ranged.Slingshots.EmeraldSlingshot>();
		Item.damage = 21;
		Item.width = 38;
		Item.height = 36;
		Item.useTime = 37;
		Item.useAnimation = 37;
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.sellPrice(0, 0, 14, 0);
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Emerald, 8)
			.AddIngredient(ItemID.TungstenBar, 6)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
