using Everglow.Food.Items.Ingredients;
using Everglow.Food.Items.ModFood;
using Everglow.Food.UI;
using static Everglow.Food.UI.PotUI;

namespace Everglow.Food.UI;

public class SteamBoxUI : PotUI
{
	public Vector2 PanelPos0 = new Vector2(100, -100);
	public Vector2 PanelPos1 = new Vector2(80, -92);
	public Vector2 PanelPos2 = new Vector2(104, -88);
	public Vector2 PanelPos3 = new Vector2(140, -50);
	public Vector2 PanelPos4 = new Vector2(60, -100);

	public static CookingUnitWithOrder XiaoLongBao;

	/// <summary>
	/// Menus of this pot can cook
	/// </summary>
	public static List<CookingUnit> PotMenu = new List<CookingUnit>();
	public static List<CookingUnitWithOrder> PotMenuWithOrder = new List<CookingUnitWithOrder>();

	public SteamBoxUI(Point anchorTilePos)
		: base(anchorTilePos)
	{
	}

	public override void SetDefault(int count)
	{
		int maxSlotCount = 7;
		Ingredients = new int[maxSlotCount];
		IngredientsSlotPos = new Vector2[maxSlotCount];
		for (int i = 0; i < 6; i++)
		{
				IngredientsSlotPos[i] = new Vector2(i - 2.5f, 0.1f) * 50;
				Ingredients[i] = -1;
		}
		IngredientsSlotPos[6] = new Vector2(0, 0.5f) * 50;
		Ingredients[6] = -1;
		MaxSlotCount = maxSlotCount;
		if (PotMenu.Count == 0)
		{
			SetMenu();
		}
	}

	public static void SetMenu()
	{
		PotMenu.Clear();
		PotMenuWithOrder.Clear();
		int[] XiaoLongBaoIngredients = new int[]
		{
			ModContent.ItemType<RawXiaoLongBao>(),
			ModContent.ItemType<RawXiaoLongBao>(),
			ModContent.ItemType<RawXiaoLongBao>(),
			ModContent.ItemType<RawXiaoLongBao>(),
			ModContent.ItemType<RawXiaoLongBao>(),
			ModContent.ItemType<RawXiaoLongBao>(),
			ItemID.BottledWater,
		};
		XiaoLongBao = new CookingUnitWithOrder(ModContent.ItemType<XiaoLongBao>(), XiaoLongBaoIngredients);
		PotMenuWithOrder.Add(XiaoLongBao);
	}

	public override void Update()
	{
		if (Maximized)
		{
			PanelPos0 = new Vector2(140, -110);
			PanelPos1 = new Vector2(-60, 82);
			PanelPos2 = new Vector2(40, 82);
			PanelPos3 = new Vector2(-10, 82);
			PanelPos4 = new Vector2(100, -110);
			for (int i = 0; i < 6; i++)
			{
				IngredientsSlotPos[i] = new Vector2(i - 2.5f, 0.1f) * 50;
			}
			IngredientsSlotPos[6] = new Vector2(0, 1f) * 50;
		}
		else
		{
			PanelPos0 = new Vector2(44, -94);
			PanelPos1 = new Vector2(-34, 70);
			PanelPos2 = new Vector2(14, 70);
			PanelPos3 = new Vector2(-10, 70);
			PanelPos4 = new Vector2(20, -94);
			for (int y = 0; y < 2; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					int index = 3 * y + x;
					IngredientsSlotPos[index] = new Vector2(x - 1, y - 1) * 30;
				}
			}
			IngredientsSlotPos[6] = new Vector2(0, 1f) * 50;
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
		Texture2D casserole = ModAsset.SteamBoxUIPanel.Value;
		Main.spriteBatch.Draw(casserole, drawPos, null, Color.White, 0, casserole.Size() * 0.5f, 2, SpriteEffects.None, 0);
	}

	public bool CanCook(CookingUnit cookingUnit)
	{
		List<int> ingredientsCopy = [.. cookingUnit.Ingredients];
		foreach (int type in Ingredients)
		{
			if (ingredientsCopy.Contains(type))
			{
				ingredientsCopy.Remove(type);
			}
		}
		if (ingredientsCopy.Count == 0)
		{
			return true;
		}
		return false;
	}

	public bool CanCookWithOrder(CookingUnitWithOrder cookingUnitwithorder)
	{
		if (cookingUnitwithorder.Ingredients.SequenceEqual(Ingredients))
		{
			return true;
		}
		return false;
	}

	public Tuple<int, int>  CheckCuisineType()
	{
		foreach (var cookingUnit in PotMenu)
		{
			if (CanCook(cookingUnit))
			{
				return new Tuple<int, int>(cookingUnit.Type, cookingUnit.Num);
			}
		}
		foreach (var cookingUnitwithorder in PotMenuWithOrder)
		{
			if (CanCookWithOrder(cookingUnitwithorder))
			{
				return new Tuple<int, int>(cookingUnitwithorder.Type, cookingUnitwithorder.Num);
			}
		}
		return new Tuple<int, int>(-1, -1);
	}
}