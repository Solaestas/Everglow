using Everglow.Food.Items.Ingredients;
using Everglow.Food.Items.ModFood;
using static Everglow.Food.FoodRecipes.FoodRecipes;
using Everglow.Food.UI;
using Everglow.Food.FoodRecipes;


namespace Everglow.Food.UI;

public class SteamBoxUI2 : PotUI
{
	public Vector2 PanelPos0 = new Vector2(100, -100);
	public Vector2 PanelPos1 = new Vector2(80, -92);
	public Vector2 PanelPos2 = new Vector2(104, -88);
	public Vector2 PanelPos3 = new Vector2(140, -50);
	public Vector2 PanelPos4 = new Vector2(60, -100);




	public SteamBoxUI2(Point anchorTilePos)
		: base(anchorTilePos)
	{
	}

	public override void SetDefault(int count)
	{
		int maxSlotCount = 13;
		Ingredients = new int[maxSlotCount];
		IngredientsSlotPos = new Vector2[maxSlotCount];
		for (int i = 0; i < 6; i++)
		{
			IngredientsSlotPos[i] = new Vector2(i - 2.5f, 0.1f) * 50;
			Ingredients[i] = -1;
		}
		for (int i = 0; i < 6; i++)
		{
			IngredientsSlotPos[i + 6] = new Vector2(i - 2.5f, -0.9f) * 50;
			Ingredients[i + 6] = -1;
		}
		IngredientsSlotPos[12] = new Vector2(0, 1.2f) * 50;
		Ingredients[12] = -1;
		MaxSlotCount = maxSlotCount;
		if (PotMenu.Count == 0)
		{
			SetMenu();
		}
	}

	public static void SetMenu()
	{
		SteamBox2Recipe SteamBox2Recipe = new SteamBox2Recipe();
		PotMenuWithOrder = SteamBox2Recipe.CookingUnitWithOrderMenu;
		PotMenu = SteamBox2Recipe.CookingUnitMenu;
	}

	public override void Update()
	{
		if (Maximized)
		{
			PanelPos0 = new Vector2(140, -150);
			PanelPos1 = new Vector2(-60, 132);
			PanelPos2 = new Vector2(40, 132);
			PanelPos3 = new Vector2(-10, 132);
			PanelPos4 = new Vector2(100, -150);
			for (int i = 0; i < 6; i++)
			{
				IngredientsSlotPos[i] = new Vector2(i - 2.5f, 0.1f) * 50;
			}
			for (int i = 0; i < 6; i++)
			{
				IngredientsSlotPos[i + 6] = new Vector2(i - 2.5f, -0.9f) * 50;
			}
			IngredientsSlotPos[12] = new Vector2(0, 1.2f) * 50;

		}
		else
		{
			PanelPos0 = new Vector2(44, -124);
			PanelPos1 = new Vector2(-34, 70);
			PanelPos2 = new Vector2(14, 70);
			PanelPos3 = new Vector2(-10, 70);
			PanelPos4 = new Vector2(20, -124);
			for (int y = 0; y < 4; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					int index = 3 * y + x;
					IngredientsSlotPos[index] = new Vector2(x - 1, y - 2.4f) * 30;
				}
			}
			IngredientsSlotPos[12] = new Vector2(0, 1f) * 50;
		}
		if (CookTimer > 0)
		{
			ClosePanelPos = Vector2.Lerp(ClosePanelPos, Vector2.zeroVector, 0.3f);
			RemovePanelPos = Vector2.Lerp(RemovePanelPos, Vector2.zeroVector, 0.3f);
			ClearPanelPos = Vector2.Lerp(ClearPanelPos, Vector2.zeroVector, 0.3f);
			CookPanelPos = Vector2.Lerp(CookPanelPos, Vector2.zeroVector, 0.3f);
			MaximizePanelPos = Vector2.Lerp(MaximizePanelPos, Vector2.zeroVector, 0.3f);
		}
		else
		{
			ClosePanelPos = Vector2.Lerp(ClosePanelPos, PanelPos0, 0.3f);
			RemovePanelPos = Vector2.Lerp(RemovePanelPos, PanelPos1, 0.3f);
			ClearPanelPos = Vector2.Lerp(ClearPanelPos, PanelPos2, 0.3f);
			CookPanelPos = Vector2.Lerp(CookPanelPos, PanelPos3, 0.3f);
			MaximizePanelPos = Vector2.Lerp(MaximizePanelPos, PanelPos4, 0.3f);
		}

		base.Update();
	}

	public override void Cook()
	{
		CuisineType = CheckCuisineType().Item1;
		CuisineNum = CheckCuisineType().Item2;
		base.Cook();
	}

	public override void DrawMainPanel()
	{
		Vector2 drawPos = GetDrawPos();
		Texture2D casserole = ModAsset.SteamBoxUI2Panel.Value;
		Main.spriteBatch.Draw(casserole, drawPos, null, Color.White, 0, casserole.Size() * 0.5f, 2, SpriteEffects.None, 0);
	}


	public bool CanCookWithOrder(CookingUnitWithOrder cookingUnitwithorder)
	{
		for (int index = 0; index < cookingUnitwithorder.Ingredients.Length; index++)
		{
			if (!cookingUnitwithorder.Ingredients[index].Contains(Ingredients[index]))
			{
				return false;
			}
		}
		return true;
	}

	public bool CanCook(CookingUnit cookingUnit)
	{
		List<int[]> ingredientsCopy = [.. cookingUnit.Ingredients];
		List<int> CopyInUI = Ingredients.ToList();
		for (int i = 0; i < ingredientsCopy.Count(); i++)
		{
			Main.NewText(ingredientsCopy[i][0]);
		}

		foreach (int type in Ingredients)
		{
			if (type == -1) // 如果槽内为空则不管
			{
				continue;
			}
			else // 如果槽内有东西则检测是否在配方组中
			{
				foreach (int[] group in cookingUnit.Ingredients)
				{
					if (group.Contains(type))
					{
						if (!ingredientsCopy.Remove(group)) // 如果配方组里面没有则return false
						{
							return false;
						}
						CopyInUI.Remove(type);
						break;
					}
				}
			}
		}
		CopyInUI.RemoveAll(n => n == -1);

		if (ingredientsCopy.Count == 0 && CopyInUI.Count == 0)
		{
			return true;
		}
		return false;
	}

	public Tuple<int, int> CheckCuisineType()
	{

		foreach (var cookingUnitwithorder in PotMenuWithOrder)
		{
			if (CanCookWithOrder(cookingUnitwithorder))
			{
				return new Tuple<int, int>(cookingUnitwithorder.Type, cookingUnitwithorder.Num);
			}
		}
		foreach (var cookingUnit in PotMenu)
		{
			if (CanCook(cookingUnit))
			{
				return new Tuple<int, int>(cookingUnit.Type, cookingUnit.Num);
			}
		}
		return new Tuple<int, int>(-1, -1);
	}
}