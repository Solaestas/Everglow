using Everglow.Yggdrasil.KelpCurtain.Background;
using Everglow.Yggdrasil.YggdrasilTown.Background;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Graphics.Light;

namespace Everglow.Yggdrasil;

internal class YggdrasilEnvironmentLightManager : ModSystem
{
	public override void OnModLoad()
	{
		if (Main.netMode != NetmodeID.Server)
		{
			On_TileLightScanner.ApplySurfaceLight += TileLightScanner_ApplySurfaceLight;
			
		}
	}
	public float Alpha = 0f;
	public int Scene = -1;
	public int OldScene = -1;
	public int LastScene = -1;
	public Vector3 MulColor = Vector3.Zero;
	public readonly Vector3[] AmbientColors = { new Vector3(0.1f, 0.1f, 0.2f) , new Vector3(1f, 238 / 255f, 188 / 255f) };

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
		if (Scene != -1 && LastScene != -1)
		{
			MulColor = AmbientColors[Scene] * Alpha + AmbientColors[LastScene] * (1 - Alpha);
		}
		if (Scene != -1 && LastScene == -1)
		{
			MulColor = AmbientColors[Scene] * Alpha + lightColor * (1 - Alpha);
		}
		if (Scene == -1 && LastScene != -1)
		{
			MulColor = lightColor * Alpha + AmbientColors[LastScene] * (1 - Alpha);
		}
		lightColor *= MulColor;
	}
	public override void PostUpdateEverything()
	{
		Scene = -1;
		if (YggdrasilTownBackground.BiomeActive())
		{
			Scene = YggdrasilSceneID.YggdrasilTown;
		}
		if (KelpCurtainBackground.BiomeActive())
		{
			Scene = YggdrasilSceneID.KelpCurtain;
		}
		if (Scene != OldScene)
		{
			Alpha = 0f;
			LastScene = OldScene;
		}
		if (Alpha < 1)
		{
			Alpha += 0.02f;
		}
		else
		{
			Alpha = 1;
		}
		OldScene = Scene;
	}
}