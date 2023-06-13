using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Events
{
	public abstract class ReplicaEvent : ModEvent
	{
		public int Progress;
		public int ProgressMax;
		public float ProgressAlpha;
		public int Timer;
		public int Wave;
		/// <summary>
		/// 默认加载同路径的图片
		/// <br>图片尺寸要求:32*32</br>
		/// </summary>
		public Texture2D Texture { get; protected set; }
		protected bool InnerActive;
		public override void OnStart(params object[] args)
		{
			InnerActive = true;
			Progress = ProgressMax = Timer = Wave = 0;
			ProgressAlpha = 0;
		}
		public override void OnStop(params object[] args)
		{
			InnerActive = false;
		}
		public virtual void ModifyInvasionProgress(ref string text, ref Color c) { }
		public virtual void ReportProgress() { }
		public override void Draw(SpriteBatch sprite)
		{
			if (InnerActive)
			{
				Timer = 160;
			}
			if (!Main.gamePaused && Timer > 0)
			{
				Timer--;
			}
			ProgressAlpha = Math.Clamp(ProgressAlpha + Timer > 0 ? 0.05f : -0.05f, 0, 1);
			if (!InnerActive && ProgressAlpha <= 0f)
			{
				EventManager.Stop(this);
				return;
			}
			float num = 0.5f + ProgressAlpha * 0.5f;
			string text = "";
			Color c = Color.White;
			Texture2D value = Texture ??= ModContent.Request<Texture2D>(FullName, AssetRequestMode.ImmediateLoad).Value;
			ModifyInvasionProgress(ref text, ref c);
			ReportProgress();
			Vector2 texturescale = new(25.6f / value.Width, 25.6f / value.Height);
			texturescale *= num;
			if (Wave > 0)
			{
				int num2 = (int)(200f * num);
				int num3 = (int)(45f * num);
				Vector2 vector = new(Main.screenWidth - 120, Main.screenHeight - 40);
				Rectangle r4 = new((int)vector.X - num2 / 2, (int)vector.Y - num3 / 2, num2, num3);
				Utils.DrawInvBG(Main.spriteBatch, r4, new Color(63, 65, 151, 255) * 0.785f);
				string key = "Game.WaveMessage";
				object arg = (ProgressMax != 0) ? ((float)Progress / ProgressMax).ToString("#0.0#%") : Language.GetTextValue("Game.InvasionPoints", Progress);
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
				Utils.DrawBorderString(Main.spriteBatch, text2, vector2, Color.White * ProgressAlpha, num, 0.5f, 1f, -1);
				Main.spriteBatch.Draw(value2, vector, null, Color.White * ProgressAlpha, 0f, new Vector2(value2.Width / 2, 0f), num, SpriteEffects.None, 0f);
				vector2 += Vector2.UnitX * (num4 - 0.5f) * num5;
				Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 241, 51) * ProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(num5 * num4, num6), SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 165, 0, 127) * ProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num6), SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector2, new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * ProgressAlpha, 0f, new Vector2(0f, 0.5f), new Vector2(num5 * (1f - num4), num6), SpriteEffects.None, 0f);
			}
			else
			{
				int num7 = (int)(200f * num);
				int num8 = (int)(45f * num);
				Vector2 vector3 = new(Main.screenWidth - 120, Main.screenHeight - 40);
				Rectangle r4 = new((int)vector3.X - num7 / 2, (int)vector3.Y - num8 / 2, num7, num8);
				Utils.DrawInvBG(Main.spriteBatch, r4, new Color(63, 65, 151, 255) * 0.785f);
				string text3 = (ProgressMax != 0) ? ((float)Progress / ProgressMax).ToString("#0.0#%") : Progress.ToString();
				text3 = Language.GetTextValue("Game.WaveCleared", text3);
				Texture2D value3 = TextureAssets.ColorBar.Value;
				if (ProgressMax != 0)
				{
					Main.spriteBatch.Draw(value3, vector3, null, Color.White * ProgressAlpha, 0f, new Vector2(value3.Width / 2, 0f), num, SpriteEffects.None, 0f);
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
					Utils.DrawBorderString(Main.spriteBatch, text3, vector5 + new Vector2(0f, -4f), Color.White * ProgressAlpha, num10, 0.5f, 1f, -1);
					vector5 += Vector2.UnitX * (num9 - 0.5f) * num11;
					Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 241, 51) * ProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(num11 * num9, num12), SpriteEffects.None, 0f);
					Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle?(new Rectangle(0, 0, 1, 1)), new Color(255, 165, 0, 127) * ProgressAlpha, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num12), SpriteEffects.None, 0f);
					Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, vector5, new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * ProgressAlpha, 0f, new Vector2(0f, 0.5f), new Vector2(num11 * (1f - num9), num12), SpriteEffects.None, 0f);
				}
			}
			Vector2 value4 = FontAssets.MouseText.Value.MeasureString(text);
			float num13 = 120f;
			if (value4.X > 200f)
			{
				num13 += value4.X - 200f;
			}
			Rectangle r3 = Utils.CenteredRectangle(new Vector2(Main.screenWidth - num13, Main.screenHeight - 80), (value4 + new Vector2(value.Width + 12, 6f)) * num);
			Utils.DrawInvBG(Main.spriteBatch, r3, c);
			Main.spriteBatch.Draw(value, r3.Left() + Vector2.UnitX * num * 8f, null, Color.White * ProgressAlpha, 0f, new Vector2(0f, value.Height / 2), texturescale, SpriteEffects.None, 0f);
			Utils.DrawBorderString(Main.spriteBatch, text, r3.Right() + Vector2.UnitX * num * -22f, Color.White * ProgressAlpha, num * 0.9f, 1f, 0.4f, -1);
		}
		public override void SaveData(TagCompound tag)
		{
			tag[nameof(Progress)] = Progress;
			tag[nameof(ProgressMax)] = ProgressMax;
			tag[nameof(ProgressAlpha)] = ProgressAlpha;
			tag[nameof(Timer)] = Timer;
			tag[nameof(Wave)] = Wave;
		}
		public override void LoadData(TagCompound tag)
		{
			if(Active)
			{
				InnerActive = true;
			}
			tag.TryGet(nameof(Progress), out Progress);
			tag.TryGet(nameof(ProgressMax), out ProgressMax);
			tag.TryGet(nameof(ProgressAlpha),out ProgressAlpha);
			tag.TryGet(nameof(Timer), out Timer);
			tag.TryGet(nameof(Wave), out Wave);
		}
	}
}
