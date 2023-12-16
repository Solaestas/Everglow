using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Everglow.Food.Buffs.ModFoodBuffs;
using Everglow.Food.FoodUtilities;

namespace Everglow.Food.Items.ModFood;
public class BakedMussel : FoodBase
{
	public override FoodInfo FoodInfo
	{
		get
		{
			return new FoodInfo()
			{
				Satiety = 20,
				BuffType = ModContent.BuffType<BakedMusselBuff>(),
				BuffTime = new FoodDuration(6, 0, 0),
				Name = "BakedMusselBuff"
			};
		}
	}
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

		Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

		ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
			new Color(255, 171, 45),
			new Color(15, 6, 0),
			new Color(137, 75, 34)
		};

		ItemID.Sets.IsFood[Type] = true;
	}

	public override void SetDefaults()
	{

		Item.DefaultToFood(22, 22, BuffID.WellFed3, 57600);
		Item.value = Item.buyPrice(0, 3);
		Item.rare = ItemRarityID.Blue;
	}
}