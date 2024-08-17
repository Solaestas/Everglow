using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Food.Items.Ingredients;
using Everglow.Food.Items.ModFood;

namespace Everglow.Food.FoodRecipes;

public class FoodRecipes : ModSystem
{

	public List<CookingUnitWithOrder> CookingUnitWithOrderMenu;
	public List<CookingUnit> CookingUnitMenu;

	public override void Unload()
	{
		CookingUnitWithOrderMenu.Clear();
		CookingUnitMenu.Clear();
	}

	public struct CookingUnitWithOrder
	{
		public int[][] Ingredients;
		public int Type;
		public int Num;

		public CookingUnitWithOrder(int type, int num , params int[][] itemgroup)
		{
			Ingredients = itemgroup;

			Type = type;

			Num = num;
		}
	}

	public struct CookingUnit
	{
		public List<int[]> Ingredients;
		public int Type;
		public int Num;

		public CookingUnit(int type, int num, params int[][] itemgroup)
		{

			Ingredients = itemgroup.ToList();

			Type = type;

			Num = num;
		}
	}
}
