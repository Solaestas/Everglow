using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Light;

namespace Everglow.Commons.EliminateLight;

public class EliminateLight_Hooks : ModSystem
{
	public override void Load()
	{
		On_TileLightScanner.GetTileLight += On_TileLightScanner_GetTileLight;
		On_TileLightScanner.ExportTo += On_TileLightScanner_ExportTo;
		On_Main.DrawBlack += Main_DrawBlack;
		base.Load();
	}

	private void On_TileLightScanner_ExportTo(On_TileLightScanner.orig_ExportTo orig, TileLightScanner self, Rectangle area, LightMap outputMap, TileLightScannerOptions options)
	{
		orig(self, area, outputMap, options);
		EliminateLight.Point_BlockLightAsWall.Clear();
		EliminateLight.Point_BlockLight_Circle.Clear();
		EliminateLight.Point_BlockLight_Polygon.Clear();
	}

	private void On_TileLightScanner_GetTileLight(On_TileLightScanner.orig_GetTileLight orig, TileLightScanner self, int x, int y, out Vector3 outputColor)
	{
		orig(self, x, y, out outputColor);
		EliminateLight.WallLightWithFakeBlock(x, y, ref outputColor);
	}

	/// <summary>
	/// Warning: This hook delete the BlackTile drawing, which may cause some visual bugs.
	/// </summary>
	/// <param name="orig"></param>
	/// <param name="self"></param>
	/// <param name="force"></param>
	private void Main_DrawBlack(On_Main.orig_DrawBlack orig, Main self, bool force)
	{
		//orig(self, force);
	}
}