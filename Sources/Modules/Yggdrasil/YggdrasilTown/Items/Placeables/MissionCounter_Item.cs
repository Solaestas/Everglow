using Everglow.Yggdrasil.YggdrasilTown.Tiles;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class MissionCounter_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 58;
		Item.height = 34;
		Item.maxStack = Item.CommonMaxStack;
		Item.createTile = ModContent.TileType<MissionCounter>();
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
		var mCounter = TileLoader.GetTile(ModContent.TileType<MissionCounter>()) as MissionCounter;
		if (mCounter != null)
		{
			int x = (int)(Main.MouseWorld.X / 16 - 11);
			int y = (int)(Main.MouseWorld.Y / 16 - 11);
			mCounter.PlaceOriginAtTopLeft(x, y);
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