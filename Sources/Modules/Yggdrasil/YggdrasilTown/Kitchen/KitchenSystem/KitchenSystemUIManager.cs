using Everglow.Food.Items.ModFood;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.UI;

namespace Everglow.Yggdrasil.YggdrasilTown.Kitchen.KitchenSystem;

public class KitchenSystemUI : GameInterfaceLayer
{
	public static List<FoodRequestPanel> FoodRequests = new List<FoodRequestPanel>();
	public static List<ScoreChange> ScoreChangeList = new List<ScoreChange>();

	public KitchenSystemUI(string name, InterfaceScaleType scaleType)
		: base(name, scaleType)
	{
	}

	public static int Score = 0;
	public static bool Pause = false;
	public static Vector2 MainPanelOrigin;
	public static Vector2 PauseContinuePanelOffset;
	public static Vector2 DragPanelOffset;
	public static Vector2 RestartPanelOffset;
	public Vector2 DragStartMousePos;
	public Vector2 DragStartPanelPos;
	public static bool IsMouseOverPauseContinue;
	public static bool IsMouseOverDrag;
	public static bool IsMouseOverRestart;
	public bool IsDragging;

	public static void Reset()
	{
		FoodRequests = new List<FoodRequestPanel>();
		ScoreChangeList = new List<ScoreChange>();
		Score = 0;
		MainPanelOrigin = new Vector2(530, Main.screenHeight - 300);
		Pause = true;
	}

	public static void Restart()
	{
		FoodRequests = new List<FoodRequestPanel>();
		ScoreChangeList = new List<ScoreChange>();
		Score = 0;
		Pause = true;
	}

	public override bool DrawSelf()
	{
		if (!Main.gamePaused)
		{
			Update();
		}

		Draw9Pieces(MainPanelOrigin, 400, 160, new Color(0.3f, 0.2f, 0.2f), 0);
		foreach (var foodRequest in FoodRequests)
		{
			foodRequest.Draw();
		}
		DrawScoreBar();
		for (int index = ScoreChangeList.Count - 1; index >= 0; index--)
		{
			ScoreChange scoreChange = ScoreChangeList[index];
			scoreChange.Draw();
		}
		Color defaulePanel = new Color(0.3f, 0.2f, 0.2f);
		if (IsMouseOverPauseContinue)
		{
			defaulePanel = new Color(0.1f, 0.05f, 0.04f);
		}
		Draw9Pieces(PauseContinuePanelOffset, 24, 24, defaulePanel, 0);
		Texture2D icons = ModAsset.FoodRequestUIPanelIcons.Value;
		Rectangle pauseFrame = new Rectangle(0, 0, 13, 13);
		Rectangle continueFrame = new Rectangle(0, 13, 13, 13);
		Main.spriteBatch.Draw(icons, PauseContinuePanelOffset, Pause ? continueFrame : pauseFrame, Color.AliceBlue, 0, pauseFrame.Size() * 0.5f, 2f, SpriteEffects.None, 0);

		defaulePanel = new Color(0.3f, 0.2f, 0.2f);
		if (IsMouseOverDrag)
		{
			defaulePanel = new Color(0.1f, 0.05f, 0.04f);
		}
		Draw9Pieces(DragPanelOffset, 24, 24, defaulePanel, 0);
		Rectangle dragFrame = new Rectangle(0, 39, 13, 13);
		Main.spriteBatch.Draw(icons, DragPanelOffset, dragFrame, Color.AliceBlue, 0, dragFrame.Size() * 0.5f, 2f, SpriteEffects.None, 0);

		defaulePanel = new Color(0.3f, 0.2f, 0.2f);
		if (IsMouseOverRestart)
		{
			defaulePanel = new Color(0.1f, 0.05f, 0.04f);
		}
		Draw9Pieces(RestartPanelOffset, 24, 24, defaulePanel, 0);
		Rectangle resetFrame = new Rectangle(0, 26, 13, 13);
		Main.spriteBatch.Draw(icons, RestartPanelOffset, resetFrame, Color.AliceBlue, 0, resetFrame.Size() * 0.5f, 2f, SpriteEffects.None, 0);
		return true;
	}

	public void Update()
	{
		GetOffsets();
		UpdateFoodRequest();
		UpdateScore();
		PanelCollisonCheck();
		CheckPosValid();
	}

	public void CheckPosValid()
	{
		if (MainPanelOrigin.X > Main.screenWidth - 200)
		{
			MainPanelOrigin.X = Main.screenWidth - 200;
		}
		if (MainPanelOrigin.X < 200)
		{
			MainPanelOrigin.X = 200;
		}
		if (MainPanelOrigin.Y > Main.screenHeight - 100)
		{
			MainPanelOrigin.Y = Main.screenHeight - 100;
		}
		if (MainPanelOrigin.Y < 100)
		{
			MainPanelOrigin.Y = 100;
		}
	}

	public void PanelCollisonCheck()
	{
		// Pause to taking new orders
		Rectangle pauseBox = new Rectangle((int)PauseContinuePanelOffset.X - 20, (int)PauseContinuePanelOffset.Y - 20, 40, 40);
		if (pauseBox.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y))
		{
			if (!IsMouseOverPauseContinue)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
			}
			IsMouseOverPauseContinue = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				Pause = !Pause;
			}
		}
		else
		{
			IsMouseOverPauseContinue = false;
		}

		// Drag the main panel
		Rectangle dragBox = new Rectangle((int)DragPanelOffset.X - 20, (int)DragPanelOffset.Y - 20, 40, 40);
		if (dragBox.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y))
		{
			if (!IsMouseOverDrag)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
			}
			IsMouseOverDrag = true;
			if (Main.mouseLeft && !IsDragging)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				IsDragging = true;
				DragStartMousePos = Main.MouseScreen;
				DragStartPanelPos = MainPanelOrigin;
			}
			if (Main.mouseLeftRelease && IsDragging)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
				IsDragging = false;
			}
		}
		else
		{
			IsMouseOverDrag = false;
		}

		// Restart a new game
		Rectangle restartBox = new Rectangle((int)RestartPanelOffset.X - 20, (int)RestartPanelOffset.Y - 20, 40, 40);
		if (restartBox.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y))
		{
			if (!IsMouseOverRestart)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
			}
			IsMouseOverRestart = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				Restart();
			}
		}
		else
		{
			IsMouseOverRestart = false;
		}
	}

	public void GetOffsets()
	{
		if (IsDragging)
		{
			MainPanelOrigin = DragStartPanelPos + Main.MouseScreen - DragStartMousePos;
		}
		PauseContinuePanelOffset = MainPanelOrigin + new Vector2(340, 200);

		DragPanelOffset = MainPanelOrigin + new Vector2(280, 200);

		RestartPanelOffset = MainPanelOrigin + new Vector2(220, 200);
	}

	public void DrawScoreBar()
	{
		Draw9Pieces(MainPanelOrigin + new Vector2(0, 220), 400, 60, new Color(0.3f, 0.2f, 0.2f), 0);

		// Value display
		Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "Profit: " + Score.ToString(), MainPanelOrigin + new Vector2(-370, 210), new Color(1f, 1f, 0.5f));
	}

	public void UpdateFoodRequest()
	{
		// New order
		if (!Pause && FoodRequests.Count < 5 && Main.rand.NextBool(30))
		{
			Vector3 value = GetRandomFoodType();
			FoodRequestPanel foodRequestPanel = new FoodRequestPanel((int)value.X, (int)value.Y, (int)value.Z);
			foodRequestPanel.AnchorPos = new Vector2(310, 0);
			FoodRequests.Add(foodRequestPanel);
		}
		for (int index = FoodRequests.Count - 1; index >= 0; index--)
		{
			FoodRequestPanel foodRequest = FoodRequests[index];
			foodRequest.Update(index);
			if (!foodRequest.Active && foodRequest.Alpha >= 1f)
			{
				FoodRequests.Remove(foodRequest);
			}
		}
		FoodRequests.Sort((panel1, panel2) => panel1.TimeLeft.CompareTo(panel2.TimeLeft));

		// Iterate through the sorted list and execute CheckFinish() for each panel
		foreach (var panel in FoodRequests)
		{
			panel.CheckFinish();
		}
	}

	public Vector3 GetRandomFoodType()
	{
		switch (Main.rand.Next(3))
		{
			case 0:
				return new Vector3(ModContent.ItemType<Mapo_Tofu>(), 600, 60);
			case 1:
				return new Vector3(ModContent.ItemType<YuxiangEggplant>(), 600, 90);
			case 2:
				return new Vector3(ModContent.ItemType<BoiledBullfrog>(), 600, 130);
		}
		return new Vector3(0);
	}

	public static void AddScore(int Value)
	{
		Score += Value;
		ScoreChangeList.Add(new ScoreChange(Value));
	}

	public void UpdateScore()
	{
		for (int index = ScoreChangeList.Count - 1; index >= 0; index--)
		{
			ScoreChange scoreChange = ScoreChangeList[index];
			scoreChange.Update();
			if (scoreChange.TimeLeft <= 0f)
			{
				ScoreChangeList.Remove(scoreChange);
			}
		}
	}

	/// <summary>
	/// Width is half width, height as well.
	/// </summary>
	/// <param name="anchorCenter"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	/// <param name="color"></param>
	public static void Draw9Pieces(Vector2 anchorCenter, float width, float height, Color color, float alpha, Texture2D texture = default)
	{
		color *= 1 - alpha;
		List<Vertex2D> bars = new List<Vertex2D>();

		if (width > 10 && height > 10)
		{
			AddRectangleBars(bars, anchorCenter + new Vector2(-width, -height), anchorCenter + new Vector2(-width, -height) + new Vector2(10, 10), new Vector2(0, 0), new Vector2(0.2f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(-width, -height) + new Vector2(10, 0), anchorCenter + new Vector2(width, -height) + new Vector2(-10, 10), new Vector2(0.5f, 0), new Vector2(0.5f, 0.2f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, -height) + new Vector2(-10, 0), anchorCenter + new Vector2(width, -height) + new Vector2(0, 10), new Vector2(0.8f, 0), new Vector2(1f, 0.2f), color);

			AddRectangleBars(bars, anchorCenter + new Vector2(-width, -height) + new Vector2(0, 10), anchorCenter + new Vector2(-width, height) + new Vector2(10, -10), new Vector2(0, 0.4f), new Vector2(0.2f, 0.6f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(-width, -height) + new Vector2(10, 10), anchorCenter + new Vector2(width, height) + new Vector2(-10, -10), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, -height) + new Vector2(-10, 10), anchorCenter + new Vector2(width, height) + new Vector2(0, -10), new Vector2(0.8f, 0.2f), new Vector2(1f, 0.8f), color);

			AddRectangleBars(bars, anchorCenter + new Vector2(-width, height) + new Vector2(0, -10), anchorCenter + new Vector2(-width, height) + new Vector2(10, 0), new Vector2(0f, 0.8f), new Vector2(0.2f, 1f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(-width, height) + new Vector2(10, -10), anchorCenter + new Vector2(width, height) + new Vector2(-10, 0), new Vector2(0.5f, 0.8f), new Vector2(0.5f, 1f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, height) + new Vector2(-10, -10), anchorCenter + new Vector2(width, height) + new Vector2(0, 0), new Vector2(0.8f, 0.8f), new Vector2(1f, 1f), color);
		}
		else if (width < 10 && height > 10)
		{
			AddRectangleBars(bars, anchorCenter + new Vector2(-width, -height), anchorCenter + new Vector2(-width, -height) + new Vector2(width, 10), new Vector2(0, 0), new Vector2(0.2f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, -height) + new Vector2(-width, 0), anchorCenter + new Vector2(width, -height) + new Vector2(0, 10), new Vector2(0.8f, 0), new Vector2(1f, 0.2f), color);

			AddRectangleBars(bars, anchorCenter + new Vector2(-width, -height) + new Vector2(0, 10), anchorCenter + new Vector2(-width, height) + new Vector2(width, -10), new Vector2(0, 0.4f), new Vector2(0.2f, 0.6f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, -height) + new Vector2(-width, 10), anchorCenter + new Vector2(width, height) + new Vector2(0, -10), new Vector2(0.8f, 0.2f), new Vector2(1f, 0.8f), color);

			AddRectangleBars(bars, anchorCenter + new Vector2(-width, height) + new Vector2(0, -10), anchorCenter + new Vector2(-width, height) + new Vector2(width, 0), new Vector2(0f, 0.8f), new Vector2(0.2f, 1f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, height) + new Vector2(-width, -10), anchorCenter + new Vector2(width, height) + new Vector2(0, 0), new Vector2(0.8f, 0.8f), new Vector2(1f, 1f), color);
		}
		else if (width > 10 && height < 10)
		{
			AddRectangleBars(bars, anchorCenter + new Vector2(-width, -height), anchorCenter + new Vector2(-width, -height) + new Vector2(10, height), new Vector2(0, 0), new Vector2(0.2f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(-width, -height) + new Vector2(10, 0), anchorCenter + new Vector2(width, -height) + new Vector2(-10, height), new Vector2(0.5f, 0), new Vector2(0.5f, 0.2f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, -height) + new Vector2(-10, 0), anchorCenter + new Vector2(width, -height) + new Vector2(0, height), new Vector2(0.8f, 0), new Vector2(1f, 0.2f), color);

			AddRectangleBars(bars, anchorCenter + new Vector2(-width, height) + new Vector2(0, -height), anchorCenter + new Vector2(-width, height) + new Vector2(10, 0), new Vector2(0f, 0.8f), new Vector2(0.2f, 1f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(-width, height) + new Vector2(10, -height), anchorCenter + new Vector2(width, height) + new Vector2(-10, 0), new Vector2(0.5f, 0.8f), new Vector2(0.5f, 1f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, height) + new Vector2(-10, -height), anchorCenter + new Vector2(width, height) + new Vector2(0, 0), new Vector2(0.8f, 0.8f), new Vector2(1f, 1f), color);
		}
		else
		{
			AddRectangleBars(bars, anchorCenter + new Vector2(-width, -height), anchorCenter + new Vector2(-width, -height) + new Vector2(width, height), new Vector2(0, 0), new Vector2(0.2f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, -height) + new Vector2(-width, 0), anchorCenter + new Vector2(width, -height) + new Vector2(0, height), new Vector2(0.8f, 0), new Vector2(1f, 0.2f), color);

			AddRectangleBars(bars, anchorCenter + new Vector2(-width, height) + new Vector2(0, -height), anchorCenter + new Vector2(-width, height) + new Vector2(width, 0), new Vector2(0f, 0.8f), new Vector2(0.2f, 1f), color);
			AddRectangleBars(bars, anchorCenter + new Vector2(width, height) + new Vector2(-width, -height), anchorCenter + new Vector2(width, height) + new Vector2(0, 0), new Vector2(0.8f, 0.8f), new Vector2(1f, 1f), color);
		}
		if (texture == default)
		{
			texture = ModAsset.FoodRequestUIPanel.Value;
		}
		Main.graphics.graphicsDevice.Textures[0] = texture;
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);
	}

	public static void AddRectangleBars(List<Vertex2D> bars, Vector2 posTopLeft, Vector2 posBottomRight, Vector2 coordTopLeft, Vector2 coordBottomRight, Color color)
	{
		bars.Add(posTopLeft, color, new Vector3(coordTopLeft, 0));
		bars.Add(new Vector2(posBottomRight.X, posTopLeft.Y), color, new Vector3(coordBottomRight.X, coordTopLeft.Y, 0));
		bars.Add(new Vector2(posTopLeft.X, posBottomRight.Y), color, new Vector3(coordTopLeft.X, coordBottomRight.Y, 0));

		bars.Add(new Vector2(posBottomRight.X, posTopLeft.Y), color, new Vector3(coordBottomRight.X, coordTopLeft.Y, 0));
		bars.Add(posBottomRight, color, new Vector3(coordBottomRight, 0));
		bars.Add(new Vector2(posTopLeft.X, posBottomRight.Y), color, new Vector3(coordTopLeft.X, coordBottomRight.Y, 0));
	}
}