using Everglow.Food;
using Everglow.Food.Buffs.ModDrinkBuffs;
using Everglow.Sources.Modules.FoodModule.Buffs.ModFoodBuffs;
using Everglow.Sources.Modules.FoodModule.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Food.Items.ModDrink
{
	public class DreamYearning : DrinkBase
	{
		public override DrinkInfo DrinkInfo
		{
			get
			{
				return new DrinkInfo()
				{
					Thirsty = false,
					BuffType = ModContent.BuffType<DreamYearningBuff>(),
					BuffTime = new FoodDuration(0, 10, 0),
					Name = "DreamYearningBuff"
				};
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("魂牵梦萦");

			Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'思君忆君，魂牵梦萦，翠销香暖云屏，更哪堪酒醒。'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(255, 63, 149),
				new Color(52, 211, 239),
				new Color(45, 74, 158)
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
}