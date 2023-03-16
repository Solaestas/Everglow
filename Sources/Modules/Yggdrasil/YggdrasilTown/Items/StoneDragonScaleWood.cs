using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class StoneDragonScaleWood : ModItem
{
	public override void SetStaticDefaults()
	{
		//DisplayName.SetDefault("Fossilized Dragon Scale Wood");
		//DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "石化龙鳞木");
	}
	public override void SetDefaults()
	{
		Item.width = 24;
		Item.height = 22;
		Item.rare = ItemRarityID.White;
		Item.scale = 1f;
		Item.createTile = ModContent.TileType<StoneScaleWood>();
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTurn = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.autoReuse = true;
		Item.consumable = true;
		Item.width = 16;
		Item.height = 16;
		Item.maxStack = 999;
		Item.value = 1000;
	}
}
