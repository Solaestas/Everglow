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
	public class PiercoldWind :   DrinkBase
    {
        public override  DrinkInfo DrinkInfo
        {
			get
			{
				return new DrinkInfo()
				{
					Thirsty = false,
					BuffType = ModContent.BuffType<PiercoldWindBuff>(),
					BuffTime = new FoodDuration(0, 10, 0),
					Name = "PiercoldWindBuff"
                };
            }
        }
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("寒风凛冽");

			Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'长啸出原野，凛然寒风生。'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(70, 45, 181),
				new Color(74, 173, 226),
				new Color(53, 86, 161)
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