using Terraria.WorldBuilding;
using SubworldLibrary;
using ReLogic.Content;
using Everglow.Myth.Common;

namespace Everglow.Myth.TheFirefly.WorldGeneration;

internal class MothWorld : Subworld
{
	public override int Width => 800;
	public override int Height => 600;
	public override bool NormalUpdates => true;
	public override bool ShouldSave => true;
	public override List<GenPass> Tasks => new List<GenPass>()
	{
		new MothLand.MothLandGenPass()
	};
	public override void DrawMenu(GameTime gameTime)
	{
		Texture2D MenuUG2SBG = (Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Backgrounds/FireflyUnderground1Screen4K", (AssetRequestMode)2);

		//Texture2D MenuSkyBG = (Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Backgrounds/FireflySky", (AssetRequestMode)2);
		//Texture2D MenuFarBG = (Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Backgrounds/FireflyFar", (AssetRequestMode)2);
		//Texture2D MenuMidCloseBG = (Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Backgrounds/FireflyMidClose", (AssetRequestMode)2);
		//Texture2D MenuMiddleBG = (Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Backgrounds/FireflyMiddle", (AssetRequestMode)2);
		//Texture2D MenuMiddleGlowBG = (Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Backgrounds/FireflyMiddleGlow", (AssetRequestMode)2);
		//Texture2D MenuCloseBG = (Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Backgrounds/FireflyClose", (AssetRequestMode)2);
		//Texture2D MenuClose2BG = (Texture2D)ModContent.Request<Texture2D>("Everglow/Myth/TheFirefly/Backgrounds/FireflyClose2", (AssetRequestMode)2);

		Vector2 zero = Vector2.Zero;
		#region MenuUGS2BG
		float uG1Width = (float)Main.screenWidth / (float)MenuUG2SBG.Width;
		float uG1Height = (float)Main.screenHeight / (float)MenuUG2SBG.Height;
		if (uG1Width != uG1Height)
		{
			if (uG1Height > uG1Width)
			{
				uG1Width = uG1Height;
				zero.X -= ((float)MenuUG2SBG.Width * uG1Width - (float)Main.screenWidth) * 0.5f;
			}
			else
			{
				zero.Y -= ((float)MenuUG2SBG.Height * uG1Width - (float)Main.screenHeight) * 0.5f;
			}
		}
		#endregion
		#region MenuSkyBG
		//float skyWidth = (float)Main.screenWidth / (float)MenuSkyBG.Width;
		//float shyHeight = (float)Main.screenHeight / (float)MenuSkyBG.Height;
		//if (skyWidth != shyHeight)
		//{
		//	if (shyHeight > skyWidth)
		//	{
		//		skyWidth = shyHeight;
		//		zero.X -= ((float)MenuSkyBG.Width * skyWidth - (float)Main.screenWidth) * 0f;
		//	}
		//	else
		//	{
		//		zero.Y -= ((float)MenuSkyBG.Height * skyWidth - (float)Main.screenHeight) * 0f;
		//	}
		//}
		#endregion
		#region MenuFarBG
		//float farWidth = (float)Main.screenWidth / (float)MenuFarBG.Width;
		//float farHeight = (float)Main.screenHeight / (float)MenuFarBG.Height;
		//if (farWidth != farHeight)
		//{
		//	if (farHeight > farWidth)
		//	{
		//		farWidth = farHeight;
		//		zero.X -= ((float)MenuFarBG.Width * farWidth - (float)Main.screenWidth) * 0f;
		//	}
		//	else
		//	{
		//		zero.Y -= ((float)MenuFarBG.Height * farWidth - (float)Main.screenHeight) * 0f;
		//	}
		//}
		#endregion
		#region MenuMidCloseBG
		//float midCloseWidth = (float)Main.screenWidth / (float)MenuMidCloseBG.Width;
		//float midCloseHeight = (float)Main.screenHeight / (float)MenuMidCloseBG.Height;
		//if (midCloseWidth != midCloseHeight)
		//{
		//	if (midCloseHeight > midCloseWidth)
		//	{
		//		midCloseWidth = midCloseHeight;
		//		zero.X -= ((float)MenuMidCloseBG.Width * midCloseWidth - (float)Main.screenWidth) * 0f;
		//	}
		//	else
		//	{
		//		zero.Y -= ((float)MenuMidCloseBG.Height * midCloseWidth - (float)Main.screenHeight) * 0f;
		//	}
		//}
		#endregion
		#region MenuMiddleBG
		//float middleWidth = (float)Main.screenWidth / (float)MenuMiddleBG.Width;
		//float middleHeight = (float)Main.screenHeight / (float)MenuMiddleBG.Height;
		//if (middleWidth != middleHeight)
		//{
		//	if (middleHeight > middleWidth)
		//	{
		//		middleWidth = middleHeight;
		//		zero.X -= ((float)MenuMiddleBG.Width * middleWidth - (float)Main.screenWidth) * 0f;
		//	}
		//	else
		//	{
		//		zero.Y -= ((float)MenuMiddleBG.Height * middleWidth - (float)Main.screenHeight) * 0f;
		//	}
		//}
		#endregion
		#region MenuMiddleGlowBG
		//float middleGlowWidth = (float)Main.screenWidth / (float)MenuMiddleGlowBG.Width;
		//float middleGlowHeight = (float)Main.screenHeight / (float)MenuMiddleGlowBG.Height;
		//if (middleGlowWidth != middleGlowHeight)
		//{
		//	if (middleGlowHeight > middleGlowWidth)
		//	{
		//		middleGlowWidth = middleGlowHeight;
		//		zero.X -= ((float)MenuMiddleGlowBG.Width * middleGlowWidth - (float)Main.screenWidth) * 0f;
		//	}
		//	else
		//	{
		//		zero.Y -= ((float)MenuMiddleGlowBG.Height * middleGlowWidth - (float)Main.screenHeight) * 0f;
		//	}
		//}
		#endregion

		Main.spriteBatch.Draw(MenuUG2SBG, zero, (Rectangle?)null, Color.White, 0f, Vector2.Zero, uG1Width, (SpriteEffects)0, 0f);
		//Main.spriteBatch.Draw(MenuSkyBG, zero, (Rectangle?)null, Color.White, 0f, Vector2.Zero, skyWidth, (SpriteEffects)0, 0f);
		//Main.spriteBatch.Draw(MenuFarBG, zero, (Rectangle?)null, Color.White, 0f, Vector2.Zero, farWidth, (SpriteEffects)0, 0f);
		//Main.spriteBatch.Draw(MenuMidCloseBG, zero, (Rectangle?)null, Color.White, 0f, Vector2.Zero, midCloseWidth, (SpriteEffects)0, 0f);
		//Main.spriteBatch.Draw(MenuMiddleBG, zero, (Rectangle?)null, Color.White, 0f, Vector2.Zero, middleWidth, (SpriteEffects)0, 0f);
		//Main.spriteBatch.Draw(MenuMiddleGlowBG, zero, (Rectangle?)null, Color.White, 0f, Vector2.Zero, middleGlowWidth, (SpriteEffects)0, 0f);

		base.DrawMenu(gameTime);
		//return;
	}
	//public override bool ChangeAudio()
	// TODO: MothBiomeOld should play when entering and exiting the firefly subworld but MothBiome should play while inside the subworld.
	//{
	//	Main.newMusic = MythContent.QuickMusic("MothBiomeOld");
	//	if (SubworldSystem.IsActive<MothWorld>())
	//	{
	//		Main.newMusic = MythContent.QuickMusic("MothBiome");
	//		return true;
	//	}
	//	return false;
	//}
	public override void OnEnter()
	{
		SubworldSystem.hideUnderworld = true;
	}
	public override void OnLoad()
	{
		Main.worldSurface = 20;
		Main.rockLayer = 150;
		GenVars.waterLine = 50;
	}
}
