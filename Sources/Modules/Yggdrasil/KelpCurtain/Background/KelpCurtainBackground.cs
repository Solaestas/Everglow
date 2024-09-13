using Everglow.Yggdrasil.Common.BackgroundManager;
using SubworldLibrary;
using Terraria.Graphics.Light;

namespace Everglow.Yggdrasil.KelpCurtain.Background;

public class KelpCurtainBackground : ModSystem
{
	public Vector2 BiomeCenter = new Vector2(9000, 157000);
	public bool ZoneKelp = false;
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

	public override void PostUpdatePlayers()
	{
		var KelpCurtainBiome = new KelpCurtainBiome();
		ZoneKelp = KelpCurtainBiome.IsBiomeActive(Main.LocalPlayer);
	}
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

		if (Main.screenPosition.Y > 148000 && Main.screenPosition.Y < 170700)
		{
			if (SubworldSystem.IsActive<YggdrasilWorld>())
				return true;
		}
		return false;
	}

	private void DrawFarBG(Color baseColor)
	{
		var texSky = ModAsset.KelpCurtainSky.Value;
		var texClose = ModAsset.KelpCurtainClose.Value;
		var texC0 = ModAsset.KelpCurtainMiddleClose.Value;
		var texC1 = ModAsset.KelpCurtainMiddle.Value;
		var texC2 = ModAsset.KelpCurtainFar.Value;

		BackgroundManager.QuickDrawBG(texSky, GetDrawRect(texSky.Size(), 0f), baseColor, 148000, 173375, true, true);
		BackgroundManager.QuickDrawBG(texC2, GetDrawRect(texC2.Size(), 0.10f), baseColor, 148000, 173375, false, false);
		BackgroundManager.QuickDrawBG(texC1, GetDrawRect(texC1.Size(), 0.15f), baseColor, 148000, 173375, false, false);
		BackgroundManager.QuickDrawBG(texC0, GetDrawRect(texC1.Size(), 0.25f), baseColor, 148000, 173375, false, false);
		BackgroundManager.QuickDrawBG(texClose, GetDrawRect(texClose.Size(), 0.35f), baseColor, 148000, 173375, false, false);
	}
	/// <summary>
	/// 获取绘制矩形
	/// </summary>
	/// <param name="texSize"></param>
	/// <param name="MoveStep"></param>
	/// <returns></returns>
	public Rectangle GetDrawRect(Vector2 texSize, float MoveStep)
	{
		Vector2 sampleTopleft = Vector2.Zero;
		Vector2 sampleCenter = sampleTopleft + texSize / 2;
		var screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
		Vector2 dCen = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
		Vector2 deltaPos = dCen - BiomeCenter;
		deltaPos *= MoveStep;
		int RX = (int)(sampleCenter.X - screenSize.X / 2f + deltaPos.X);
		int RY = (int)(sampleCenter.Y - screenSize.Y / 2f + deltaPos.Y);
		var rt = new Rectangle(RX, RY, (int)screenSize.X, (int)screenSize.Y);
		return rt;
	}

	/// <summary>
	/// 当然是绘制主体啦
	/// </summary>
	private void DrawBackground()
	{
		if (alpha <= 0)
			return;
		Color baseColor = Color.White * alpha;
		DrawFarBG(baseColor);
	}
}