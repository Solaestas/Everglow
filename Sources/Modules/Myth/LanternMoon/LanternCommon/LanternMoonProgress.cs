using Everglow.Myth.LanternMoon.NPCs;
using Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;
using Terraria.GameContent;
using Terraria.Localization;
namespace Everglow.Myth.LanternMoon.LanternCommon;

public class LanternMoonProgress : ModSystem//灯笼月
{
	public override void PostUpdateInvasions()
	{
		//AddPoint(8);
		if (PreWavePoint[0] == 0)//共计40波，每一波需要的分数
		{
			PreWavePoint[0] = 100;
			PreWavePoint[1] = 150;
			PreWavePoint[2] = 200;
			PreWavePoint[3] = 250;
			PreWavePoint[4] = 300;
			PreWavePoint[5] = 400;
			PreWavePoint[6] = 500;
			PreWavePoint[7] = 500;
			PreWavePoint[8] = 600;//3000
			PreWavePoint[9] = 600;
			PreWavePoint[10] = 600;
			PreWavePoint[11] = 800;//5000
			PreWavePoint[12] = 1000;
			PreWavePoint[13] = 1000;//7000
			PreWavePoint[14] = 3000;//10000

			PreWavePoint[15] = 1200;
			PreWavePoint[16] = 1350;
			PreWavePoint[17] = 1400;
			PreWavePoint[18] = 1500;
			PreWavePoint[19] = 1750;
			PreWavePoint[20] = 2000;//19200
			PreWavePoint[21] = 2250;
			PreWavePoint[22] = 2750;
			PreWavePoint[23] = 3300;//27500
			PreWavePoint[24] = 6000;//33500

			PreWavePoint[25] = 4500;
			PreWavePoint[26] = 5300;
			PreWavePoint[27] = 6100;
			PreWavePoint[28] = 6800;
			PreWavePoint[29] = 7500;
			PreWavePoint[30] = 9000;
			PreWavePoint[31] = 11500;
			PreWavePoint[32] = 12600;
			PreWavePoint[33] = 15000;
			PreWavePoint[34] = 20000;//131800

			PreWavePoint[35] = 123500;
			PreWavePoint[36] = 167800;
			PreWavePoint[37] = 235500;
			PreWavePoint[38] = 341400;//1000000
			PreWavePoint[39] = 0;
		}
		if (OnLanternMoon)
		{
			AddEnemies();
			if (Wave == 0)
				WavePoint = Point;
			if (WavePoint > PreWavePoint[Wave])
			{
				if (Wave == 0)
					Main.NewText("Wave 2:", new Color(175, 75, 255));
				if (Wave == 1)
					Main.NewText("Wave 3:", new Color(175, 75, 255));
				if (Wave == 2)
					Main.NewText("Wave 4:", new Color(175, 75, 255));
				if (Wave == 3)
					Main.NewText("Wave 5:", new Color(175, 75, 255));
				if (Wave == 4)
					Main.NewText("Wave 6:", new Color(175, 75, 255));
				if (Wave == 5)
					Main.NewText("Wave 7:", new Color(175, 75, 255));
				WavePoint -= PreWavePoint[Wave];
				Wave++;
			}
		}
		if (Main.dayTime)
		{
			OnLanternMoon = false;
			Main.invasionProgressMode = 0;
		}
	}
	public int Wave = 0;
	public int[] PreWavePoint = new int[40];
	public int Point = 0;
	public int WavePoint = 0;
	public bool OnLanternMoon = false;
	public void AddPoint(int value)
	{
		WavePoint += value;
		Point += value;
	}
	public override void PostDrawInterface(SpriteBatch spriteBatch)
	{
		if (!OnLanternMoon)
			return;
		Main.invasionProgressMax = PreWavePoint[Wave];
		Main.invasionProgress = WavePoint;
		if (OnLanternMoon)
		{
			Main.invasionProgressMode = 2;
			Main.invasionProgressNearInvasion = true;
			Main.invasionProgressWave = Wave + 1;
		}
		if (Main.invasionProgressMode == 2 && Main.invasionProgressNearInvasion && Main.invasionProgressDisplayLeft < 160)
			Main.invasionProgressDisplayLeft = 160;

		if (!Main.gamePaused && Main.invasionProgressDisplayLeft > 0)
			Main.invasionProgressDisplayLeft--;
		if (Main.invasionProgressDisplayLeft > 0)
			Main.invasionProgressAlpha += 0.005f;
		else
		{
			Main.invasionProgressAlpha -= 0.005f;
		}
		if (Main.invasionProgressMode == 0)
		{
			Main.invasionProgressDisplayLeft = 0;
			Main.invasionProgressAlpha = 0f;
		}


		Main.invasionProgressAlpha = MathHelper.Clamp(Main.invasionProgressAlpha, 0, 1);
		if (Main.invasionProgressAlpha == 0f)
			return;


		float fadinAlpha = 0.5f + Main.invasionProgressAlpha * 0.5f;
		Texture2D lanternMoonImage = ModAsset.LanternMoonIcon.Value;
		//TODO Hjson，示例我在WorldSystem里写了
		string LanternMoon = " Lantern Moon ";
		Color BGColor = Color.White;
		int barSizeX = (int)(200f * fadinAlpha);
		int barSizeY = (int)(45f * fadinAlpha);
		var barCenter = new Vector2(Main.screenWidth - 120, Main.screenHeight - 40);
		string waveStage = Main.invasionProgressMax == 0 ?
			WavePoint.ToString() :
			(int)(Main.invasionProgress * 100f / Main.invasionProgressMax) + "%";
		float textSizeX = 169f * fadinAlpha;
		float textSizeY = 8f * fadinAlpha;
		Texture2D colorBar = TextureAssets.ColorBar.Value;
		if (Main.invasionProgressWave > 0)
			waveStage = Language.GetTextValue("Game.WaveMessage", Main.invasionProgressWave, waveStage);
		else
		{
			waveStage = Language.GetTextValue("Game.WaveCleared", waveStage);
		}

		float waveRate = Main.invasionProgressMax == 0 ? 1 :
				MathHelper.Clamp(Main.invasionProgress / (float)Main.invasionProgressMax, 0f, 1f);

		Vector2 textPosition = barCenter + Vector2.UnitY * textSizeY + Vector2.UnitX * 1f;

		//进度条背景
		Utils.DrawInvBG(Main.spriteBatch,
			new Rectangle((int)barCenter.X - barSizeX / 2, (int)barCenter.Y - barSizeY / 2, barSizeX, barSizeY),
			new Color(63 * 0.785f, 65 * 0.785f, 151 * 0.785f, 255 * 0.785f));
		//进度条文字
		Utils.DrawBorderString(Main.spriteBatch, waveStage, textPosition, Color.White * Main.invasionProgressAlpha, fadinAlpha, 0.5f, 1f, -1);
		//进度条外框
		Main.spriteBatch.Draw(colorBar, barCenter, null, Color.White * Main.invasionProgressAlpha, 0f, new Vector2(colorBar.Width / 2, 0f), fadinAlpha, SpriteEffects.None, 0f);

		textPosition += Vector2.UnitX * (waveRate - 0.5f) * textSizeX;

		//进度条，由两短一长的像素条组成，有点……
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value,
			textPosition,
			new Rectangle(0, 0, 1, 1),
			new Color(255, 241, 51) * Main.invasionProgressAlpha,
			0f,
			new Vector2(1f, 0.5f),
			new Vector2(textSizeX * waveRate, textSizeY),
			SpriteEffects.None,
			0f);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value,
			textPosition,
			new Rectangle(0, 0, 1, 1),
			new Color(255, 165, 0, 127) * Main.invasionProgressAlpha,
			0f,
			new Vector2(1f, 0.5f),
			new Vector2(2f, textSizeY),
			SpriteEffects.None,
			0f);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value,
			textPosition,
			new Rectangle(0, 0, 1, 1),
			Color.Black * Main.invasionProgressAlpha,
			0f,
			new Vector2(0f, 0.5f),
			new Vector2(textSizeX * (1f - waveRate), textSizeY),
			SpriteEffects.None,
			0f);

		Vector2 lanternMoonSize = FontAssets.MouseText.Value.MeasureString(LanternMoon);
		float OffsetX = 120f;
		if (lanternMoonSize.X > 200f)
			OffsetX += lanternMoonSize.X - 200f;
		Rectangle BGRectangle = Utils.CenteredRectangle(
			new Vector2(Main.screenWidth - OffsetX, Main.screenHeight - 80),
			(lanternMoonSize + new Vector2(lanternMoonImage.Width + 12, 6f)) * fadinAlpha);
		//灯笼月图标背景
		Utils.DrawInvBG(Main.spriteBatch,
			BGRectangle,
			BGColor);
		//灯笼月图标
		Main.spriteBatch.Draw(lanternMoonImage,
			BGRectangle.Left() + Vector2.UnitX * fadinAlpha * 8f,
			null,
			Color.White * Main.invasionProgressAlpha,
			0f,
			new Vector2(0f, lanternMoonImage.Height / 2),
			fadinAlpha * 0.8f,
			SpriteEffects.None,
			0f);
		//灯笼月文字
		Utils.DrawBorderString(Main.spriteBatch,
			LanternMoon,
			BGRectangle.Right() + Vector2.UnitX * fadinAlpha * -22f,
			Color.White * Main.invasionProgressAlpha,
			fadinAlpha * 0.9f,
			1f,
			0.4f,
			-1);
	}

	public void AddEnemies()
	{
		if (Wave <= 14)
		{
			if(NPC.CountNPCS(ModContent.NPCType<FloatLantern>()) < 15)
			{ 
				if(Main.rand.NextBool(20))
				{
					int x0 = (int)(Main.screenPosition.X - Main.offScreenRange - 150);
					int x1 = (int)(Main.screenPosition.X + Main.screenWidth + Main.offScreenRange + 150);
					int y0 = (int)(Main.screenPosition.Y + Main.screenHeight * 0.5f);
					NPC.NewNPC(NPC.GetSource_NaturalSpawn(), Main.rand.NextBool(2) ? x0 : x1, y0, ModContent.NPCType<FloatLantern>());
				}
				
			}
			if (Wave >= 5)
			{
				if (Main.rand.NextBool(20))
				{
					int x0 = (int)(Main.screenPosition.X - Main.offScreenRange - 150);
					int x1 = (int)(Main.screenPosition.X + Main.screenWidth + Main.offScreenRange + 150);
					int y0 = (int)(Main.screenPosition.Y + Main.screenHeight * 0.5f);
					NPC.NewNPC(NPC.GetSource_NaturalSpawn(), Main.rand.NextBool(2) ? x0 : x1, y0, ModContent.NPCType<BombLantern>());
				}
			}
			if (Wave >= 8)
			{
				if (Main.rand.NextBool(20))
				{
					int x0 = (int)(Main.screenPosition.X - Main.offScreenRange - 150);
					int x1 = (int)(Main.screenPosition.X + Main.screenWidth + Main.offScreenRange + 150);
					int y0 = (int)(Main.screenPosition.Y + Main.screenHeight * 0.5f);
					NPC.NewNPC(NPC.GetSource_NaturalSpawn(), Main.rand.NextBool(2) ? x0 : x1, y0, ModContent.NPCType<CylindricalLantern>());
				}
			}
		}
		if(Wave == 15)
		{
			if (NPC.CountNPCS(ModContent.NPCType<LanternGhostKing>()) < 1)
			{
				int x0 = (int)(Main.screenPosition.X - Main.offScreenRange - 150);
				int x1 = (int)(Main.screenPosition.X + Main.screenWidth + Main.offScreenRange + 150);
				int y0 = (int)(Main.screenPosition.Y + Main.screenHeight * 0.5f);
				NPC.NewNPC(NPC.GetSource_NaturalSpawn(), Main.rand.NextBool(2) ? x0 : x1, y0, ModContent.NPCType<LanternGhostKing>());
			}
		}
	}
}
