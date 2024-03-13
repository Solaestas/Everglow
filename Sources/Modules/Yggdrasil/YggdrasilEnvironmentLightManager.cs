using Everglow.Yggdrasil.KelpCurtain.Background;
using Everglow.Yggdrasil.YggdrasilTown.Background;
using SubworldLibrary;
using Terraria.Graphics.Light;

namespace Everglow.Yggdrasil;

public class YggdrasilEnvironmentLightManager : ModSystem
{
	public override void OnModLoad()
	{
		if (Main.netMode != NetmodeID.Server)
		{
			On_TileLightScanner.ApplySurfaceLight += TileLightScanner_ApplySurfaceLight;
		}
	}
	public float Alpha = 0f;
	public static int LightingScene = -1;
	public Vector3 LightColor = Vector3.Zero;
	/// <summary>
	/// 按环境ID排序的光照效果, 环境ID用YggdrasilSceneID查询
	/// </summary>
	public readonly Vector3[] AmbientColors = { 
		new Vector3(0.2f, 0.1f, 0.3f), 
		new Vector3(1f, 238 / 255f, 188 / 255f),
		new Vector3(0.1f, 0.1f, 0.2f),
		new Vector3(1f, 238 / 255f, 188 / 255f),
		new Vector3(0.1f, 0.1f, 0.2f),
		new Vector3(1f, 238 / 255f, 188 / 255f)
	};

	/// <summary>
	/// 环境光的钩子
	/// </summary>
	/// <param name="orig"></param>
	/// <param name="self"></param>
	/// <param name="tile"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="lightColor"></param>
	private void TileLightScanner_ApplySurfaceLight(On_TileLightScanner.orig_ApplySurfaceLight orig, Terraria.Graphics.Light.TileLightScanner self, Tile tile, int x, int y, ref Vector3 lightColor)
	{
		orig(self, tile, x, y, ref lightColor);
		if(LightingScene > 0)
		{
			LightColor = Vector3.Lerp(LightColor, AmbientColors[LightingScene], 0.8f);
			lightColor = LightColor;
		}
	}
	public override void PostUpdateEverything()
	{
		if (Alpha < 1)
		{
			Alpha += 0.02f;
		}
		else
		{
			Alpha = 1;
		}
	}
}