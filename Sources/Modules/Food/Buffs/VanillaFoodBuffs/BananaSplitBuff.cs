using Everglow.Resources.ItemList.Weapons.Ranged;

namespace Everglow.Sources.Modules.FoodModule.Buffs.VanillaFoodBuffs
{
    public class BananaSplitBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("BananaSplitBuff");
            //Description.SetDefault("10%不消耗弹药，增加6%远程暴击\n“低血压”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FoodBuffModPlayer FoodBuffModPlayer = player.GetModPlayer<FoodBuffModPlayer>();
            FoodBuffModPlayer.BananaSplitBuff = true;
            if (Shotguns.vanillaShotguns.Contains(player.HeldItem.type))
            {
                player.GetCritChance(DamageClass.Ranged) += 6;
            }

        }
    }
}

