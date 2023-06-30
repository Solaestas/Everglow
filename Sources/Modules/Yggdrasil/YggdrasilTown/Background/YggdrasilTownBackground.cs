using Everglow.Yggdrasil.Common.BackgroundManager;
using SubworldLibrary;
using Terraria.Graphics.Light;

namespace Everglow.Yggdrasil.YggdrasilTown.Background;

public class YggdrasilTownBackground : ModSystem
{
	//// TODO Dup
	private Vector2 BiomeCenter = new(7200, 180000);
	/// <summary>
	/// 初始化
	/// </summary>
	public override void OnModLoad()
	{
		if (Main.netMode != NetmodeID.Server)
		{
			Ins.HookManager.AddHook(CodeLayer.PostDrawBG, DrawBackground);
		}
	}
	/// <summary>
	/// 环境光的钩子
	/// </summary>
	/// <param name="orig"></param>
	/// <param name="self"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="outputColor"></param>
	public float alpha = 0f;
	public override void PostUpdateEverything()//开启地下背景
	{
		const float increase = 0.02f;
		if (BiomeActive() && Main.BackgroundEnabled)
		{
			if (alpha < 1)
			{
				alpha += increase;
			}
			else
			{
				alpha = 1;
				Ins.HookManager.Disable(TerrariaFunction.DrawBackground);
			}

		}
		else
		{
			if (alpha > 0)
			{
				alpha -= increase;
			}
			else
			{
				alpha = 0;
			}
			Ins.HookManager.Enable(TerrariaFunction.DrawBackground);
		}
	}
	/// <summary>
	/// 判定是否开启地形
	/// </summary>
	/// <returns></returns>
	public static bool BiomeActive()
	{
		if (Main.screenPosition.Y > 169600)
		{
			if (SubworldSystem.IsActive<YggdrasilWorld>())
				return true;
		}
		return false;
	}

	private void DrawYggdrasilTownBackground(Color baseColor)
	{
		var texSky = ModAsset.YggdrasilTownBackgroundSky.Value;
		var texClose = ModAsset.YggdrasilTownBackgroundClose.Value;
		var texC1 = ModAsset.YggdrasilTownBackgroundC1.Value;
		var texC2 = ModAsset.YggdrasilTownBackgroundC2.Value;
		var texC3 = ModAsset.YggdrasilTownBackgroundC3.Value;
		var texBound = ModAsset.KelpCurtainBound.Value;

		BackgroundManager.QuickDrawBG(texSky, GetDrawRect(texSky.Size(), 0f, Vector2.Zero), baseColor, 171400, 200000);
		BackgroundManager.QuickDrawBG(texC3, GetDrawRect(texClose.Size(), 0.05f, Vector2.Zero), baseColor, 171400, 200000, false, false);
		BackgroundManager.QuickDrawBG(texC2, GetDrawRect(texClose.Size(), 0.10f, Vector2.Zero), baseColor, 171400, 200000, false, false);
		BackgroundManager.QuickDrawBG(texC1, GetDrawRect(texClose.Size(), 0.15f, new Vector2(0, 7200)), baseColor, 171400, 200000, false, true);

		Vector2 DCen = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
		Vector2 deltaPos = DCen - BiomeCenter;
		float MoveStep = 0.15f;
		deltaPos *= MoveStep;
		for (int x = -5; x < 6; x++)
		{
			Vector2 DrawCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f - deltaPos + new Vector2(-650 + texC1.Width * x, -400);
			if (DrawCenter.X >= -60 && DrawCenter.X <= Main.screenWidth + 60)
				BackgroundManager.DrawWaterfallInBackground(BiomeCenter, 0.15f, DrawCenter, 20f, 750f, baseColor * 0.06f, 171400, 200000, texC1.Size(), false, false);
			DrawCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f - deltaPos + new Vector2(-1350 + texC1.Width * x, -100);
			if (DrawCenter.X >= -60 && DrawCenter.X <= Main.screenWidth + 60)
				BackgroundManager.DrawWaterfallInBackground(BiomeCenter, 0.15f, DrawCenter, 20f, 750f, baseColor * 0.06f, 171400, 200000, texC1.Size(), false, false);
		}
		BackgroundManager.QuickDrawBG(texClose, GetDrawRect(texClose.Size(), 0.35f, Vector2.Zero), baseColor, 171400, 200000);

		BackgroundManager.DrawWaterfallInBackground(BiomeCenter, 0.35f, new Vector2(-650, -400), 60f, 550f, baseColor * 0.12f, 171400, 200000, texClose.Size());
		BackgroundManager.QuickDrawBG(texBound, GetDrawRect(texBound.Size(), 1f, Vector2.Zero), baseColor, 171280, 171580, false, false);
	}

	/// <summary>
	/// 获取XY向缩放比例
	/// </summary>
	/// <param name="texSize"></param>
	/// <param name="MoveStep"></param>
	/// <returns></returns>
	public static Vector2 GetZoomByScreenSize()
	{
		return Vector2.One;
	}
	/// <summary>
	/// 获取绘制矩形
	/// </summary>
	/// <param name="texSize"></param>
	/// <param name="MoveStep"></param>
	/// <returns></returns>
	public Rectangle GetDrawRect(Vector2 texSize, float moveStep, Vector2 correction = new Vector2())
	{
		Vector2 sampleTopleft = Vector2.Zero;
		Vector2 sampleCenter = sampleTopleft + texSize / 2;
		var screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
		Vector2 drawCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
		Vector2 deltaPos = drawCenter - (BiomeCenter + correction);
		deltaPos *= moveStep;
		int RX = (int)(sampleCenter.X - screenSize.X / 2f + deltaPos.X);
		int RY = (int)(sampleCenter.Y - screenSize.Y / 2f + deltaPos.Y);

		return new Rectangle(RX, RY, (int)screenSize.X, (int)screenSize.Y);
	}

	/// <summary>
	/// 当然是绘制主体啦
	/// </summary>
	private void DrawBackground()
	{
		if (alpha <= 0)
			return;
		Color baseColor = Color.White * alpha;
		DrawYggdrasilTownBackground(baseColor);
	}
}