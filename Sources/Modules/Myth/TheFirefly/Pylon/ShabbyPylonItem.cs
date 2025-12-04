using Terraria.ObjectData;

namespace Everglow.Myth.TheFirefly.Pylon;

public class ShabbyPylonItem : ModItem
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.Placeables;

	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<ShabbyPylon>());
	}

	public override bool CanUseItem(Player player)
	{
		ushort TileID = (ushort)ModContent.TileType<ShabbyPylon>();
		var position = Main.MouseWorld;
		var bottom = position.ToTileCoordinates();

		if (TileObject.CanPlace(bottom.X, bottom.Y, TileID, 0, 0, out var tileObject) && TileObject.Place(tileObject))
		{
			TileObjectData.CallPostPlacementPlayerHook(bottom.X, bottom.Y, TileID, 0, 0, 0, tileObject);
		}

		return base.CanUseItem(player);
	}
}