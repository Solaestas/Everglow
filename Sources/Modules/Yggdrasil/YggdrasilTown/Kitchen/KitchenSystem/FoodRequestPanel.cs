using Everglow.Yggdrasil.YggdrasilTown.Kitchen.Tiles;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;
using static Everglow.Yggdrasil.YggdrasilTown.Kitchen.KitchenSystem.KitchenSystemUI;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.KitchenSystem;

public class FoodRequestPanel
{
	public Vector2 AnchorPos;
	public int TimeLeft;
	public int MaxTime;
	public int FoodType;
	public int Value;

	/// <summary>
	/// 0: normal, 1 success, 2 fail, 3 urge
	/// </summary>
	public int State;
	public bool Active;
	public bool IsMouseOverCancelButtom;
	public float Alpha;

	public FoodRequestPanel(int foodType, int maxTime, int value)
	{
		FoodType = foodType;
		TimeLeft = maxTime;
		MaxTime = maxTime;
		Value = value;
		Active = true;
		IsMouseOverCancelButtom = false;
		State = 0;
		Alpha = 0;
	}

	public virtual void Draw()
	{
		Color panelColor = new Color(0.4f, 0.3f, 0.2f);
		if (State == 1)
		{
			panelColor = new Color(0.1f, 0.8f, 0.3f);
		}
		if (State == 2)
		{
			panelColor = new Color(0.8f, 0.0f, 0.2f);
		}
		Vector2 drawPos = MainPanelOrigin + AnchorPos;
		Draw9Pieces(drawPos, 60, 120, panelColor, Alpha);

		// Timer bar
		Draw9Pieces(drawPos + new Vector2(0, 50), 46, 8, new Color(0.3f, 0.3f, 0.6f), Alpha);
		Draw9Pieces(drawPos + new Vector2(0, 50), 44, 6, new Color(0f, 0f, 0f), Alpha);
		float timeValue = TimeLeft / (float)MaxTime;

		Texture2D timer = ModAsset.FoodRequestUIPanelTimer.Value;
		Main.spriteBatch.Draw(timer, drawPos + new Vector2(0, 50), new Rectangle(0, 0, (int)(timer.Width * timeValue), timer.Height), Color.Lerp(new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), timeValue) * 0.9f, 0, timer.Size() * 0.5f, new Vector2(0.7f, 1.6f), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(timer, drawPos + new Vector2(96 * timeValue, 50), new Rectangle((int)(timer.Width * timeValue), 0, 2, timer.Height), Color.Lerp(new Color(1f, 0f, 0f), new Color(0f, 1f, 0f), timeValue) * 3, 0, timer.Size() * 0.5f, new Vector2(0.7f, 0.8f), SpriteEffects.None, 0);
		Draw9Pieces(drawPos + new Vector2(0, -40), 48, 48, new Color(0.2f, 0.1f, 0.15f), Alpha);

		// Item slot
		Item item = new Item(FoodType);
		Color color = Color.White * (1 - Alpha);
		Texture2D food = TextureAssets.Item[FoodType].Value;
		Rectangle frame = (Main.itemAnimations[FoodType] == null) ? food.Frame() : Main.itemAnimations[FoodType].GetFrame(food);
		float scale;
		ItemSlot.DrawItem_GetColorAndScale(item, 1, ref color, 20000, ref frame, out color, out scale);
		Main.spriteBatch.Draw(food, drawPos + new Vector2(0, -40), frame, color, 0f, new Vector2(frame.Width, frame.Height) * 0.5f, scale, SpriteEffects.None, 0f);

		// Value display
		Vector2 textSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, Value.ToString(), Vector2.One);
		Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Value.ToString(), drawPos + new Vector2(0, 30), color, 0, textSize * 0.5f, 1, SpriteEffects.None, 0);

		// Cancel button
		Color buttomColor = new Color(0.5f, 0.5f, 0.5f);
		Color textColor = new Color(0.8f, 0.8f, 0.8f);
		if (IsMouseOverCancelButtom)
		{
			buttomColor = new Color(0.2f, 0.1f, 0.1f);
			textColor = new Color(1f, 0.1f, 0.1f);
		}
		Draw9Pieces(drawPos + new Vector2(0, 90), 40, 14, buttomColor, Alpha);
		textSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, "Cancel", Vector2.One);
		Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "Cancel", drawPos + new Vector2(0, 94), textColor, 0, textSize * 0.5f, 1, SpriteEffects.None, 0);
	}

	public void Update(int index)
	{
		// Timer
		if (Active)
		{
			TimeLeft -= 1;
			if (TimeLeft <= 0)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				TimeLeft = 0;
				Active = false;
				AddScore(-Value);
				State = 2;
			}
			else
			{
				AnchorPos = Vector2.Lerp(AnchorPos, new Vector2(index * 160 - 330, 0), 0.2f);
			}
		}
		if (!Active)
		{
			Alpha += 0.1f;
			AnchorPos.Y -= Alpha * 10;
		}

		// Allow cancel manually
		Rectangle cancelBox = new Rectangle((int)(AnchorPos + MainPanelOrigin + new Vector2(0, 90)).X - 40, (int)(AnchorPos + MainPanelOrigin + new Vector2(0, 90)).Y - 14, 80, 20);
		if (cancelBox.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y))
		{
			if (!IsMouseOverCancelButtom)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
			}
			IsMouseOverCancelButtom = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				Active = false;
				State = 2;
				AddScore(-(int)(Value * (1 - (float)TimeLeft / MaxTime)));
			}
		}
		else
		{
			IsMouseOverCancelButtom = false;
		}
	}

	public void CheckFinish()
	{
		if (Active)
		{
			foreach (var item in Main.item)
			{
				if (item.active && item.type == FoodType && (item.Center - Main.LocalPlayer.Center).Length() < 3000)
				{
					for (int j = 0; j < 3; j++)
					{
						Point point = item.Center.ToTileCoordinates();
						if (Main.tile[point + new Point(0, j)].TileType == ModContent.TileType<ServingCounter_ChineseStyle>())
						{
							if (item.stack >= 1)
							{
								item.stack--;
							}
							else
							{
								item.active = false;
							}
							SoundEngine.PlaySound(SoundID.Item35);
							AddScore(Value);
							Active = false;
							State = 1;
							return;
						}
					}
				}
			}
		}
	}

	public void CheckFinishInventory()
	{
		if (Active)
		{
			foreach (var item in Main.LocalPlayer.inventory)
			{
				if (item.active && item.type == FoodType)
				{
					if (item.stack >= 1)
					{
						item.stack--;
					}
					else
					{
						item.active = false;
					}
					SoundEngine.PlaySound(SoundID.Item35);
					AddScore(Value);
					Active = false;
					State = 1;
					return;
				}
			}
		}
	}
}