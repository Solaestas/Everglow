namespace Everglow.Sources.Modules.Food.Buffs
{
    public class GoldenDelightBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("GoldenDelightBuff");
            Description.SetDefault("金灿灿! \n 攻击造迈达斯，中幅提升大部分属性");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {

            FoodBuffModPlayer FoodBuffModPlayer = player.GetModPlayer<FoodBuffModPlayer>();
            FoodBuffModPlayer.GoldenDelightBuff = true;
            player.statDefense += 3; // 加3防御
            player.GetCritChance(DamageClass.Generic) += 3; // 加3%暴击
            player.GetDamage(DamageClass.Generic).Base += 0.03f; // 加3%伤害
            player.GetAttackSpeed(DamageClass.Generic) += 0.03f; // 加3%攻速
            player.lifeRegen += 1; // 加1生命回复
            player.manaRegen += 3; // 魔力再生加3
            player.maxRunSpeed += 0.05f;//加速
            player.jumpSpeedBoost += 2;
        }
    }
}

