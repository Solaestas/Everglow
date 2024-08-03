using Everglow.Food.Items.Ingredients;
using Everglow.Food.Items.ModFood;
using Everglow.Food.UI;

namespace Everglow.Food.UI;

public class SteamBoxUI : PotUI
{
	public Vector2 PanelPos0 = new Vector2(100, -100);
	public Vector2 PanelPos1 = new Vector2(80, -92);
	public Vector2 PanelPos2 = new Vector2(104, -88);
	public Vector2 PanelPos3 = new Vector2(140, -50);
	public Vector2 PanelPos4 = new Vector2(60, -100);

	public static CookingUnit MapoTofu;
	public static CookingUnit YuxiangEggplant;
	public static CookingUnit BoiledBullfrog;

	/// <summary>
	/// Menus of this pot can cook
	/// </summary>
	public static List<CookingUnit> PotMenu = new List<CookingUnit>();

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
		MapoTofu = new CookingUnit(ModContent.ItemType<Mapo_Tofu>(), ModContent.ItemType<Doubanjiang>(), ModContent.ItemType<TofuCubes>(), ModContent.ItemType<ChoppedScallion>(), ModContent.ItemType<SpicyPepperRing>(), ModContent.ItemType<SichuanPepper>(), ModContent.ItemType<GroundMeat>());
		YuxiangEggplant = new CookingUnit(ModContent.ItemType<YuxiangEggplant>(), ModContent.ItemType<GroundMeat>(), ModContent.ItemType<MincedGarlic>(), ModContent.ItemType<ChoppedScallion>(), ModContent.ItemType<Doubanjiang>(), ModContent.ItemType<Rice>(), ModContent.ItemType<EggplantCubes>());
		BoiledBullfrog = new CookingUnit(ModContent.ItemType<BoiledBullfrog>(), ModContent.ItemType<FrogMeat>(), ModContent.ItemType<SichuanPepper>(), ModContent.ItemType<SpicyPepperRing>(), ModContent.ItemType<ChoppedScallion>());
		PotMenu.Add(MapoTofu);
		PotMenu.Add(YuxiangEggplant);
		PotMenu.Add(BoiledBullfrog);
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
		CuisineType = CheckCuisineType();
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

	public int CheckCuisineType()
	{
		foreach (var cookingUnit in PotMenu)
		{
			if (CanCook(cookingUnit))
			{
				return cookingUnit.Type;
			}
		}
		return -1;
	}
}