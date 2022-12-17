namespace Everglow.Sources.Modules.FoodModule.Buffs.ModDrinkBuffs
{
    public class GreenStormBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("GreenStormBuff");
            //Description.SetDefault("短时间内鞭子的范围和速度提升至2.5倍\n“别以为草便宜,没上规模前,它比苹果核桃什么的都贵。”");
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

