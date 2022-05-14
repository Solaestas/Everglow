using Everglow.Sources.Modules.Food.Buffs;

namespace Everglow.Sources.Modules.Food
{
    internal class FoodSystem : ModSystem
    {
        // 对于原版的食物进行类型Id到 FoodInfo 的映射，直接获取FoodInfo实例
        private Dictionary<Item_id, FoodInfo> m_vanillaFoodInfos;

        public Dictionary<Item_id, FoodInfo> VanillaFoodInfos
        {
            get { return m_vanillaFoodInfos; }
        }

        public override void PostSetupContent()
        {
            m_vanillaFoodInfos = new Dictionary<Item_id, FoodInfo>
            {
                // 香蕉奶昔？饱食度20，会给一个弹药箱的buff
                { ItemID.BananaSplit, new FoodInfo() { Satiety = 20, BuffType = BuffID.AmmoBox } },
                // 盒装牛奶 饱食度20，会给一个4防御的buff
                { ItemID.MilkCarton, new FoodInfo() { Satiety = 20, BuffType = ModContent.BuffType<MilkCartonBuff>() } }
            };
            base.PostSetupContent();
        }
    }
}
