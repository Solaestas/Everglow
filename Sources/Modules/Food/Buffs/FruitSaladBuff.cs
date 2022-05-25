namespace Everglow.Sources.Modules.Food.Buffs
{
    public class FruitSaladBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FruitSaladBuff");
            Description.SetDefault("提升大部分属性\n“长久的维生素！”");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 4; // 加3防御
            player.GetCritChance(DamageClass.Generic) += 3; // 加4%暴击
            player.GetDamage(DamageClass.Generic) *= 1.03f; // 加4%伤害
            player.GetAttackSpeed(DamageClass.Generic) += 0.03f; // 加4%攻速
            player.lifeRegen += 2; // 加2生命回复
            player.manaRegen += 4; // 魔力再生加4
            player.maxRunSpeed *= 1.2f;//加速
            player.runAcceleration *= 1.2f;
            player.jumpSpeedBoost += 2;
        }
    }
}

