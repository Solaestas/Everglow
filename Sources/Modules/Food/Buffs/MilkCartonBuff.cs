namespace Everglow.Sources.Modules.Food.Buffs
{
    public class MilkCartonBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("MilkCarton Buff");
            Description.SetDefault("一奶解百毒 \n 短时间内免疫所有buff和debuff ");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 0; i < 114514; i++)
            {
                if (i != ModContent.BuffType<MilkCartonBuff>())
                {
                    player.buffImmune[i] = true;
                }
                else
                {
                    player.buffImmune[i] = false;
                }

            }

        }
    }
}
