namespace Everglow.Sources.Modules.FoodModule.Buffs.VanillaFoodBuffs
{
    public class SeafoodDinnerBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("SeafoodDinnerBuff");
            //Description.SetDefault("增加8%暴击，伤害，攻速，爆伤\n“够生猛！”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetCritChance(DamageClass.Generic) += 8; // 加8%暴击
            player.GetDamage(DamageClass.Generic) *= 1.08f; // 加8%伤害
            player.GetAttackSpeed(DamageClass.Generic) *= 1.12f; // 加8%攻速
            
        }
    }
}

