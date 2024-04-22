namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class YggdrasilAmber : ModItem
{
	public override void SetDefaults()
	{
		//Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.StoneScaleWood>());
		Item.width = 24;
		Item.height = 22;
		Item.rare = ItemRarityID.White;
	}
}
