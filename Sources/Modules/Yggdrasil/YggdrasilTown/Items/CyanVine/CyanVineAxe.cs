using Everglow.Yggdrasil.YggdrasilTown.Items;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.CyanVine;

public class CyanVineAxe : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 46;
		Item.height = 50;
		Item.useAnimation = 25;
		Item.useTime = 25;
		Item.knockBack = 3.5f;
		Item.damage = 10;
		Item.rare = ItemRarityID.White;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.DamageType = DamageClass.Melee;

		Item.value = 3300;

		Item.axe = 12;
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 10)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 8)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
