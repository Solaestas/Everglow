using Everglow.CagedDomain.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class UnionMarblePost_Body_Khaki_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 42;
		Item.height = 34;
		Item.maxStack = Item.CommonMaxStack;
		Item.createTile = ModContent.TileType<UnionMarblePost_Body_Khaki>();
		Item.placeStyle = 0;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 15;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.value = Item.sellPrice(0, 0, 1, 0);
		Item.rare = ItemRarityID.White;
	}

	public override void HoldItem(Player player)
	{
		Main.placementPreview = true;
	}
}