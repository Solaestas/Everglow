using Everglow.Food.Buffs.ModDrinkBuffs;
using Everglow.Food.FoodUtilities;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Food.Items.ModDrink;

public class B_52 : DrinkBase
{
	public override DrinkInfo DrinkInfo
	{
		get
		{
			return new DrinkInfo()
			{
				Thirsty = false,
				BuffType = ModContent.BuffType<B_52Buff>(),
				BuffTime = new FoodDuration(0, 10, 0),
				Name = "B_52Buff"
			};
		}
	}
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

		Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

		ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
			new Color(94, 75, 170),
			new Color(255, 229, 0),
			new Color(160, 85, 87)
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