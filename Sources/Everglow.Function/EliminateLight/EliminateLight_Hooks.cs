using Terraria.Graphics.Light;

namespace Everglow.Commons.EliminateLight;

public class EliminateLight_Hooks : ModSystem
{
	public override void Load()
	{
		On_TileLightScanner.ApplySurfaceLight += TileLightScanner_ApplySurfaceLight;
		base.Load();
	}

	private void TileLightScanner_ApplySurfaceLight(On_TileLightScanner.orig_ApplySurfaceLight orig, TileLightScanner self, Tile tile, int x, int y, ref Vector3 lightColor)
	{
		orig(self, tile, x, y, ref lightColor);
		EliminateLight.WallLightWithFakeBlock(x, y, ref lightColor);
	}
}