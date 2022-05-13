using Everglow.Sources.Modules.Food.Items;

namespace Everglow.Sources.Modules.Food
{
    public class FoodGlobalItem : GlobalItem
    {
        // 对于原版的食物进行类型Id到 FoodInfo 的映射，直接获取FoodInfo实例
        private static Dictionary<Item_id, FoodInfo> m_vanillaFoodInfos;

        public FoodGlobalItem()
        {
            m_vanillaFoodInfos = new Dictionary<Item_id, FoodInfo>
            {
                // 香蕉奶昔？饱食度20，会给一个弹药箱的buff
                { ItemID.BananaSplit, new FoodInfo() { Satiety = 20, BuffType = BuffID.AmmoBox } }
            };
        }

        public override void SetDefaults(Item item)
        {
            // 如果是原版的食物，那么就手动处理
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];

                // 替换掉原版的 buff 类型
                item.buffType = foodInfo.BuffType;
            }
            base.SetDefaults(item);
        }

        public override bool? UseItem(Item item, Player player)
        {
            // 如果是原版的食物，那么就手动处理，因为已经使用了物品，说明玩家满足饱食度要求
            if (m_vanillaFoodInfos.ContainsKey(item.type))
            {
                var foodInfo = m_vanillaFoodInfos[item.type];
                var foodPlayer = player.GetModPlayer<FoodModPlayer>();

                // 增加饱食度，并且应用一些特效
                foodPlayer.CurrentSatiety += foodInfo.Satiety;
                Main.NewText($"Added {foodInfo.Satiety}! Current Satiety {foodPlayer.CurrentSatiety} / {foodPlayer.MaximumSatiety}");
            }
            return true;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            var foodPlayer = player.GetModPlayer<FoodModPlayer>();
            // 判断能否吃下物品
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
