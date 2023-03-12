﻿using Everglow.Sources.Modules.FoodModule.Utils;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Everglow.Sources.Modules.FoodModule.Buffs.ModFoodBuffs;

namespace Everglow.Sources.Modules.FoodModule.Items.ModFood
{
	public class Durian : FoodBase
	{
        public override FoodInfo FoodInfo
        {
            get
            {
				return new FoodInfo()
				{
					Satiety = 20,
					BuffType = ModContent.BuffType<DurianBuff>(),
					BuffTime = new FoodDuration(6, 0, 0),
					Name = "DurianBuff"
                };
            }
        }
        public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(175, 90, 0),
				new Color(234, 195, 89),
				new Color(99, 82, 0)
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