using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.KitchenSystem;

public class ScoreChange
{
	public Vector2 AnchorPos;
	public int TimeLeft;
	public int MaxTime;
	public int Value;

	public ScoreChange(int value)
	{
		Value = value;
		TimeLeft = 50;
		MaxTime = 50;
	}

	public void Draw()
	{
		string drawValue = Value.ToString();
		if (Value > 0)
		{
			drawValue = "+" + drawValue;
		}
		Vector2 textSize = ChatManager.GetStringSize(FontAssets.MouseText.Value, drawValue, Vector2.One);
		Color color = new Color(1f, 1f, 0.5f);
		if (Value < 0)
		{
			color = new Color(1f, 0f, 0.3f);
		}
		if (TimeLeft < 30)
		{
			color *= TimeLeft / 30f;
		}
		Main.spriteBatch.DrawString(FontAssets.MouseText.Value, drawValue, AnchorPos, color, 0, textSize * 0.5f, 1, SpriteEffects.None, 0);
	}

	public void Update()
	{
		TimeLeft -= 1;
		if (TimeLeft <= 0)
		{
			TimeLeft = 0;
		}
		if (TimeLeft < 30)
		{
			AnchorPos.Y -= 1f;
		}
		else
		{
			AnchorPos = KitchenSystemUI.MainPanelOrigin + new Vector2(-310, 200);
		}
	}
}