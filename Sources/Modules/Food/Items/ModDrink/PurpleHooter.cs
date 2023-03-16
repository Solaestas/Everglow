using Everglow.Food.Buffs.ModDrinkBuffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Food.Items.ModDrink;

public class PurpleHooter : DrinkBase
{
	public override DrinkInfo DrinkInfo
	{
		get
		{
			return new DrinkInfo()
			{
				Thirsty = false,
				BuffType = ModContent.BuffType<PurpleHooterBuff>(),
				BuffTime = new FoodDuration(0, 10, 0),
				Name = "PurpleHooterBuff"
			};
		}
	}
	public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("紫色汽笛");

		Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'高贵与深沉'");

		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
		Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
		ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
			new Color(121, 26, 153),
			new Color(35, 173, 188),
			new Color(53, 22, 153)
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