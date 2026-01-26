using Everglow.Commons.DataStructures;
using Everglow.Commons.Mechanics.Events;
using Everglow.Myth.LanternMoon.NPCs;
using Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;
using Terraria.GameContent;
using Terraria.Localization;

namespace Everglow.Myth.LanternMoon.LanternCommon;

public class LanternMoonInvasionEvent : ReplicaEvent
{
	/// <summary>
	/// Point value for completing the current wave
	/// </summary>
	public int[] ScoreRequireOfWave = new int[40];

	public List<int>[] WaveEnemiesType = new List<int>[40];

	/// <summary>
	/// Total point account from the beginning.
	/// </summary>
	public float AccumulatedScore = 0;

	/// <summary>
	/// A Lantern Ghost King.
	/// </summary>
	public NPC Wave15Boss;

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

		WaveEnemiesType[0] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[1] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[2] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<CylindricalLantern>() };
		WaveEnemiesType[3] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<CylindricalLantern>() };
		WaveEnemiesType[4] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<CylindricalLantern>(), ModContent.NPCType<BombLantern>() };
		WaveEnemiesType[5] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<CylindricalLantern>(), ModContent.NPCType<BombLantern>() };
		WaveEnemiesType[6] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<CylindricalLantern>(), ModContent.NPCType<BombLantern>() };
		WaveEnemiesType[7] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<CylindricalLantern>(), ModContent.NPCType<BombLantern>() };
		WaveEnemiesType[8] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<BombLantern>(), ModContent.NPCType<CylindricalLantern>() };
		WaveEnemiesType[9] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<BombLantern>(), ModContent.NPCType<CylindricalLantern>() };
		WaveEnemiesType[10] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<BombLantern>(), ModContent.NPCType<CylindricalLantern>() };
		WaveEnemiesType[11] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<BombLantern>(), ModContent.NPCType<CylindricalLantern>() };
		WaveEnemiesType[12] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<BombLantern>(), ModContent.NPCType<CylindricalLantern>() };
		WaveEnemiesType[13] = new List<int>() { ModContent.NPCType<EvilLantern>(), ModContent.NPCType<BombLantern>(), ModContent.NPCType<CylindricalLantern>() };
		WaveEnemiesType[14] = new List<int>() { ModContent.NPCType<LanternGhostKing>() };
		WaveEnemiesType[15] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[16] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[17] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[18] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[19] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[20] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[21] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[22] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[23] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[24] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[25] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[26] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[27] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[28] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[29] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[30] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[31] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[32] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[33] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[34] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[35] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[36] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[37] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[38] = new List<int>() { ModContent.NPCType<EvilLantern>() };
		WaveEnemiesType[39] = new List<int>() { ModContent.NPCType<EvilLantern>() };

		var messageColor = new Color(175, 75, 255);
		Color messageColor1 = Color.PaleGreen;
		Main.NewText(Language.GetTextValue("Lantern Moon is raising..."), messageColor1);
		Wave = 1;
		Main.NewText(Language.GetTextValue("Wave 1:" + GetWaveEnemiesMessage()), messageColor);
		ProgressMax = ScoreRequireOfWave[0];
		AccumulatedScore = 0;
		Icon = ModAsset.LanternMoonIcon.Value;
	}

	public override void OnActivate(params object[] args)
	{
		Initialization();
		base.OnActivate();
	}

	public bool ShouldReinitialize()
	{
		bool flag0 = Wave == 0 || ScoreRequireOfWave[0] == 0 || Icon == null;
		bool flag1 = Active;
		return flag0 && flag1;
	}

	public override void Update()
	{
		if(Main.dayTime)
		{
			innerActive = false;
		}
		if (ShouldReinitialize())
		{
			Initialization();
		}
		Progress = WaveScore();
		if (Progress > ScoreRequireOfWave[Wave - 1])
		{
			NewWave();
		}
		if (Wave == 15)
		{
			UpdateWave15();
		}
	}

	public override void ModifyInvasionProgress(ref string text, ref Color c)
	{
		text = "Lantern Moon";
		base.ModifyInvasionProgress(ref text, ref c);
	}

	public string GetWaveEnemiesMessage()
	{
		if (WaveEnemiesType[Wave - 1] is not null)
		{
			string message = string.Empty;
			foreach (var npcType in WaveEnemiesType[Wave - 1])
			{
				string npcName = Lang.GetNPCNameValue(npcType);
				if (message != string.Empty)
				{
					message += ", ";
				}
				message += npcName;
			}
			return message;
		}
		return string.Empty;
	}

	public float WaveScore()
	{
		float score = AccumulatedScore - ScoreRequireOfWave.Take(Wave - 1).Sum();
		return score;
	}

	public void AddPoint(float value)
	{
		if (Wave == 15 || Wave == 25)
		{
			return;
		}
		float mulValue = 1;
		if (Main.expertMode)
		{
			mulValue = 2;
		}
		if (Main.masterMode)
		{
			mulValue = 2.5f;
		}
		AccumulatedScore += value * mulValue;
	}

	public void NewWave()
	{
		Main.NewText("Wave " + (Wave + 1) + ":" + GetWaveEnemiesMessage(), new Color(175, 75, 255));
		Wave++;
		ProgressMax = ScoreRequireOfWave[Wave - 1];
	}

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
			if (Wave15Boss.type != ModContent.NPCType<LanternGhostKing>() || !Wave15Boss.active)
			{
				foreach (var npc in Main.npc)
				{
					if (npc != null && npc.active && npc.type == ModContent.NPCType<LanternGhostKing>())
					{
						Wave15Boss = npc;
						break;
					}
				}
			}
			else
			{
				int waveRequire = ScoreRequireOfWave[Wave - 1];
				if (Wave15Boss.active)
				{
					AccumulatedScore = ScoreRequireOfWave.Take(14).Sum() + waveRequire - (int)(Wave15Boss.life / (float)Wave15Boss.lifeMax * waveRequire);
				}
			}
		}
	}

	public override void Draw(SpriteBatch sprite)
	{
		if (ShouldReinitialize())
		{
			Initialization();
		}
		DrawProgressBar(sprite);
	}

	public void DrawProgressBar(SpriteBatch spriteBatch)
	{
		SpriteBatchState sBS = spriteBatch.GetState().Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
		if (innerActive)
		{
			UIBarFadeTimer = 160;
		}
		if (!Main.gamePaused && UIBarFadeTimer > 0)
		{
			UIBarFadeTimer--;
		}
		ProgressAlpha += UIBarFadeTimer > 0 ? 0.05f : -0.05f;
		ProgressAlpha = Math.Clamp(ProgressAlpha, 0, 1);
		if (ProgressAlpha <= 0f)
		{
			EventSystem.Deactivate(this);
			return;
		}
		float num = 0.5f + ProgressAlpha * 0.5f;
		string text = string.Empty;
		Color c = Color.White;

		ModifyInvasionProgress(ref text, ref c);
		Vector2 texturescale = new(25.6f / Icon.Width, 25.6f / Icon.Height);
		texturescale *= num;
		if (Wave > 0)
		{
			int num2 = (int)(200f * num);
			int num3 = (int)(45f * num);
			Vector2 vector = new Vector2(Main.screenWidth - 120, Main.screenHeight - 40);
			Rectangle r4 = new((int)vector.X - num2 / 2, (int)vector.Y - num3 / 2, num2, num3);
			Utils.DrawInvBG(spriteBatch, r4, new Color(63, 65, 151, 255) * 0.785f);
			string key = "Game.WaveMessage";
			object arg = ProgressMax != 0 ? ((float)Progress / ProgressMax).ToString("##.##%") : Language.GetTextValue("Game.InvasionPoints", Progress);
			string text2 = Language.GetTextValue(key, Wave, arg);
			Texture2D value2 = TextureAssets.ColorBar.Value;
			float num4 = MathHelper.Clamp(Progress / (float)ProgressMax, 0f, 1f);
			if (ProgressMax == 0)
			{
				num4 = 1f;
			}
			float num5 = 169f * num;
			float num6 = 8f * num;
			Vector2 vector2 = vector + Vector2.UnitY * num6 + Vector2.UnitX * 1f;
			Utils.DrawBorderString(spriteBatch, text2, vector2, Color.White * ProgressAlpha, num, 0.5f, 1f, -1);
			spriteBatch.Draw(value2, vector, null, Color.White * ProgressAlpha, 0f, new Vector2(value2.Width / 2, 0f), num, SpriteEffects.None, 0f);
			vector2 += Vector2.UnitX * (num4 - 0.5f) * num5;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 241, 51) * ProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(num5 * num4, num6), SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 165, 0, 127) * ProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num6), SpriteEffects.None, 0f);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * ProgressAlpha, 0f, new Vector2(0f, 0.5f), new Vector2(num5 * (1f - num4), num6), SpriteEffects.None, 0f);
		}
		else
		{
			int num7 = (int)(200f * num);
			int num8 = (int)(45f * num);
			Vector2 vector3 = new(Main.screenWidth - 120, Main.screenHeight - 40);
			Rectangle r4 = new((int)vector3.X - num7 / 2, (int)vector3.Y - num8 / 2, num7, num8);
			Utils.DrawInvBG(spriteBatch, r4, new Color(63, 65, 151, 255) * 0.785f);
			string text3 = ProgressMax != 0 ? ((float)Progress / ProgressMax).ToString("##.##%") : Progress.ToString();
			text3 = Language.GetTextValue("Game.WaveCleared", text3);
			Texture2D value3 = TextureAssets.ColorBar.Value;
			if (ProgressMax != 0)
			{
				spriteBatch.Draw(value3, vector3, null, Color.White * ProgressAlpha, 0f, new Vector2(value3.Width / 2, 0f), num, SpriteEffects.None, 0f);
				float num9 = MathHelper.Clamp(Progress / (float)ProgressMax, 0f, 1f);
				Vector2 vector4 = FontAssets.MouseText.Value.MeasureString(text3);
				float num10 = num;
				if (vector4.Y > 22f)
				{
					num10 *= 22f / vector4.Y;
				}
				float num11 = 169f * num;
				float num12 = 8f * num;
				Vector2 vector5 = vector3 + Vector2.UnitY * num12 + Vector2.UnitX * 1f;
				Utils.DrawBorderString(spriteBatch, text3, vector5 + new Vector2(0f, -4f), Color.White * ProgressAlpha, num10, 0.5f, 1f, -1);
				vector5 += Vector2.UnitX * (num9 - 0.5f) * num11;
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 241, 51) * ProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(num11 * num9, num12), SpriteEffects.None, 0f);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 165, 0, 127) * ProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num12), SpriteEffects.None, 0f);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * ProgressAlpha, 0f, new Vector2(0f, 0.5f), new Vector2(num11 * (1f - num9), num12), SpriteEffects.None, 0f);
			}
		}
		Vector2 value4 = FontAssets.MouseText.Value.MeasureString(text);
		float num13 = 120f;
		if (value4.X > 200f)
		{
			num13 += value4.X - 200f;
		}
		Rectangle r3 = Utils.CenteredRectangle(new Vector2(Main.screenWidth - num13, Main.screenHeight - 80), (value4 + new Vector2(Icon.Width + 12, 6f)) * num);
		Utils.DrawInvBG(spriteBatch, r3, c);
		spriteBatch.Draw(Icon, r3.Left() + Vector2.UnitX * num * 8f, null, Color.White * ProgressAlpha, 0f, new Vector2(0f, Icon.Height / 2), texturescale, SpriteEffects.None, 0f);
		Utils.DrawBorderString(spriteBatch, text, r3.Right() + Vector2.UnitX * num * -22f, Color.White * ProgressAlpha, num * 0.9f, 1f, 0.4f, -1);

		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}
}