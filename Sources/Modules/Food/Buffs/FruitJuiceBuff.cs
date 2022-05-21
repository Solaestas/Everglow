namespace Everglow.Sources.Modules.Food.Buffs
{
    public class FruitJuiceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FruitJuiceBuff");
            Description.SetDefault("维生素！ \n 小幅提升大部分属性");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 2; // 加2防御
            player.GetCritChance(DamageClass.Generic) += 2; // 加2%暴击
            player.endurance *= 0.95f;// 加5%减伤
            player.GetDamage(DamageClass.Generic).Base += 0.02f; // 加2%伤害
            player.GetAttackSpeed(DamageClass.Generic) += 0.02f; // 加2%攻速
            player.lifeRegen += 1; // 加1生命回复
            player.manaRegen += 2; // 魔力再生加2
            player.maxRunSpeed += 0.05f;//加速
            player.jumpSpeedBoost += 1;

        }
    }
}

