using Terraria.Localization;

namespace Everglow.Myth.TheTusk.Items;

public class TuskStatusII : ModItem
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Crimson Statue II");
		DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "猩红碑文石·其二");
	}

	public override void SetDefaults()
	{
		Item.width = 16;
		Item.height = 16;
		Item.maxStack = 999;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.createTile = ModContent.TileType<Tiles.StrangeTuskStone2>();
		Item.placeStyle = 0;
	}
}
