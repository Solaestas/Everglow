using Everglow.Sources.Modules.FoodModule.Buffs.VanillaFoodBuffs;
using Everglow.Sources.Modules.FoodModule;
using Everglow.Sources.Modules.FoodModule.Items;
using Everglow.Sources.Modules.FoodModule.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Everglow.Sources.Modules.FoodModule.Buffs.ModFoodBuffs;

namespace Everglow.Sources.Modules.FoodModule.Items.ModFood
{
	public class EggTart : FoodBase
	{
        public override FoodInfo FoodInfo
        {
            get
            {
				return new FoodInfo()
				{
					Satiety = 10,
					BuffType = ModContent.BuffType<EggTartBuff>(),
					BuffTime = new FoodDuration(4, 0, 0),
					Name = "EggTartBuff"
                };
            }
        }
        public override void SetStaticDefaults() {
			DisplayName.SetDefault("蛋挞");

			Tooltip.SetDefault("{$CommonItemTooltip.MediumStats}\n'这酥皮不错'");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(249, 230, 136),
				new Color(152, 93, 95),
				new Color(174, 192, 192)
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