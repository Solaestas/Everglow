namespace Everglow.Yggdrasil.YggdrasilTown.Items.LampWood;

public class LampWoodChandelier_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.LampWood.LampWoodChandelier>());
	}
}
