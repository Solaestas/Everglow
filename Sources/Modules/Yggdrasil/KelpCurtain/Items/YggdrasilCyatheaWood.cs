namespace Everglow.Yggdrasil.KelpCurtain.Items;

public class YggdrasilCyatheaWood : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.CyatheaWood>());
		Item.width = 24;
		Item.height = 22;
		Item.rare = ItemRarityID.White;
	}
}
