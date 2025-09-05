using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.KelpCurtain.Biomes;

namespace Everglow.Yggdrasil.KelpCurtain.Background;

public class DeathJadeLakeBackground : ModSystem
{
	public static Vector2 BiomeCenter = new Vector2(325, 18775) * 16;

	public float alpha = 0f;

	public override void PostUpdateEverything()// 开启地下背景
	{
		const float increase = 0.02f;
		if (Main.LocalPlayer.InModBiome<DeathJadeLakeBiome>() && Main.BackgroundEnabled)
		{
			DeathJadeLakeBiome dJLB = ModContent.GetInstance<DeathJadeLakeBiome>();
			if (dJLB != null && alpha == 0)
			{
				dJLB.GetLiquidSurfaceY();
			}
			if (alpha < 1)
			{
				alpha += increase;
			}
			else
			{
				alpha = 1;
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
		DrawBGLiquid(tex5, 0.4f);
		DrawBGLiquid(tex4, 0.5f);
		DrawBGLiquid(tex3, 0.6f);
		DrawBGLiquid(tex2, 0.8f);
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
		if (dJLB == null)
		{
			return;
		}
		float drawTop = dJLB.LiquidSurfaceY;
		if (drawTop - Main.screenPosition.Y < -Main.offScreenRange)
		{
			drawTop = -Main.offScreenRange + Main.screenPosition.Y;
		}
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
		int yLayers = (int)((drawBottom - drawTop) / 16f);
		for (int offsetY = 0; offsetY < yLayers; offsetY++)
		{
			float rightClamp = Main.screenWidth + Main.offScreenRange;
			float rightBound = Main.maxTilesX * 16;
			int tileY = (int)(drawTop / 16) + offsetY;
			if (dJLB.RightBoundOfACertainY.ContainsKey(tileY))
			{
				int rightX;
				dJLB.RightBoundOfACertainY.TryGetValue(tileY, out rightX);
				rightBound = rightX * 16;
			}
			rightBound -= Main.screenPosition.X;
			if (rightClamp > rightBound)
			{
				rightClamp = rightBound;
			}
			bars.Add(new Vector2(-Main.offScreenRange, drawTop + offsetY * 16 - Main.screenPosition.Y), baseColor, new Vector3(0, 0.7f, 0));
			bars.Add(new Vector2(rightClamp, drawTop + offsetY * 16 - Main.screenPosition.Y), baseColor, new Vector3(1, 0.7f, 0));
			bars.Add(new Vector2(-Main.offScreenRange, drawTop + (offsetY + 1) * 16 - Main.screenPosition.Y), baseColor, new Vector3(0, 0.9f, 0));

			bars.Add(new Vector2(-Main.offScreenRange, drawTop + (offsetY + 1) * 16 - Main.screenPosition.Y), baseColor, new Vector3(0, 0.9f, 0));
			bars.Add(new Vector2(rightClamp, drawTop + offsetY * 16 - Main.screenPosition.Y), baseColor, new Vector3(1, 0.7f, 0));
			bars.Add(new Vector2(rightClamp, drawTop + (offsetY + 1) * 16 - Main.screenPosition.Y), baseColor, new Vector3(1, 0.9f, 0));
		}
		if (bars.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = texSky;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
	}

	/// <summary>
	/// Draw a texture in the background layer.
	/// Auto rasterize the area that out of yWorldCoord Clamp (yWorldCoordMin, yWorldCoordMax).
	/// Liquid only.
	/// </summary>
	/// <param name="tex"></param>
	/// <param name="drawFrame"></param>
	/// <param name="baseColor"></param>
	/// <param name="yWorldCoordMin"></param>
	/// <param name="yWorldCoordMax"></param>
	/// <param name="xClamp"></param>
	/// <param name="yClamp"></param>
	public static void DrawBGLiquid(Texture2D tex, float moveScale)
	{
		DeathJadeLakeBackground deathJadeLakeBackground = ModContent.GetInstance<DeathJadeLakeBackground>();
		if (deathJadeLakeBackground is null)
		{
			return;
		}
		Color baseColor = Color.White * deathJadeLakeBackground.alpha;
		DeathJadeLakeBiome dJLB = ModContent.GetInstance<DeathJadeLakeBiome>();
		if (dJLB == null)
		{
			return;
		}
		float drawTop = dJLB.LiquidSurfaceY;
		if (drawTop - Main.screenPosition.Y < -Main.offScreenRange)
		{
			drawTop = -Main.offScreenRange + Main.screenPosition.Y;
		}
		float drawBottom = Main.screenPosition.Y + Main.screenHeight + Main.offScreenRange;
		if (drawTop > drawBottom)
		{
			return;
		}
		Vector2 totalOffset = (Main.screenPosition - BiomeCenter) * moveScale / 3000f;

		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		var bars = new List<Vertex2D>();
		int yLayers = (int)((drawBottom - drawTop) / 16f);

		for (int offsetY = 0; offsetY < yLayers; offsetY++)
		{
			float rightClamp = Main.screenWidth + Main.offScreenRange;
			float rightBound = Main.maxTilesX * 16;
			int tileY = (int)(drawTop / 16) + offsetY;
			if (dJLB.RightBoundOfACertainY.ContainsKey(tileY))
			{
				int rightX;
				dJLB.RightBoundOfACertainY.TryGetValue(tileY, out rightX);
				rightBound = rightX * 16;
			}
			rightBound -= Main.screenPosition.X;
			if (rightClamp > rightBound)
			{
				rightClamp = rightBound;
			}
			float topYcoord = offsetY * 16 / (float)tex.Height + totalOffset.Y - (-Main.offScreenRange + Main.screenPosition.Y - drawTop) / tex.Height;
			float bottomYcoord = (offsetY + 1) * 16 / (float)tex.Height + totalOffset.Y - (-Main.offScreenRange + Main.screenPosition.Y - drawTop) / tex.Height;
			if (topYcoord < 0)
			{
				topYcoord = 0;
			}
			if (bottomYcoord < 0)
			{
				bottomYcoord = 0;
			}
			if (topYcoord >= 1)
			{
				topYcoord = 0.999f;
			}
			if (bottomYcoord >= 1)
			{
				bottomYcoord = 0.999f;
			}
			float leftXcoord = totalOffset.X;
			float rightXcoord = totalOffset.X + (rightClamp + Main.offScreenRange) / tex.Width;
			bars.Add(new Vector2(-Main.offScreenRange, drawTop + offsetY * 16 - Main.screenPosition.Y), baseColor, new Vector3(leftXcoord, topYcoord, 0));
			bars.Add(new Vector2(rightClamp, drawTop + offsetY * 16 - Main.screenPosition.Y), baseColor, new Vector3(rightXcoord, topYcoord, 0));
			bars.Add(new Vector2(-Main.offScreenRange, drawTop + (offsetY + 1) * 16 - Main.screenPosition.Y), baseColor, new Vector3(leftXcoord, bottomYcoord, 0));

			bars.Add(new Vector2(-Main.offScreenRange, drawTop + (offsetY + 1) * 16 - Main.screenPosition.Y), baseColor, new Vector3(leftXcoord, bottomYcoord, 0));
			bars.Add(new Vector2(rightClamp, drawTop + offsetY * 16 - Main.screenPosition.Y), baseColor, new Vector3(rightXcoord, topYcoord, 0));
			bars.Add(new Vector2(rightClamp, drawTop + (offsetY + 1) * 16 - Main.screenPosition.Y), baseColor, new Vector3(rightXcoord, bottomYcoord, 0));
		}
		if (bars.Count > 2)
		{
			Main.graphics.GraphicsDevice.Textures[0] = tex;
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