namespace Everglow.Sources.Modules.FoodModule.Buffs.ModDrinkBuffs
{
    public class QuinceMarryBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("AleBuff");
            //Description.SetDefault("短时间内鞭子的范围和速度提升至2.5倍\n“耍酒疯”");
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

