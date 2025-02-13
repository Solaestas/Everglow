using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class MissionBoard_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 58;
		Item.height = 34;
		Item.maxStack = Item.CommonMaxStack;
		Item.createTile = ModContent.TileType<MissionBoard>();
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

	public override bool CanUseItem(Player player)
	{
		var mBoard = TileLoader.GetTile(ModContent.TileType<MissionBoard>()) as MissionBoard;
		if (mBoard != null)
		{
			int x = (int)(Main.MouseWorld.X / 16 - 5);
			int y = (int)(Main.MouseWorld.Y / 16 - 7);
			mBoard.PlaceOriginAtTopLeft(x, y);
			Item.stack--;
			return false;
		}
		return false;
	}

	public override bool? UseItem(Player player)
	{
		return false;
	}
}