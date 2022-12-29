﻿using Everglow.Sources.Modules.FoodModule.Buffs.ModDrinkBuffs;
using Everglow.Sources.Modules.FoodModule.Buffs.ModFoodBuffs;
using Everglow.Sources.Modules.FoodModule.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.FoodModule.Items.ModDrink
{
	public class QuinceMarry :   DrinkBase
    {
        public override  DrinkInfo DrinkInfo
        {
			get
			{
				return new DrinkInfo()
				{
					Thirsty = false,
					BuffType = ModContent.BuffType<QuinceMarryBuff>(),
					BuffTime = new FoodDuration(0, 10, 0),
					Name = "QuinceMarryBuff"
                };
            }
        }
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("榅桲玛丽");

			Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'炙热与温柔'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));
			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(44, 130, 41),
				new Color(216, 25, 0),
				new Color(102, 0, 18)
			};

			ItemID.Sets.IsFood[Type] = true; 
		}
		public override void SetDefaults() {	
			Item.DefaultToFood(22, 22, BuffID.WellFed3, 57600); 
			Item.value = Item.buyPrice(0, 3);
			Item.rare = ItemRarityID.Blue;
		}
		
	}
}