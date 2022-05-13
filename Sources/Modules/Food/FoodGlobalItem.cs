using Everglow.Sources.Modules.Food.Items;

namespace Everglow.Sources.Modules.Food
{
    public class FoodGlobalItem : GlobalItem
    {
        private static Dictionary<Item_id, FoodInfo> m_vanillaFoodInfos;

        public FoodGlobalItem()
        {
            m_vanillaFoodInfos = new Dictionary<Item_id, FoodInfo>
            {
                { ItemID.BananaSplit, new FoodInfo() { Satiety = 20, BuffType = BuffID.AmmoBox } }
            };
        }

        public override void SetDefaults(Item item)
        {
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];
                item.buffType = foodInfo.BuffType;
            }
            base.SetDefaults(item);
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];
                var foodPlayer = player.GetModPlayer<FoodModPlayer>();
                foodPlayer.CurrentSatiety += foodInfo.Satiety;
                Main.NewText($"Added {foodInfo.Satiety}! Current Satiety {foodPlayer.CurrentSatiety} / {foodPlayer.MaximumSatiety}");
            }
            return true;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            var foodPlayer = player.GetModPlayer<FoodModPlayer>();
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];
                if (!foodPlayer.CanEat(foodInfo))
                {
                    Main.NewText($"Cannot eat this!");
                    return false;
                }
            }
            else if(item.ModItem is FoodBase)
            {
                var foodItem = item.ModItem as FoodBase;
                var foodInfo = foodItem.FoodInfo;
                if (!foodPlayer.CanEat(foodInfo))
                {
                    Main.NewText($"Cannot eat this!");
                    return false;
                }
            }
            return base.CanUseItem(item, player);
        }
    }
}
