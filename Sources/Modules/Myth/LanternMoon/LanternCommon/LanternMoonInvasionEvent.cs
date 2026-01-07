using Everglow.Commons.Mechanics.Events;
using Everglow.Myth.LanternMoon.NPCs;
using Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Myth.LanternMoon.LanternCommon;

public class LanternMoonInvasionEvent : ModEvent
{
	/// <summary>
	/// Current wavw
	/// </summary>
	public int Wave = 0;

	/// <summary>
	/// Point value for completing the current wave
	/// </summary>
	public int[] ScoreRequireOfWave = new int[40];

	/// <summary>
	/// Total point account from the beginning.
	/// </summary>
	public int AccumulatedScore = 0;

	public int WaveScore()
	{
		int score = AccumulatedScore;
		int w = 0;
		while (score > ScoreRequireOfWave[w])
		{
			score -= ScoreRequireOfWave[w];
			w++;
			if (w >= 40)
			{
				break;
			}
		}
		return score;
	}

	public void Initialization()
	{
		// Total: 40 Waves
		ScoreRequireOfWave[0] = 100;
		ScoreRequireOfWave[1] = 150;
		ScoreRequireOfWave[2] = 200;
		ScoreRequireOfWave[3] = 250;
		ScoreRequireOfWave[4] = 300;
		ScoreRequireOfWave[5] = 400;
		ScoreRequireOfWave[6] = 500;
		ScoreRequireOfWave[7] = 500;
		ScoreRequireOfWave[8] = 600; // 3000
		ScoreRequireOfWave[9] = 600;
		ScoreRequireOfWave[10] = 600;
		ScoreRequireOfWave[11] = 800; // 5000
		ScoreRequireOfWave[12] = 1000;
		ScoreRequireOfWave[13] = 1500; // 7000
		ScoreRequireOfWave[14] = 2500; // 10000

		ScoreRequireOfWave[15] = 1200;
		ScoreRequireOfWave[16] = 1350;
		ScoreRequireOfWave[17] = 1400;
		ScoreRequireOfWave[18] = 1500;
		ScoreRequireOfWave[19] = 1750;
		ScoreRequireOfWave[20] = 2000; // 19200
		ScoreRequireOfWave[21] = 2250;
		ScoreRequireOfWave[22] = 2750;
		ScoreRequireOfWave[23] = 3300; // 27500
		ScoreRequireOfWave[24] = 6000; // 33500

		ScoreRequireOfWave[25] = 4500;
		ScoreRequireOfWave[26] = 5300;
		ScoreRequireOfWave[27] = 6100;
		ScoreRequireOfWave[28] = 6800;
		ScoreRequireOfWave[29] = 7500;
		ScoreRequireOfWave[30] = 9000;
		ScoreRequireOfWave[31] = 11500;
		ScoreRequireOfWave[32] = 12600;
		ScoreRequireOfWave[33] = 15000;
		ScoreRequireOfWave[34] = 20000; // 131800

		ScoreRequireOfWave[35] = 123500;
		ScoreRequireOfWave[36] = 167800;
		ScoreRequireOfWave[37] = 235500;
		ScoreRequireOfWave[38] = 341400; // 1000000
		ScoreRequireOfWave[39] = 0;

		var messageColor = new Color(175, 75, 255);
		Color messageColor1 = Color.PaleGreen;
		Main.NewText(Language.GetTextValue("Lantern Moon is raising..."), messageColor1);
		Main.NewText(Language.GetTextValue("Wave 1:"), messageColor);
		Wave = 0;
		AccumulatedScore = 0;
	}

	public void AddPoint(int value)
	{
		if (Wave == 14 || Wave == 24)
		{
			return;
		}
		int mulValue = 1;
		if (Main.expertMode)
		{
			mulValue = 2;
		}
		if (Main.masterMode)
		{
			mulValue = 3;
		}
		AccumulatedScore += value * mulValue;
	}

	public void NewWave()
	{
		Main.NewText("Wave " + (Wave + 2) + ":", new Color(175, 75, 255));
		Wave++;
	}

	/// <summary>
	/// A Lantern Ghost King.
	/// </summary>
	public NPC Wave15Boss;

	public void UpdateWave15()
	{
		if (Wave15Boss == null || !Wave15Boss.active)
		{
			int x0 = (int)(Main.screenPosition.X - Main.offScreenRange - 150);
			int x1 = (int)(Main.screenPosition.X + Main.screenWidth + Main.offScreenRange + 150);
			int y0 = (int)(Main.screenPosition.Y + Main.screenHeight * 0.5f);

			Wave15Boss = NPC.NewNPCDirect(NPC.GetSource_NaturalSpawn(), Main.rand.NextBool(2) ? x0 : x1, y0, ModContent.NPCType<LanternGhostKing>());
		}
		else
		{
			int waveRequire = ScoreRequireOfWave[Wave];
			if (Wave15Boss.active)
			{
				AccumulatedScore = 10000 + waveRequire - (int)(Wave15Boss.life / (float)Wave15Boss.lifeMax * waveRequire);
			}
		}
	}
	public override void OnActivate(params object[] args)
	{
		Initialization();
	}

	public override void Update()
	{
		AddEnemies();
		if (Wave == 0)
		{
			AccumulatedScore = 0;
		}

		if (WaveScore() > ScoreRequireOfWave[Wave])
		{
			NewWave();
		}
		if (Wave == 14)
		{
			UpdateWave15();
		}
		if (Main.dayTime)
		{
			EventSystem.Deactivate(this);
			Main.invasionProgressMode = 0;
		}
	}

	public void AddEnemies()
	{
		if (Wave < 14)
		{
			if (NPC.CountNPCS(ModContent.NPCType<EvilLantern>()) < 15)
			{
				if (Main.rand.NextBool(20))
				{
					int x0 = (int)(Main.screenPosition.X - Main.offScreenRange - 150);
					int x1 = (int)(Main.screenPosition.X + Main.screenWidth + Main.offScreenRange + 150);
					int y0 = (int)(Main.screenPosition.Y + Main.screenHeight * 0.5f);
					NPC.NewNPC(NPC.GetSource_NaturalSpawn(), Main.rand.NextBool(2) ? x0 : x1, y0, ModContent.NPCType<EvilLantern>());
				}
			}
			if (Wave >= 5)
			{
				if (NPC.CountNPCS(ModContent.NPCType<BombLantern>()) < 15)
				{
					if (Main.rand.NextBool(20))
					{
						int x0 = (int)(Main.screenPosition.X - Main.offScreenRange - 150);
						int x1 = (int)(Main.screenPosition.X + Main.screenWidth + Main.offScreenRange + 150);
						int y0 = (int)(Main.screenPosition.Y + Main.screenHeight * 0.5f);
						NPC.NewNPC(NPC.GetSource_NaturalSpawn(), Main.rand.NextBool(2) ? x0 : x1, y0, ModContent.NPCType<BombLantern>());
					}
				}
			}
			if (Wave >= 8)
			{
				if (NPC.CountNPCS(ModContent.NPCType<CylindricalLantern>()) < 15)
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
		}
	}

	public override void Draw(SpriteBatch sprite)
	{
		DrawProgressBar(sprite);
	}

	public void DrawProgressBar(SpriteBatch spriteBatch)
	{
		Main.invasionProgressMax = ScoreRequireOfWave[Wave];
		Main.invasionProgress = WaveScore();
		Main.invasionProgressMode = 2;
		Main.invasionProgressNearInvasion = true;
		Main.invasionProgressWave = Wave + 1;

		if (Main.invasionProgressMode == 2 && Main.invasionProgressNearInvasion && Main.invasionProgressDisplayLeft < 160)
		{
			Main.invasionProgressDisplayLeft = 160;
		}

		if (!Main.gamePaused && Main.invasionProgressDisplayLeft > 0)
		{
			Main.invasionProgressDisplayLeft--;
		}

		if (Main.invasionProgressDisplayLeft > 0)
		{
			Main.invasionProgressAlpha += 0.005f;
		}
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
		{
			return;
		}

		float fadinAlpha = 0.5f + Main.invasionProgressAlpha * 0.5f;
		Texture2D lanternMoonImage = ModAsset.LanternMoonIcon.Value;

		// TODO Hjson，示例我在WorldSystem里写了
		string LanternMoon = " Lantern Moon ";
		Color BGColor = Color.White;
		int barSizeX = (int)(200f * fadinAlpha);
		int barSizeY = (int)(45f * fadinAlpha);
		var barCenter = new Vector2(Main.screenWidth - 120, Main.screenHeight - 40);
		string waveStage = Main.invasionProgressMax == 0 ?
			WaveScore().ToString() :
			(int)(Main.invasionProgress * 100f / Main.invasionProgressMax) + "%";
		float textSizeX = 169f * fadinAlpha;
		float textSizeY = 8f * fadinAlpha;
		Texture2D colorBar = TextureAssets.ColorBar.Value;
		if (Main.invasionProgressWave > 0)
		{
			waveStage = Language.GetTextValue("Game.WaveMessage", Main.invasionProgressWave, waveStage);
		}
		else
		{
			waveStage = Language.GetTextValue("Game.WaveCleared", waveStage);
		}

		float waveRate = Main.invasionProgressMax == 0 ? 1 :
				MathHelper.Clamp(Main.invasionProgress / (float)Main.invasionProgressMax, 0f, 1f);

		Vector2 textPosition = barCenter + Vector2.UnitY * textSizeY + Vector2.UnitX * 1f;

		// Progress Bar Background
		Utils.DrawInvBG(
			spriteBatch,
			new Rectangle((int)barCenter.X - barSizeX / 2, (int)barCenter.Y - barSizeY / 2, barSizeX, barSizeY),
			new Color(63 * 0.785f, 65 * 0.785f, 151 * 0.785f, 255 * 0.785f));

		// Progress Bar Text
		Utils.DrawBorderString(spriteBatch, waveStage, textPosition, Color.White * Main.invasionProgressAlpha, fadinAlpha, 0.5f, 1f, -1);

		// Progress Bar Outline
		spriteBatch.Draw(colorBar, barCenter, null, Color.White * Main.invasionProgressAlpha, 0f, new Vector2(colorBar.Width / 2, 0f), fadinAlpha, SpriteEffects.None, 0f);

		textPosition += Vector2.UnitX * (waveRate - 0.5f) * textSizeX;

		// Progress Bar Value Bar
		spriteBatch.Draw(
			TextureAssets.MagicPixel.Value,
			textPosition,
			new Rectangle(0, 0, 1, 1),
			new Color(255, 241, 51) * Main.invasionProgressAlpha,
			0f,
			new Vector2(1f, 0.5f),
			new Vector2(textSizeX * waveRate, textSizeY),
			SpriteEffects.None,
			0f);
		spriteBatch.Draw(
			TextureAssets.MagicPixel.Value,
			textPosition,
			new Rectangle(0, 0, 1, 1),
			new Color(255, 165, 0, 127) * Main.invasionProgressAlpha,
			0f,
			new Vector2(1f, 0.5f),
			new Vector2(2f, textSizeY),
			SpriteEffects.None,
			0f);
		spriteBatch.Draw(
			TextureAssets.MagicPixel.Value,
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
		{
			OffsetX += lanternMoonSize.X - 200f;
		}

		Rectangle BGRectangle = Utils.CenteredRectangle(
			new Vector2(Main.screenWidth - OffsetX, Main.screenHeight - 80),
			(lanternMoonSize + new Vector2(lanternMoonImage.Width + 12, 6f)) * fadinAlpha);

		// Lantern Moon Icon
		Utils.DrawInvBG(
			spriteBatch,
			BGRectangle,
			BGColor);

		spriteBatch.Draw(
			lanternMoonImage,
			BGRectangle.Left() + Vector2.UnitX * fadinAlpha * 8f,
			null,
			Color.White * Main.invasionProgressAlpha,
			0f,
			new Vector2(0f, lanternMoonImage.Height / 2),
			fadinAlpha * 0.8f,
			SpriteEffects.None,
			0f);

		// 灯笼月文字
		Utils.DrawBorderString(
			spriteBatch,
			LanternMoon,
			BGRectangle.Right() + Vector2.UnitX * fadinAlpha * -22f,
			Color.White * Main.invasionProgressAlpha,
			fadinAlpha * 0.9f,
			1f,
			0.4f,
			-1);
	}
}