using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.Common.BackgroundManager;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using SubworldLibrary;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;

namespace Everglow.Yggdrasil.YggdrasilTown.Background;

public class YggdrasilTownBackground : ModSystem
{
	/// <summary>
	/// 地形中心
	/// </summary>
	public static Vector2 BiomeCenter => new Vector2(Main.maxTilesX / 2f * 16, (Main.maxTilesY - 1000) * 16);

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

	public float BackgroundSwitchAlpha = 0f;

	public override void PostUpdateEverything()// 开启地下背景
	{
		const float increase = 0.02f;
		if (BiomeActive() && Main.BackgroundEnabled)
		{
			if (BackgroundSwitchAlpha < 1)
			{
				BackgroundSwitchAlpha += increase;
			}
			else
			{
				BackgroundSwitchAlpha = 1;
				Ins.HookManager.Disable(TerrariaFunction.DrawBackground);
			}
		}
		else
		{
			if (BackgroundSwitchAlpha > 0)
			{
				BackgroundSwitchAlpha -= increase;
			}
			else
			{
				BackgroundSwitchAlpha = 0;
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
		if (Main.screenPosition.Y > (BiomeCenter.Y - 16000))
		{
			if (SubworldSystem.IsActive<YggdrasilWorld>())
			{
				YggdrasilEnvironmentLightManager.LightingScene = YggdrasilScene.YggdrasilTown;
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 灯木背景的绘制中心
	/// </summary>
	public float LampWoodCenterY = 0;

	/// <summary>
	/// 切换背景造成的颜色变化
	/// </summary>
	public float BackgroundSwitchingAlpha = 0f;

	/// <summary>
	/// 绘制天穹城镇背景
	/// </summary>
	/// <param name="baseColor"></param>
	private void DrawYggdrasilTownBackground(Color baseColor)
	{
		var texSky = ModAsset.YggdrasilTownBackgroundSky.Value;
		var texClose = ModAsset.Great_Outpost.Value;
		var texC1 = ModAsset.YggdrasilTownBackgroundC1.Value;
		var texC2 = ModAsset.YggdrasilTownBackgroundC2.Value;
		var texC3 = ModAsset.YggdrasilTownBackgroundC3.Value;
		var texBound = ModAsset.KelpCurtainBound.Value;

		Vector2 screenCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
		if (SubworldSystem.Current != null)
		{
			YggdrasilWorld yWorld = SubworldSystem.Current as YggdrasilWorld;
			if (yWorld != null)
			{
				// 获取挑战者石牢的位置
				if (yWorld.StoneCageOfChallengesCenter == Vector2.zeroVector)
				{
					for (int x = 50; x < Main.maxTilesX - 50; x++)
					{
						for (int y = 50; y < Main.maxTilesY - 50; y++)
						{
							Tile tile = SafeGetTile(x, y);
							if (tile.TileType == ModContent.TileType<SquamousShellSeal>())
							{
								if (tile.TileFrameX == 180 && tile.TileFrameY == 162)
								{
									yWorld.StoneCageOfChallengesCenter = new Vector2(x, y - 40) * 16;
									Main.NewText((x, y));
									x = Main.maxTilesX - 50;
									break;
								}
							}
						}
					}
					if (yWorld.StoneCageOfChallengesCenter == Vector2.zeroVector)
					{
						yWorld.StoneCageOfChallengesCenter = new Vector2(Main.maxTilesX * 16 / 2, Main.maxTilesY - 9000);
					}
				}

				// 如果在挑战者石牢附近
				if (Math.Abs(yWorld.StoneCageOfChallengesCenter.X - screenCenter.X) < 240 * 16)
				{
					if (Math.Abs(yWorld.StoneCageOfChallengesCenter.Y - screenCenter.Y) < 60 * 16)
					{
						var stoneClose2 = ModAsset.StoneCageOfChallengesClose2.Value;
						var stoneClose = ModAsset.StoneCageOfChallengesClose.Value;
						var stoneMiddle = ModAsset.StoneCageOfChallengesMiddle.Value;
						var stoneFar = ModAsset.StoneCageOfChallengesFar.Value;
						var stoneSky = ModAsset.StoneCageOfChallengesSky.Value;
						Vector2 correction = yWorld.StoneCageOfChallengesCenter + new Vector2(0, -8000) - BiomeCenter;
						float setSize = 1f;
						BackgroundManager.QuickDrawBG(stoneSky, GetDrawRect(stoneSky.Size(), 0f, correction, setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
						BackgroundManager.QuickDrawBG(stoneFar, GetDrawRect(stoneFar.Size(), 0.05f, correction, setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
						BackgroundManager.QuickDrawBG(stoneMiddle, GetDrawRect(stoneMiddle.Size(), 0.10f, correction + new Vector2(0, 5000), setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
						BackgroundManager.QuickDrawBG(stoneClose, GetDrawRect(stoneFar.Size(), 0.15f, correction + new Vector2(0, 6000), setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
						BackgroundManager.QuickDrawBG(stoneClose2, GetDrawRect(stoneMiddle.Size(), 0.20f, correction + new Vector2(0, 7000), setSize), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);
						return;
					}
				}
				if (ModContent.GetInstance<LampWoodForest>().IsBiomeActive(Main.LocalPlayer))
				{
					var lampClose = ModAsset.LampWoodClose.Value;
					var lampMiddle = ModAsset.LampWoodMiddle.Value;
					var lampFar = ModAsset.LampWoodFar.Value;
					var lampSky = ModAsset.LampWoodSky.Value;
					Vector2 correction = new Vector2(0, LampWoodCenterY - 4000) - BiomeCenter;
					if (LampWoodCenterY - Main.screenPosition.Y < -Main.screenHeight)
					{
						LampWoodCenterY = Main.screenPosition.Y;
						BackgroundSwitchingAlpha = 0;
					}
					float setSize = 1f;
					if (BackgroundSwitchingAlpha < 1f)
					{
						BackgroundSwitchingAlpha += 0.02f;
					}
					else
					{
						BackgroundSwitchingAlpha = 1f;
					}
					BackgroundManager.QuickDrawBG(lampSky, GetDrawRect(lampSky.Size(), 0f, correction, setSize), baseColor * BackgroundSwitchingAlpha, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), false, true);
					BackgroundManager.QuickDrawBG(lampFar, GetDrawRect(lampFar.Size(), 0.05f, correction, setSize), baseColor * BackgroundSwitchingAlpha, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), false, true);
					BackgroundManager.QuickDrawBG(lampMiddle, GetDrawRect(lampMiddle.Size(), 0.10f, correction + new Vector2(0, 5000), setSize), baseColor * BackgroundSwitchingAlpha, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), false, true);
					BackgroundManager.QuickDrawBG(lampClose, GetDrawRect(lampFar.Size(), 0.15f, correction + new Vector2(0, 6000), setSize), baseColor * BackgroundSwitchingAlpha, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), false, true);
				}
				else
				{
					if (Math.Abs(LampWoodCenterY - Main.screenPosition.Y) > 200)
					{
						LampWoodCenterY = Main.screenPosition.Y;
					}
					if (BackgroundSwitchingAlpha > 0f)
					{
						BackgroundSwitchingAlpha -= 0.02f;
					}
					else
					{
						BackgroundSwitchingAlpha = 0f;
					}
				}
			}
		}

		// 旧背景
		// BackgroundManager.QuickDrawBG(texSky, GetDrawRect(texSky.Size(), 0f, Vector2.Zero), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 16000));
		// BackgroundManager.QuickDrawBG(texC3, GetDrawRect(texClose.Size(), 0.05f, Vector2.Zero), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 16000), false, false);
		// BackgroundManager.QuickDrawBG(texC2, GetDrawRect(texClose.Size(), 0.10f, Vector2.Zero), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 16000), false, false);
		// BackgroundManager.QuickDrawBG(texC1, GetDrawRect(texClose.Size(), 0.15f, new Vector2(0, 7200)), baseColor, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 16000), false, true);
		float AntiBackgroundSwitchingAlpha = 1 - BackgroundSwitchingAlpha;
		Vector2 deltaPos = screenCenter - BiomeCenter;
		float MoveStep = 0.15f;
		deltaPos *= MoveStep;
		for (int x = -5; x < 6; x++)
		{
			Vector2 DrawCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f - deltaPos + new Vector2(-650 + texC1.Width * x, -400);
			if (DrawCenter.X >= -60 && DrawCenter.X <= Main.screenWidth + 60)
			{
				BackgroundManager.DrawWaterfallInBackground(BiomeCenter, 0.15f, DrawCenter, 20f, 750f, baseColor * 0.06f * AntiBackgroundSwitchingAlpha, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), texC1.Size(), false, false);
			}

			DrawCenter = new Vector2(Main.screenWidth, Main.screenHeight) / 2f - deltaPos + new Vector2(-1350 + texC1.Width * x, -100);
			if (DrawCenter.X >= -60 && DrawCenter.X <= Main.screenWidth + 60)
			{
				BackgroundManager.DrawWaterfallInBackground(BiomeCenter, 0.15f, DrawCenter, 20f, 750f, baseColor * 0.06f * AntiBackgroundSwitchingAlpha, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), texC1.Size(), false, false);
			}
		}
		Rectangle drawArea = GetDrawRect(texClose.Size(), 0.08f, Vector2.Zero);
		drawArea.Width = (int)(drawArea.Width * 0.6f);
		drawArea.Height = (int)(drawArea.Height * 0.6f);
		drawArea.Y += 200;
		drawArea.X += 500;
		BackgroundManager.QuickDrawBG(texClose, drawArea, baseColor * AntiBackgroundSwitchingAlpha, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), true, true);

		// BackgroundManager.DrawWaterfallInBackground(BiomeCenter, 0.35f, new Vector2(-650, -400), 60f, 550f, baseColor * 0.12f, (int)(BiomeCenter.Y - 20600), (int)(BiomeCenter.Y + 8000), texClose.Size());
		BackgroundManager.QuickDrawBG(texBound, GetDrawRect(texBound.Size(), 1f, Vector2.Zero), baseColor * AntiBackgroundSwitchingAlpha, (int)(BiomeCenter.Y - 20720), (int)(BiomeCenter.Y - 20420), false, false);
	}

	/// <summary>
	/// 获取绘制矩形
	/// </summary>
	/// <param name="texSize"></param>
	/// <param name="MoveStep"></param>
	/// <returns></returns>
	public Rectangle GetDrawRect(Vector2 texSize, float moveStep, Vector2 correction = default(Vector2), float scale = 1f)
	{
		Vector2 sampleTopleft = Vector2.Zero;
		Vector2 sampleCenter = sampleTopleft + texSize / 2;
		var screenSize = new Vector2(Main.screenWidth, Main.screenHeight);
		Vector2 drawCenter = Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
		Vector2 deltaPos = drawCenter - (BiomeCenter + correction);
		deltaPos *= moveStep;
		int RX = (int)(sampleCenter.X - screenSize.X / 2f / scale + deltaPos.X);
		int RY = (int)(sampleCenter.Y - screenSize.Y / 2f / scale + deltaPos.Y);

		return new Rectangle(RX, RY, (int)(screenSize.X / scale), (int)(screenSize.Y / scale));
	}

	/// <summary>
	/// 当然是绘制主体啦
	/// </summary>
	private void DrawBackground()
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		SpriteBatchState sBS2 = sBS;
		sBS2.BlendState = BlendState.NonPremultiplied;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS2);
		if (BackgroundSwitchAlpha <= 0)
		{
			return;
		}

		Color baseColor = Color.White * BackgroundSwitchAlpha;
		DrawYggdrasilTownBackground(baseColor);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}
}