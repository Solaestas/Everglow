using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;

namespace Everglow.Yggdrasil.Common;

public class YggdrasilBiomeTileCounter : ModSystem
{
	public int DarkForestGrassCount;

	public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
	{
		DarkForestGrassCount = tileCounts[ModContent.TileType<DarkForestGrass>()];
	}
}