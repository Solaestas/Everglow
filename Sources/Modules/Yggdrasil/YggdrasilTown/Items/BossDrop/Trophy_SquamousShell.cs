namespace Everglow.Yggdrasil.YggdrasilTown.Items.BossDrop;

public class Trophy_SquamousShell : ModItem
{
	public override void SetDefaults()
	{
		// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.BossDrops.SquamousShell_Trophy>());

		Item.width = 32;
		Item.height = 32;
		Item.rare = ItemRarityID.Blue;
		Item.value = Item.buyPrice(0, 1);
	}
}