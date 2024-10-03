using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;
using Everglow.Yggdrasil.YggdrasilTown.TwilightForest.Tiles;

namespace Everglow.Yggdrasil.Common;

public class YggdrasilBiomeTileCounter : ModSystem
{
	public int DarkForestGrassCount;
	public int TwilightGrassCount;

	public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
	{
		DarkForestGrassCount = tileCounts[ModContent.TileType<DarkForestGrass>()];
		TwilightGrassCount = tileCounts[ModContent.TileType<TwilightGrassBlock>()];
	}
}