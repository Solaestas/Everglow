using Everglow.Food.Items.Ingredients;
using Everglow.Food.Items.ModFood;

namespace Everglow.Food.FoodRecipes;

public class CasseroleRecipe : FoodRecipes
{
	public CasseroleRecipe()
	{
		CookingUnitWithOrderMenu = new List<CookingUnitWithOrder>
		{
		};

		CookingUnitMenu = new List<CookingUnit>
		{
			// 麻婆豆腐
			{
				new CookingUnit(
					ModContent.ItemType<MapoTofu>(),
					1,
					[ModContent.ItemType<Doubanjiang>()],
					[ModContent.ItemType<TofuCubes>()],
					[ModContent.ItemType<ChoppedScallion>()],
					[ModContent.ItemType<SpicyPepperRing>()],
					[ModContent.ItemType<SichuanPepper>()],
					[ModContent.ItemType<GroundMeat>()])
			},

			// 鱼香茄子
			{
				new CookingUnit(
					ModContent.ItemType<YuxiangEggplant>(),
					1,
					[ModContent.ItemType<GroundMeat>()],
					[ModContent.ItemType<MincedGarlic>()],
					[ModContent.ItemType<ChoppedScallion>()],
					[ModContent.ItemType<Doubanjiang>()],
					[ModContent.ItemType<Rice>()],
					[ModContent.ItemType<EggplantCubes>()])
			},

			// 煮牛蛙
			{
				new CookingUnit(
					ModContent.ItemType<BoiledBullfrog>(),
					1,
					[ModContent.ItemType<FrogMeat>()],
					[ModContent.ItemType<SichuanPepper>()],
					[ModContent.ItemType<SpicyPepperRing>()],
					[ModContent.ItemType<ChoppedScallion>()])
			},
		};
	}
}