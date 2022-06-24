namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class BananaSplitBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("BananaSplitBuff");
            //Description.SetDefault("33%不消耗弹药，增加8%远程暴击\n“低血压”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            FoodBuffModPlayer FoodBuffModPlayer = player.GetModPlayer<FoodBuffModPlayer>();
            FoodBuffModPlayer.BananaSplitBuff = true;
            player.GetCritChance(DamageClass.Ranged) += 8; // 加8%暴击
            player.wellFed = true;
        }
    }
}

