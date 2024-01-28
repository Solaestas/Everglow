namespace Everglow.Myth.Misc.Items.Weapons.Slingshots;

public class TopazSlingshot : SlingshotItem
{
	//TODO:Translate:黄宝石弹弓
	public override void SetDef()
	{
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Ranged.Slingshots.TopazSlingshot>();
		Item.damage = 17;
		Item.width = 38;
		Item.height = 36;
		Item.useTime = 39;
		Item.useAnimation = 39;
		Item.rare = ItemRarityID.Orange;
		Item.value = Item.sellPrice(0, 0, 10, 0);
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Topaz, 8)
			.AddIngredient(ItemID.TinBar, 6)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
