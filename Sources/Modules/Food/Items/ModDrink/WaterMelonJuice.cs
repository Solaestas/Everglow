using Everglow.Food.Buffs.ModDrinkBuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Food.Items.ModDrink;

public class WaterMelonJuice : DrinkBase
{
	public override DrinkInfo DrinkInfo
	{
		get
		{
			return new DrinkInfo()
			{
				Thirsty = false,
				BuffType = ModContent.BuffType<WaterMelonJuiceBuff>(),
				BuffTime = new FoodDuration(0, 10, 0),
				Name = "WaterMelonJuiceBuff"
			};
		}
	}
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("西瓜汁");

		Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'浓缩的打击力度'");

		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

		Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
		ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
			new Color(183, 0, 9),
			new Color(255, 175, 79),
			new Color(255, 0, 12)
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