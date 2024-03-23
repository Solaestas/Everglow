namespace Everglow.Yggdrasil.KelpCurtain.Items;

public class OldMoss : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.OldMoss>());
		Item.width = 16;
		Item.height = 16;
	}
}
