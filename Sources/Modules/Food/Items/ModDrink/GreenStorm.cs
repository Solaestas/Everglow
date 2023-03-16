using Everglow.Food.Buffs.ModDrinkBuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Food.Items.ModDrink;

public class GreenStorm : DrinkBase
{
	public override DrinkInfo DrinkInfo
	{
		get
		{
			return new DrinkInfo()
			{
				Thirsty = false,
				BuffType = ModContent.BuffType<GreenStormBuff>(),
				BuffTime = new FoodDuration(0, 10, 0),
				Name = "GreenStormBuff"
			};
		}
	}
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("绿色风暴");

		Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'别以为草便宜,没上规模前,它比苹果核桃什么的都贵。'");

		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

		Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

		ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
			new Color(255, 89, 111),
			new Color(169, 216, 147),
			new Color(174, 192, 192)
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