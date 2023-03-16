using Everglow.Food.Buffs.ModDrinkBuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Food.Items.ModDrink;

public class KiwiJuice : DrinkBase
{
	public override DrinkInfo DrinkInfo
	{
		get
		{
			return new DrinkInfo()
			{
				Thirsty = false,
				BuffType = ModContent.BuffType<KiwiJuiceBuff>(),
				BuffTime = new FoodDuration(0, 10, 0),
				Name = "KiwiJuiceBuff"
			};
		}
	}
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("猕猴桃果汁");

		Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'浓缩的奇异之力'");

		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

		Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

		ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
			new Color(255, 248, 153),
			new Color(80, 221, 37),
			new Color(65, 130, 22)
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