using Everglow.Food.Items.Ingredients;
using Everglow.Food.Items.ModFood;

namespace Everglow.Food.Tiles;

public class CasseroleUI : PotUI
{
	public Vector2 PanelPos0 = new Vector2(100, -100);
	public Vector2 PanelPos1 = new Vector2(80, -92);
	public Vector2 PanelPos2 = new Vector2(104, -88);
	public Vector2 PanelPos3 = new Vector2(140, -50);
	public Vector2 PanelPos4 = new Vector2(60, -100);
	public static List<CookingUnit> CassroleMenu = new List<CookingUnit>();

	public struct CookingUnit
	{
		public List<int> Ingredients;
		public int Type;

		public CookingUnit(int type, int item1 = -1, int item2 = -1, int item3 = -1, int item4 = -1, int item5 = -1, int item6 = -1)
		{
			Ingredients = new List<int>
			{
				item1,
				item2,
				item3,
				item4,
				item5,
				item6,
			};

			Type = type;
		}
	}

	public CasseroleUI(Point anchorTilePos)
		: base(anchorTilePos)
	{
	}

	public override void SetDefault(int count)
	{
		int maxSlotCount = 6;
		Ingredients = new int[maxSlotCount];
		IngredientsSlotPos = new Vector2[maxSlotCount];
		for (int y = 0; y < 2; y++)
		{
			for (int x = 0; x < 3; x++)
			{
				int index = 3 * y + x;
				IngredientsSlotPos[index] = new Vector2(x - 1, y - 0.3f) * 50;
				Ingredients[index] = -1;
			}
		}
		MaxSlotCount = maxSlotCount;
		CassroleMenu.Add(MapoTofu);
		CassroleMenu.Add(YuxiangEggplant);
		CassroleMenu.Add(BoiledBullfrog);
	}

	public override void Update()
	{
		if (Maximized)
		{
			PanelPos0 = new Vector2(140, -90);
			PanelPos1 = new Vector2(-60, 82);
			PanelPos2 = new Vector2(40, 82);
			PanelPos3 = new Vector2(-10, 82);
			PanelPos4 = new Vector2(100, -90);
			for (int y = 0; y < 2; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					int index = 3 * y + x;
					IngredientsSlotPos[index] = new Vector2(x - 1, y - 0.3f) * 50;
				}
			}
		}
		else
		{
			PanelPos0 = new Vector2(44, -14);
			PanelPos1 = new Vector2(-34, 70);
			PanelPos2 = new Vector2(14, 70);
			PanelPos3 = new Vector2(-10, 70);
			PanelPos4 = new Vector2(20, -14);
			for (int y = 0; y < 2; y++)
			{
				for (int x = 0; x < 3; x++)
				{
					int index = 3 * y + x;
					IngredientsSlotPos[index] = new Vector2(x - 1, y + 1) * 30;
				}
			}
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
		Texture2D casserole = ModAsset.CasseroleUIPanel.Value;
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
		foreach (var cookingUnit in CassroleMenu)
		{
			if (CanCook(cookingUnit))
			{
				return cookingUnit.Type;
			}
		}
		return -1;
	}

	public CookingUnit MapoTofu = new CookingUnit(ModContent.ItemType<Mapo_Tofu>(), ModContent.ItemType<Doubanjiang>(), ModContent.ItemType<TofuCubes>(), ModContent.ItemType<ChoppedScallion>(), ModContent.ItemType<SpicyPepperRing>(), ModContent.ItemType<SichuanPepper>(), ModContent.ItemType<GroundMeat>());
	public CookingUnit YuxiangEggplant = new CookingUnit(ModContent.ItemType<YuxiangEggplant>(), ModContent.ItemType<GroundMeat>(), ModContent.ItemType<MincedGarlic>(), ModContent.ItemType<ChoppedScallion>(), ModContent.ItemType<Doubanjiang>(), ModContent.ItemType<Rice>(), ModContent.ItemType<EggplantCubes>());
	public CookingUnit BoiledBullfrog = new CookingUnit(ModContent.ItemType<BoiledBullfrog>(), ModContent.ItemType<FrogMeat>(), ModContent.ItemType<SichuanPepper>(), ModContent.ItemType<SpicyPepperRing>(), ModContent.ItemType<ChoppedScallion>());
}