using Everglow.Commons.Templates.Weapons.StabbingSwords;
using Everglow.Minortopography.GiantPinetree.Dusts;

namespace Everglow.Minortopography.GiantPinetree.TilesAndWalls;

public class PineLeaves : ModTile
{
	public override void PostSetDefaults()
	{
		Main.tileSolid[Type] = true;
		Main.tileBlockLight[Type] = true;
		Main.tileMerge[Type][TileID.PineTree] = true;
		Main.tileMerge[TileID.PineTree][Type] = true;
		DustType = ModContent.DustType<PineDust>();
		HitSound = SoundID.Grass;
		AddMapEntry(new Color(36, 64, 50));
		if (!TileClassification.StabbingSwordFragileTileType.Contains(Type))
		{
			TileClassification.StabbingSwordFragileTileType.Add(Type);
		}
	}

	public override IEnumerable<Item> GetItemDrops(int i, int j)
	{
		yield break;
	}
}

public class PineSnowSystem : ModSystem
{
	public int PineLeavesCount;

	public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
	{
		PineLeavesCount = tileCounts[ModContent.TileType<PineLeaves>()];
	}

	public override void Load()
	{
		On_SceneMetrics.ExportTileCountsToMain += SceneMetrics_ExportTileCountsToMain;
	}

	private void SceneMetrics_ExportTileCountsToMain(On_SceneMetrics.orig_ExportTileCountsToMain orig, Terraria.SceneMetrics self)
	{
		orig(self);
		Main.SceneMetrics.SnowTileCount += PineLeavesCount * 5;
	}
}