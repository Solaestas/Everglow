using Everglow.CagedDomain.Tiles;

namespace Everglow.CagedDomain.Items;

public class FluoriteLampPost_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 14;
		Item.height = 48;
		Item.maxStack = Item.CommonMaxStack;
		Item.createTile = ModContent.TileType<FluoriteLampPost>();
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
		FluoriteLampPost fluoriteLampPost = TileLoader.GetTile(ModContent.TileType<FluoriteLampPost>()) as FluoriteLampPost;
		if (fluoriteLampPost != null)
		{
			int x = (int)(Main.MouseWorld.X / 16 - 1);
			int y = (int)(Main.MouseWorld.Y / 16);
			if (fluoriteLampPost.CanPlaceAtBottomLeft(x, y))
			{
				if (fluoriteLampPost.CanPlaceAtBottomLeft(x, y))
				{
					fluoriteLampPost.PlaceOriginAtBottomLeft(x, y);
					Item.stack--;
					return false;
				}
			}
		}
		return false;
	}

	public override bool? UseItem(Player player)
	{
		return false;
	}
}