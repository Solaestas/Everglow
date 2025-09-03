using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.Common.BackgroundManager;
using Everglow.Yggdrasil.KelpCurtain.Biomes;

namespace Everglow.Yggdrasil.KelpCurtain.Background;

public class DeathJadeLakeBackground : ModSystem
{
	public static Vector2 BiomeCenter = new Vector2(325, 18760) * 16;

	public float alpha = 0f;

	public override void PostUpdateEverything()// 开启地下背景
	{
		const float increase = 0.02f;
		if (Main.LocalPlayer.InModBiome<DeathJadeLakeBiome>() && Main.BackgroundEnabled)
		{
			if (alpha < 1)
			{
				alpha += increase;
			}
			else
			{
				alpha = 1;
			}
			DeathJadeLakeBiome dJLB = ModContent.GetInstance<DeathJadeLakeBiome>();
			if (dJLB != null && dJLB.LiquidSurfaceY == 0)
			{
				dJLB.GetLiquidSurfaceY();
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
		}
	}

	public static void DrawFarBG()
	{
		DeathJadeLakeBackground deathJadeLakeBackground = ModContent.GetInstance<DeathJadeLakeBackground>();
		if (deathJadeLakeBackground is null)
		{
			return;
		}
		Color baseColor = Color.White * deathJadeLakeBackground.alpha;
		var texShore = ModAsset.DeathJadeLakeShore_Close.Value;

		var tex5 = ModAsset.DeathJadeLakeWater_5.Value;
		var tex4 = ModAsset.DeathJadeLakeWater_4.Value;
		var tex3 = ModAsset.DeathJadeLakeWater_3.Value;
		var tex2 = ModAsset.DeathJadeLakeWater_2.Value;
		int minY = (int)(Main.maxTilesY * 0.81f * 16);
		int maxY = (int)(Main.maxTilesY * 0.92f * 16);
		DrawLiquidSky();
		BackgroundManager.QuickDrawBG(tex5, GetDrawRect(tex5.Size(), 0.1f), baseColor, minY, maxY, false, true);
		BackgroundManager.QuickDrawBG(tex4, GetDrawRect(tex5.Size(), 0.2f), baseColor, minY, maxY, false, true);
		BackgroundManager.QuickDrawBG(tex3, GetDrawRect(tex5.Size(), 0.3f), baseColor, minY, maxY, false, true);
		BackgroundManager.QuickDrawBG(tex2, GetDrawRect(tex5.Size(), 0.4f), baseColor, minY, maxY, false, true);
	}

	public static void DrawLiquidSky()
	{
		DeathJadeLakeBackground deathJadeLakeBackground = ModContent.GetInstance<DeathJadeLakeBackground>();
		if (deathJadeLakeBackground is null)
		{
			return;
		}
		Color baseColor = Color.White * deathJadeLakeBackground.alpha;
		DeathJadeLakeBiome dJLB = ModContent.GetInstance<DeathJadeLakeBiome>();
		if(dJLB == null)
		{
			return;
		}
		float drawTop = dJLB.LiquidSurfaceY;
		float drawBottom = Main.screenPosition.Y + Main.screenHeight + Main.offScreenRange;
		if (drawTop > drawBottom)
		{
			return;
		}
		var texSky = ModAsset.DeathJadeLakeWater_Sky.Value;
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		var bars = new List<Vertex2D>();
		bars.Add(new Vector2(-Main.offScreenRange, drawTop - Main.screenPosition.Y), baseColor, new Vector3(0, 0.7f, 0));
		bars.Add(new Vector2(Main.screenWidth + Main.offScreenRange, drawTop - Main.screenPosition.Y), baseColor, new Vector3(1, 0.7f, 0));
		bars.Add(new Vector2(-Main.offScreenRange, drawBottom - Main.screenPosition.Y), baseColor, new Vector3(0, 0.9f, 0));

		bars.Add(new Vector2(-Main.offScreenRange, drawBottom - Main.screenPosition.Y), baseColor, new Vector3(0, 0.9f, 0));
		bars.Add(new Vector2(Main.screenWidth + Main.offScreenRange, drawTop - Main.screenPosition.Y), baseColor, new Vector3(1, 0.7f, 0));
		bars.Add(new Vector2(Main.screenWidth + Main.offScreenRange, drawBottom - Main.screenPosition.Y), baseColor, new Vector3(1, 0.9f, 0));

		if (bars.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = texSky;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	public static Rectangle GetDrawRect(Vector2 texSize, float MoveStep)
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

	public static void DrawBackground()
	{
		DeathJadeLakeBackground deathJadeLakeBackground = ModContent.GetInstance<DeathJadeLakeBackground>();
		if (deathJadeLakeBackground is null)
		{
			return;
		}
		if (deathJadeLakeBackground.alpha <= 0)
		{
			return;
		}
		DrawFarBG();
	}
}