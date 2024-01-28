namespace Everglow.Myth.Misc.Items.Weapons.Slingshots;

public class PalmWoodSlingshot : SlingshotItem
{
	//TODO:Translate:棕榈木弹弓
	public override void SetDef()
	{
		ProjType = ModContent.ProjectileType<Projectiles.Weapon.Ranged.Slingshots.PalmWoodSlingshot>();
		Item.damage = 7;
		Item.useTime = 24;
		Item.useAnimation = 24;
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Cobweb, 14)
			.AddIngredient(ItemID.PalmWood, 7)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
