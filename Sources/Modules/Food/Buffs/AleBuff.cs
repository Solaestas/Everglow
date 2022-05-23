namespace Everglow.Sources.Modules.Food.Buffs
{
    public class AleBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("AleBuff");
            Description.SetDefault("耍酒疯 \n 短时间内鞭子的范围和速度提升至2.5倍");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.whipRangeMultiplier *= 2.5f;//
            player.GetAttackSpeed(DamageClass.Summon) *= 2.5f;

        }
    }
}

