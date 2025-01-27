using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class ExitUnionTile_Item : ModItem
{
	public override void SetDefaults()
	{
		Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.ExitUnion>());
	}
}