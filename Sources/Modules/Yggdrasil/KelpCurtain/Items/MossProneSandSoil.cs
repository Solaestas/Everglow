namespace Everglow.Yggdrasil.KelpCurtain.Items;

public class MossProneSandSoil : ModItem
{
	public override void SetStaticDefaults()
	{
	}
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.MossProneSandSoil>());
	}
}
