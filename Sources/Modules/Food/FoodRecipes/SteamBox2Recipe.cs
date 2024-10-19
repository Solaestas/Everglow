using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Food.Buffs.VanillaDrinkBuffs;
using Everglow.Food.FoodUtilities;
using Everglow.Food.Items.Ingredients;
using Everglow.Food.Items.ModFood;
using static Everglow.Food.FoodRecipes.FoodRecipes;

namespace Everglow.Food.FoodRecipes;

public class SteamBox2Recipe : FoodRecipes
{
	public SteamBox2Recipe()
	{
		CookingUnitWithOrderMenu = new List<CookingUnitWithOrder>
		{
                // 小笼包*2
            {
				new CookingUnitWithOrder(
					ModContent.ItemType<XiaoLongBao>(),
					2,
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ModContent.ItemType<RawXiaoLongBao>()],
					[ItemID.BottledWater])
            },
		};

		CookingUnitMenu = new List<CookingUnit>
		{
		};
	}










}