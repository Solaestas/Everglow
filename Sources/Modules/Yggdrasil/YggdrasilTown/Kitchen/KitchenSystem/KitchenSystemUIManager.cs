using Everglow.Food.Items.ModFood;
using ReLogic.Graphics;
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

	public static void Reset()
	{
		FoodRequests = new List<FoodRequestPanel>();
		ScoreChangeList = new List<ScoreChange>();
		Score = 0;
	}

	public override bool DrawSelf()
	{
		if (!Main.gamePaused)
		{
			Update();
		}

		Draw9Pieces(new Vector2(530, Main.screenHeight - 300), 400, 160, new Color(0.3f, 0.2f, 0.2f), 0);
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
		return true;
	}

	public void Update()
	{
		UpdateFoodRequest();
		UpdateScore();
	}

	public void DrawScoreBar()
	{
		Draw9Pieces(new Vector2(530, Main.screenHeight - 80), 400, 60, new Color(0.3f, 0.2f, 0.2f), 0);

		// Value display
		Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "Profit: " + Score.ToString(), new Vector2(160, Main.screenHeight - 90), new Color(1f, 1f, 0.5f));
	}

	public void UpdateFoodRequest()
	{
		if (FoodRequests.Count < 5 && Main.rand.NextBool(30))
		{
			Vector3 value = GetRandomFoodType();
			FoodRequestPanel foodRequestPanel = new FoodRequestPanel((int)value.X, (int)value.Y, (int)value.Z);
			foodRequestPanel.AnchorPos = new Vector2(FoodRequests.Count * 160 + 200, Main.screenHeight - 300);
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