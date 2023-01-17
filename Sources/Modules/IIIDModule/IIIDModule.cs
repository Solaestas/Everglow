using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Function.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.IIIDModule
{
    internal class IIIDModule : IModule
    {
        string IModule.Name => "IIID";
        void IModule.Load()
        {
           // On.Terraria.Main.DrawCapture += Main_DrawCapture;
        }        
        void IModule.Unload()
        {
           // On.Terraria.Main.DrawCapture -= Main_DrawCapture;
        }

        /*public void DrawCapture(Microsoft.Xna.Framework.Rectangle area, CaptureSettings settings) {
			float[] array = bgAlphaFrontLayer;
			bgAlphaFrontLayer = new float[array.Length];
			float[] array2 = bgAlphaFarBackLayer;
			bgAlphaFarBackLayer = new float[array2.Length];
			UpdateBGVisibility_BackLayer(settings.Biome.BackgroundIndex, 1f);
			UpdateBGVisibility_FrontLayer(settings.Biome.BackgroundIndex, 1f);
			float[] array3 = liquidAlpha.ToArray();
			int holyTileCount = SceneMetrics.HolyTileCount;
			SceneMetrics.HolyTileCount = ((settings.Biome.BackgroundIndex == 6) ? SceneMetrics.HallowTileMax : 0);
			int num = offScreenRange;
			offScreenRange = 0;
			SpriteViewMatrix gameViewMatrix = GameViewMatrix;
			GameViewMatrix = new SpriteViewMatrix(base.GraphicsDevice);
			Rasterizer = RasterizerState.CullCounterClockwise;
			bool captureEntities = settings.CaptureEntities;
			bool captureBackground = settings.CaptureBackground;
			CaptureBiome biome = settings.Biome;
			Vector2 vector = screenPosition;
			int num2 = screenWidth;
			int num3 = screenHeight;
			float num4 = cloudAlpha;
			bool captureMech = settings.CaptureMech;
			screenWidth = area.Width << 4;
			screenHeight = area.Height << 4;
			screenPosition = new Vector2(area.X * 16, area.Y * 16);
			cloudAlpha = 0f;
			for (int i = 0; i <= 10; i++) {
				if (i != 1)
					liquidAlpha[i] = ((i == biome.WaterStyle) ? 1f : 0f);
			}

			int x = area.X;
			int y = area.Y;
			int num5 = area.X + screenWidth / 16;
			int num6 = area.Y + screenHeight / 16;
			float num7 = (biome.TileColor == CaptureBiome.TileColorStyle.Mushroom).ToInt();
			InfoToSetBackColor info = default(InfoToSetBackColor);
			info.isInGameMenuOrIsServer = (gameMenu || netMode == 2);
			info.CorruptionBiomeInfluence = (biome.TileColor == CaptureBiome.TileColorStyle.Corrupt).ToInt();
			info.CrimsonBiomeInfluence = (biome.TileColor == CaptureBiome.TileColorStyle.Crimson).ToInt();
			info.JungleBiomeInfluence = (biome.TileColor == CaptureBiome.TileColorStyle.Jungle).ToInt();
			info.MushroomBiomeInfluence = num7;
			info.GraveyardInfluence = GraveyardVisualIntensity;
			info.BloodMoonActive = (biome.WaterStyle == 9);
			info.LanternNightActive = LanternNight.LanternsUp;
			SetBackColor(info, out Microsoft.Xna.Framework.Color sunColor, out Microsoft.Xna.Framework.Color moonColor);
			ApplyColorOfTheSkiesToTiles();
			ColorOfSurfaceBackgroundsBase = (ColorOfSurfaceBackgroundsModified = ColorOfTheSkies);
			bool flag = mapEnabled;
			mapEnabled = false;
			Lighting.Initialize();
			renderCount = 99;
			for (int j = 0; j < 4; j++) {
				Lighting.LightTiles(x, num5, y, num6);
			}

			mapEnabled = flag;
			if (!((float)(settings.Area.X * 16) > vector.X - 16f) || !((float)(settings.Area.Y * 16) > vector.Y - 16f) || !((float)((settings.Area.X + settings.Area.Width) * 16) < vector.X + (float)num2 + 16f) || !((float)((settings.Area.Y + settings.Area.Height) * 16) < vector.Y + (float)num3 + 16f)) {
				for (int k = 0; k < dust.Length; k++) {
					if (dust[k].active && dust[k].type == 76)
						dust[k].active = false;
				}
			}

			Vector2 vector2 = drawToScreen ? Vector2.Zero : new Vector2(offScreenRange, offScreenRange);
			int val = (int)((screenPosition.X - vector2.X) / 16f - 1f);
			int val2 = (int)((screenPosition.X + (float)screenWidth + vector2.X) / 16f) + 2;
			int val3 = (int)((screenPosition.Y - vector2.Y) / 16f - 1f);
			int val4 = (int)((screenPosition.Y + (float)screenHeight + vector2.Y) / 16f) + 5;
			vector2 -= screenPosition;
			val = Math.Max(val, 5) - 2;
			val3 = Math.Max(val3, 5);
			val2 = Math.Min(val2, maxTilesX - 5) + 2;
			val4 = Math.Min(val4, maxTilesY - 5) + 4;
			Microsoft.Xna.Framework.Rectangle drawArea = new Microsoft.Xna.Framework.Rectangle(val, val3, val2 - val, val4 - val3);
			LiquidRenderer.Instance.PrepareDraw(drawArea);
			WorldGen.SectionTileFrameWithCheck(x, y, num5, num6);
			if (captureBackground) {
				Matrix transform = Transform;
				int num8 = screenHeight;
				int num9 = screenWidth;
				Vector2 vector3 = screenPosition;
				bool flag2 = mapFullscreen;
				mapFullscreen = false;
				float num10 = scAdj;
				Vector2 value = new Vector2(num2, num3);
				Vector2 vector4 = new Vector2(settings.Area.Width * 16, settings.Area.Height * 16) / value;
				vector4.X = Math.Max(1f, vector4.X);
				vector4.Y = Math.Max(1f, vector4.Y);
				Vector2[] array4 = new Vector2[numClouds];
				for (int l = 0; l < numClouds; l++) {
					array4[l] = cloud[l].position;
					cloud[l].position *= vector4;
				}

				if ((float)(settings.Area.Height * 16) >= 2000f || (float)(settings.Area.Width * 16) >= 2000f) {
					scAdj = 0f;
					float num11 = 2048f;
					float num12 = MathHelper.Clamp((float)settings.Area.Height * 16f / num11, 1f, 3f);
					screenWidth = settings.Area.Width * 16;
					screenHeight = Math.Min(2048, settings.Area.Height * 16);
					screenPosition.X = settings.Area.X * 16;
					screenPosition.Y = settings.Area.Y * 16;
					screenPosition.Y += Math.Max(0f, Math.Min(settings.Area.Height, (float)worldSurface) * 16f - num11 * num12);
					transform *= Matrix.CreateScale(num12);
					transform.Translation += new Vector3((settings.Area.X - area.X) * 16, (settings.Area.Y - area.Y) * 16, 0f);
					transform.Translation += new Vector3(0f, Math.Max(0f, Math.Min(settings.Area.Height, (float)worldSurface) * 16f - num11 * num12) / num12, 0f);
				}
				else if ((float)(settings.Area.X * 16) > vector.X - 16f && (float)(settings.Area.Y * 16) > vector.Y - 16f && (float)((settings.Area.X + settings.Area.Width) * 16) < vector.X + (float)num2 + 16f && (float)((settings.Area.Y + settings.Area.Height) * 16) < vector.Y + (float)num3 + 16f) {
					screenPosition = vector;
					screenWidth = num2;
					screenHeight = num3;
					transform.Translation += new Vector3(vector.X - (float)area.X * 16f, vector.Y - (float)area.Y * 16f, 0f);
				}

				Vector2 areaPosition = new Vector2(area.X * 16, area.Y * 16);
				int areaWidth = area.Width * 16;
				int areaHeight = area.Height * 16;
				tileBatch.Begin();
				DrawSimpleSurfaceBackground(areaPosition, areaWidth, areaHeight);
				tileBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, transform);
				int num13 = screenWidth;
				int num14 = screenHeight;
				Vector2 zero = Vector2.Zero;
				if (num13 < 800) {
					int num15 = 800 - num13;
					zero.X -= (float)num15 * 0.5f;
					num13 = 800;
				}

				if (num14 < 600) {
					int num16 = 600 - num14;
					zero.Y -= (float)num16 * 0.5f;
					num14 = 600;
				}

				SceneArea sceneArea = default(SceneArea);
				sceneArea.bgTopY = 0;
				sceneArea.totalWidth = num13;
				sceneArea.totalHeight = num14;
				sceneArea.SceneLocalScreenPositionOffset = zero;
				SceneArea sceneArea2 = sceneArea;
				DrawStarsInBackground(sceneArea2);
				if ((double)(screenPosition.Y / 16f) < worldSurface + 2.0)
					DrawSunAndMoon(sceneArea2, moonColor, sunColor, num7);

				DrawSurfaceBG();
				spriteBatch.End();
				for (int m = 0; m < numClouds; m++) {
					cloud[m].position = array4[m];
				}

				scAdj = num10;
				mapFullscreen = flag2;
				screenWidth = num9;
				screenHeight = num8;
				screenPosition = vector3;
			}

			if (captureBackground) {
				spriteBatch.Begin();
				DrawUnderworldBackground(flat: true);
				spriteBatch.End();
			}

			if (captureEntities) {
				spriteBatch.Begin();
				CacheNPCDraws();
				CacheProjDraws();
				DrawCachedNPCs(DrawCacheNPCsMoonMoon, behindTiles: true);
				spriteBatch.End();
			}

			tileBatch.Begin();
			spriteBatch.Begin();
			DrawBlack(force: true);
			tileBatch.End();
			spriteBatch.End();
			tileBatch.Begin();
			spriteBatch.Begin();
			if (biome == null)
				DrawWater(bg: true, waterStyle);
			else
				DrawWater(bg: true, bloodMoon ? 9 : biome.WaterStyle);

			tileBatch.End();
			spriteBatch.End();
			if (captureBackground) {
				tileBatch.Begin();
				spriteBatch.Begin();
				DrawBackground();
				tileBatch.End();
				spriteBatch.End();
			}

			tileBatch.Begin();
			spriteBatch.Begin();
			DrawWalls();
			tileBatch.End();
			spriteBatch.End();
			if (captureEntities) {
				spriteBatch.Begin();
				DrawWoF();
				spriteBatch.End();
			}

			if (drawBackGore && captureEntities) {
				spriteBatch.Begin();
				DrawGoreBehind();
				spriteBatch.End();
				drawBackGore = true;
			}

			if (captureEntities) {
				spriteBatch.Begin();
				MoonlordDeathDrama.DrawPieces(spriteBatch);
				MoonlordDeathDrama.DrawExplosions(spriteBatch);
				spriteBatch.End();
			}

			bool flag3 = false;
			bool intoRenderTargets = false;
			bool intoRenderTargets2 = false;
			TilesRenderer.PreDrawTiles(solidLayer: false, flag3, intoRenderTargets2);
			tileBatch.Begin();
			spriteBatch.Begin();
			DrawCachedNPCs(DrawCacheNPCsBehindNonSolidTiles, behindTiles: true);
			int waterStyleOverride = bloodMoon ? 9 : biome.WaterStyle;
			if (biome == null)
				DrawTiles(solidLayer: false, flag3, intoRenderTargets);
			else
				DrawTiles(solidLayer: false, flag3, intoRenderTargets, waterStyleOverride);

			tileBatch.End();
			spriteBatch.End();
			DrawTileEntities(solidLayer: false, flag3, intoRenderTargets);
			if (captureEntities) {
				spriteBatch.Begin();
				waterfallManager.FindWaterfalls(forced: true);
				waterfallManager.Draw(spriteBatch);
				spriteBatch.End();
			}

			if (captureEntities) {
				DrawCachedProjs(DrawCacheProjsBehindNPCsAndTiles);
				spriteBatch.Begin();
				DrawNPCs(behindTiles: true);
				spriteBatch.End();
			}

			TilesRenderer.PreDrawTiles(solidLayer: true, flag3, intoRenderTargets2);
			tileBatch.Begin();
			spriteBatch.Begin();
			if (biome == null)
				DrawTiles(solidLayer: true, flag3, intoRenderTargets);
			else
				DrawTiles(solidLayer: true, flag3, intoRenderTargets, waterStyleOverride);

			tileBatch.End();
			spriteBatch.End();
			DrawTileEntities(solidLayer: true, flag3, intoRenderTargets);
			if (captureEntities) {
				DrawPlayers_BehindNPCs();
				DrawCachedProjs(DrawCacheProjsBehindNPCs);
				spriteBatch.Begin();
				DrawNPCs();
				spriteBatch.End();
				spriteBatch.Begin();
				DrawCachedNPCs(DrawCacheNPCProjectiles, behindTiles: false);
				spriteBatch.End();
				DrawSuperSpecialProjectiles(DrawCacheFirstFractals);
				DrawCachedProjs(DrawCacheProjsBehindProjectiles);
				DrawProjectiles();
				DrawPlayers_AfterProjectiles();
				DrawCachedProjs(DrawCacheProjsOverPlayers);
				spriteBatch.Begin();
				DrawCachedNPCs(DrawCacheNPCsOverPlayers, behindTiles: false);
				spriteBatch.End();
				spriteBatch.Begin();
				DrawItems();
				spriteBatch.End();
				spriteBatch.Begin();
				DrawRain();
				spriteBatch.End();
				spriteBatch.Begin();
				DrawGore();
				spriteBatch.End();
				DrawDust();
			}

			tileBatch.Begin();
			spriteBatch.Begin();
			if (biome == null)
				DrawWater(bg: false, waterStyle);
			else
				DrawWater(bg: false, biome.WaterStyle);

			if (captureMech)
				DrawWires();

			tileBatch.End();
			spriteBatch.End();
			DrawCachedProjs(DrawCacheProjsOverWiresUI);
			if (mapEnabled) {
				spriteBatch.Begin();
				for (int n = area.X; n < area.X + area.Width; n++) {
					for (int num17 = area.Y; num17 < area.Y + area.Height; num17++) {
						if (!Map.IsRevealed(n, num17))
							spriteBatch.Draw(TextureAssets.BlackTile.Value, new Vector2((float)n * 16f, (float)num17 * 16f) - screenPosition, Microsoft.Xna.Framework.Color.Black);
					}
				}

				spriteBatch.End();
			}

			renderCount = 99;
			screenWidth = num2;
			screenHeight = num3;
			screenPosition = vector;
			liquidAlpha = array3;
			offScreenRange = num;
			cloudAlpha = num4;
			bgAlphaFrontLayer = array;
			bgAlphaFarBackLayer = array2;
			SceneMetrics.HolyTileCount = holyTileCount;
			Lighting.Initialize();
			GameViewMatrix = gameViewMatrix;
		}*/
    }
}
