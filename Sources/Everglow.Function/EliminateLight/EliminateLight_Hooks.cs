using Terraria.Graphics.Light;
using Terraria.Utilities;

namespace Everglow.Commons.EliminateLight;

public class EliminateLight_Hooks : ModSystem
{
	public override void Load()
	{
		On_TileLightScanner.ApplyWallLight += On_TileLightScanner_ApplyWallLight;
		On_TileLightScanner.ExportTo += On_TileLightScanner_ExportTo;
		On_Main.DrawBlack += Main_DrawBlack;
		base.Load();
	}

	private void On_TileLightScanner_ApplyWallLight(On_TileLightScanner.orig_ApplyWallLight orig, TileLightScanner self, Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
	{
		orig(self, tile, x, y, ref localRandom, ref lightColor);
		EliminateLight.CheckEliminateLight(x, y, ref lightColor);
	}

	private void On_TileLightScanner_ExportTo(On_TileLightScanner.orig_ExportTo orig, TileLightScanner self, Rectangle area, LightMap outputMap, TileLightScannerOptions options)
	{
		orig(self, area, outputMap, options);
		EliminateLight.Clear();
	}

	/// <summary>
	/// Warning: This hook delete the BlackTile drawing, which may cause some visual bugs.
	/// </summary>
	/// <param name="orig"></param>
	/// <param name="self"></param>
	/// <param name="force"></param>
	private void Main_DrawBlack(On_Main.orig_DrawBlack orig, Main self, bool force)
	{
		// orig(self, force);
	}
}