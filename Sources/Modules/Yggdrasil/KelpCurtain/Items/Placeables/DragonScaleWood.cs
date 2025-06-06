using Terraria.ID;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Placeables;

public class DragonScaleWood : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.DragonScaleWood>());
		Item.width = 24;
		Item.height = 22;
		Item.rare = ItemRarityID.White;
	}
}
