using Everglow.Sources.Modules.FoodModule.Buffs.ModDrinkBuffs;
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
	public class DryMartini :   DrinkBase
    {
        public override  DrinkInfo DrinkInfo
        {
			get
			{
				return new DrinkInfo()
				{
					Thirsty = false,
					BuffType = ModContent.BuffType<DryMartiniBuff>(),
					BuffTime = new FoodDuration(0, 10, 0),
					Name = "DryMartiniBuff"
                };
            }
        }
        public override void SetStaticDefaults() {


			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

			ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
				new Color(192, 182, 72),
				new Color(137, 124, 140),
				new Color(194, 229, 96)
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