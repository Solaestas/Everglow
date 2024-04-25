using Everglow.Yggdrasil.YggdrasilTown.Items;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.CyanVine;

public class CyanVineHammer : ModItem
{
	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.width = 38;
		Item.height = 42;
		Item.useAnimation = 26;
		Item.useTime = 26;
		Item.knockBack = 4.5f;
		Item.damage = 9;
		Item.rare = ItemRarityID.White;
		Item.UseSound = SoundID.Item1;
		Item.autoReuse = true;
		Item.DamageType = DamageClass.Melee;

		Item.value = 3600;

		Item.hammer = 68;
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ModContent.ItemType<CyanVineBar>(), 12)
			.AddIngredient(ModContent.ItemType<StoneDragonScaleWood>(), 6)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
