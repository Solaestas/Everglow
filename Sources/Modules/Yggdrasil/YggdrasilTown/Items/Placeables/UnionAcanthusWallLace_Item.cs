namespace Everglow.Yggdrasil.YggdrasilTown.Items.Placeables;

public class UnionAcanthusWallLace_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.UnionAcanthusWallLace>());
		Item.width = 16;
		Item.height = 16;
	}
}